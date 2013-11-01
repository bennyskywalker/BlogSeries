using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBMissileLanch
{
    public class DCUSBDevice: ILauncherDevice
    {
        private static readonly byte[] CMD = { 0, 0, 0, 0, 0, 0, 0, 0, 2 };
        private static readonly byte[] UP = { 0, 2, 2, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] DOWN = { 0, 2, 1, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] LEFT = { 0, 2, 4, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] RIGHT = { 0, 2, 8, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] FIRE = { 0, 2, 16, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] STOP = { 0, 2, 32, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] GetStatus = { 0, 1, 0, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] LedOn = { 0, 3, 1, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] LedOff = { 0, 3, 0, 0, 0, 0, 0, 0, 0 };

        public string ModelName
        {
            get 
            {
                return "Dream Cheeky USB Missile Launcher";
            }
        }

        public ushort UsagePage
        {
            get
            {
                return 1;
            }
        }

        public ushort UsageID
        {
            get
            {
                return 0x0010;
            } 
        }

        public ushort VendorId
        {
            get 
            {
                return 8483;
            }
        }

        public ushort DeviceId
        {
            get 
            {
                return 4112;
            }
        }

        public byte[] Down
        {
            get 
            {
                return DOWN;
            }
        }

        public byte[] Up
        {
            get 
            {
                return UP;
            }
        }

        public byte[] Left
        {
            get
            {
                return LEFT;
            }
        }

        public byte[] Right
        {
            get
            {
                return RIGHT;
            }
        }

        public byte[] Stop
        {
            get
            {
                return STOP;
                //return 0x00;
            }
        }

        public byte[] Fire
        {
            get
            {
                return FIRE;
            }
        }

        public int MinNumberOfShots
        {
            get
            {
                return 1;
            }
        }

        public int MaxNumberOfShots
        {
            get
            {
                return 4;
            }
        }

        public int ResetTimeLeft
        {
            get
            {
                return 6000;
            }
        }

        public int ResetTimeDown
        {
            get
            {
                return 1000;
            }
        }

        public int WaitBeforeFire
        {
            get
            {
                return 500;
            }
        }

        public int WaitAfterFire
        {
            get
            {
                return 4500;
            }
        }

        public byte[] CreateCommand(byte[] command)
        {
            return command;
        }

    }
}
