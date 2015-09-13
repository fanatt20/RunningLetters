using System;

namespace RunningLetters.Core
{
    public class Letter : IEquatable<Letter>,IComparable<Letter>
    {
        public Letter(char value, int position)
        {
            Value = value;
            Position = position;
        }

        public char Value { get;  set; }

        public int Position { get;  set; }

        public void MovePrevious()
        {
            if (Position > 0) Position--;
            else
                throw new IndexOutOfRangeException("You try to get on negative Position");
        }

        public void MoveNext()
        {
            Position++;
        }

        public bool Equals(Letter other)
        {
            return Value == other.Value && Position == other.Position;
        }

        public int CompareTo(Letter other)
        {
            return Position - other.Position;
        }

        public override bool Equals(object obj)
        {
            var letter = obj as Letter;
            return letter != null && Equals(letter);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value.GetHashCode()*397) ^ Position;
            }
        }
    }
}