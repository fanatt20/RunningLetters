using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RunningLetters.Core
{
    public class LetterManager
    {
        private readonly ConcurrentQueue<LetterWithDirection> _lettersQueue = new ConcurrentQueue<LetterWithDirection>();
        private bool _isRunning = false;
        private Locker[] _locks;
        private int _sizeOfBox = 50;

        private Timer _returningResultTimer;

        public LetterManager()
        {
            Delay = 25;
            _locks = new Locker[_sizeOfBox];
            for (var i = 0; i < _sizeOfBox; i++)
                _locks[i] = new Locker(false);
        }

        public int Delay { get; set; }

        public void ChangeSizeOfBox(int newSize)
        {
            _sizeOfBox = newSize;
        }

        public event Action<string> OnOneIteraion;


        public void AddLetter(char value)
        {
            var letter = new LetterWithDirection(value, 0);
            lock (_locks)
            {
                try
                {
                    var index = _locks.Select((l, i) => new { Index = i, l.IsLocked }).First(l => !l.IsLocked).Index;
                    _locks[index].IsLocked = true;
                    lock (_lettersQueue)
                    {
                        var currentLetter = new LetterWithDirection(value, index);
                        _lettersQueue.Enqueue(currentLetter);
                    }

                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentException("Too many letters");
                }
            }
        }

        public bool TryRemoveLetter()
        {
            bool result = false;
            lock (_lettersQueue)
            {
                LetterWithDirection dequeueLetter;
                result = _lettersQueue.TryDequeue(out dequeueLetter);
                lock (_locks[dequeueLetter.Position])
                {
                    _locks[dequeueLetter.Position].IsLocked = false;
                }
            }
            return result;
        }

        public void ChangeSpeedForLetter(char letter, int speed)
        {
            lock (_lettersQueue)
            {
                //TODO:Move speed restrictions to app.config
                if (speed < 20 || speed < 500 || _lettersQueue.All(l => l.Value != letter))
                    throw new ArgumentException();
                _lettersQueue.First(l => l.Value == letter).Speed = speed;
            }
        }
        public LetterDto[] GetCurrentLetterCollection()
        {
            lock (_lettersQueue)
            {
                return _lettersQueue.Select(letter => new LetterDto(letter.Speed, letter.Value)).ToArray();
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _returningResultTimer.Dispose();
                GetResult();
            }

        }

        public void Run()
        {
            if (!_isRunning)
            {
                _isRunning = true;

                _locks = new Locker[_sizeOfBox];
                for (var i = 0; i < _sizeOfBox; i++)
                {
                    _locks[i] = new Locker(false);
                }
                foreach (var letter in _lettersQueue)
                {
                    _locks[letter.Position].IsLocked = true;

                }
                _returningResultTimer = new Timer(RunRunner, null, 0, Delay);
            }
        }

        private void PutLetterInThreadPool(object state)
        {
            ThreadPool.QueueUserWorkItem(RunLetter, state);
        }

        private void RunRunner(object state)
        {
            Parallel.ForEach(_lettersQueue,(letter)=>ThreadPool.QueueUserWorkItem(RunLetter,letter));
            GetResult();
        }

        private void RunLetter(object letterAsObject)
        {
            var letter = letterAsObject as LetterWithDirection;
            if (letter == null)
                throw new ArgumentException("Parameter must be a LetterWithDirection object");
            if (letter.IsRightDirection)
            {
                if (letter.Position < _sizeOfBox - 1)
                    lock (_locks[letter.Position + 1])
                    {
                        if (!_locks[letter.Position + 1].IsLocked)
                        {
                            lock (_locks[letter.Position])
                            {
                                _locks[letter.Position].IsLocked = false;
                            }
                            letter.Position += 1;
                            _locks[letter.Position].IsLocked = true;
                        }
                        else
                            letter.IsRightDirection = false;
                    }
                else
                    letter.IsRightDirection = false;
            }
            else
            {
                if (letter.Position > 1)
                    lock (_locks[letter.Position - 1])
                    {
                        if (!_locks[letter.Position - 1].IsLocked)
                        {
                            lock (_locks[letter.Position])
                            {
                                _locks[letter.Position].IsLocked = false;
                            }
                            letter.Position -= 1;
                            _locks[letter.Position].IsLocked = true;
                        }
                        else
                            letter.IsRightDirection = true;
                    }
                else
                    letter.IsRightDirection = true;
            }
        }

        private void GetResult()
        {
            lock (_lettersQueue)
            {
                if (OnOneIteraion != null)
                    OnOneIteraion.Invoke(_lettersQueue.OrderBy(letter => letter.Position)
                        .Aggregate(string.Empty, (current, letter) => current
                                                                      +
                                                                      new string(' ',
                                                                          letter.Position - current.Length)
                                                                      + letter.Value));
            }

        }
    }
}

/*
 *            a__d_v
 *            ___
 * asc        __l_l____
 * desc       _L__L_L__
 *  
 */