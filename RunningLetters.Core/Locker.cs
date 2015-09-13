using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningLetters.Core
{
    internal class Locker
    {
        public bool IsLocked;

        public Locker(bool isLocked)
        {
            IsLocked = isLocked;
        }
    }
}
