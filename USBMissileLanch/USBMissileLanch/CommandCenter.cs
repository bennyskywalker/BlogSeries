using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBMissileLanch
{
    public enum Command
    {
        Up,
        Down,
        Left,
        Right,
        Fire,
        Stop
    }

    public class CommandCenter
    {
        private USBComs usbComs;
        private ILauncherDevice launcherDevice = new DCUSBDevice();

        public CommandCenter()
        {
            usbComs = new USBComs();
            usbComs.Initialize((ushort)launcherDevice.UsageID, (ushort)launcherDevice.UsagePage, 
                               (ushort)launcherDevice.VendorId, (ushort)launcherDevice.DeviceId);
        }

        //Should thread this
        public void RunCommand(LauncherCommand command)
        {
            Task task = new Task(() => { RunCommandTask(command); });
            task.Start();
            return;
        }

        private void RunCommandTask(LauncherCommand command)
        {
            switch(command.Command)
            {
                case Command.Up:
                    SendMoveCommand(launcherDevice.Up, command.Value);
                    break;
                case Command.Down:
                    SendMoveCommand(launcherDevice.Down, command.Value);
                    break;
                case Command.Left:
                    SendMoveCommand(launcherDevice.Left, command.Value);
                    break;
                case Command.Right:
                    SendMoveCommand(launcherDevice.Right, command.Value);
                    break;
                case Command.Fire:
                    Fire(command.Value);
                    break;
                case Command.Stop:
                    SendCommand(launcherDevice.Stop);
                    break;
            }
        }

        private void Fire(int numberOfShots)
        {
            //Thread.Sleep(launcherDevice.WaitBeforeFire);
            for(int i=0;i<(numberOfShots);i++)
            {
                SendCommand(launcherDevice.Fire);
                //Thread.Sleep(launcherDevice.WaitAfterFire);
            }
        }

        private void SendCommand(ushort command)
        {
            var data = launcherDevice.CreateCommand(command);
            usbComs.SendCommand(data);
        }

        private void SendMoveCommand(byte command, int milliseconds)
        {
            if(usbComs.IsReady)
            {
                SendCommand(command);
            }
        }
    }
}
