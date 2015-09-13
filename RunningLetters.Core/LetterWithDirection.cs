namespace RunningLetters.Core
{
    internal class LetterWithDirection : Letter
    {
        public LetterWithDirection(char value, int position, bool isRightDirection = false,int speed=20)
            : base(value, position)
        {
            IsRightDirection = isRightDirection;
            Speed = speed;
        }

        public LetterWithDirection(Letter letter)
            : base(letter.Value, letter.Position)
        {
            IsRightDirection = true;
        }

        public bool IsRightDirection { get; set; }
        public int Speed { get; set; }
    }
}