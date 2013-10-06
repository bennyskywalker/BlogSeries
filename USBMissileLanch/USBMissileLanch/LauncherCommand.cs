using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBMissileLanch
{
    public class LauncherCommand
    {
        public Command Command
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public LauncherCommand(Command command, int value)
        {
            this.Command = command;
            this.Value = value;
        }

        public LauncherCommand(Command command)
            : this(command, 0)
        {
        }
    }
}
