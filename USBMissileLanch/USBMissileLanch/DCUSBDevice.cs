using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBMissileLanch
{
    public class DCUSBDevice: ILauncherDevice
    {
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
                return 0xFFA0;
            }
        }

        public ushort UsageID
        {
            get
            {
                return 0x0001;
            } 
        }

        public ushort VendorId
        {
            get 
            {
                return 0x1941;
            }
        }

        public ushort DeviceId
        {
            get 
            {
                return 0x8021;
            }
        }

        public byte Down
        {
            get 
            {
                return 0x02; 
            }
        }

        public byte Up
        {
            get 
            {
                return 0x01;
            }
        }

        public byte Left
        {
            get
            {
                return 0x04;
            }
        }

        public byte Right
        {
            get
            {
                return 0x08;
            }
        }

        public ushort Stop
        {
            get
            {
                return 0x20;
                //return 0x00;
            }
        }

        public ushort Fire
        {
            get
            {
                return 0x10;
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

        public ushort CreateCommand(ushort command)
        {
            return command;
        }

    }
}
