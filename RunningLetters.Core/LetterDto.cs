using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningLetters.Core
{
   public class LetterDto
    {
        public readonly int Speed;
        public readonly char Value;

        public LetterDto(int speed,char value)
        {
            Speed = speed;
            Value = value;
        }
    }
}
