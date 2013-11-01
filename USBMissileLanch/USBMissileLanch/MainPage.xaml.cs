using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.Enumeration.Pnp;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Devices.Usb;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace USBMissileLanch
{
    public class Device
    { 
        public const int Vid = 8483;
        public const int Pid = 4112;

        public const int UsagePage = 1;
        public const int UsageId = 16;
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /*
        private HidDevice device;
        private DeviceInformation deviceInformation;
        private Dictionary<DeviceWatcher, String> mapDeviceWatchersToDeviceSelector;
        */
        private CommandCenter commandCenter = null;

        public MainPage()
        {
            this.InitializeComponent();

            //ToDo:  Need to handle suspenstion, etc.

        }

        private void Button_Connect(object sender, RoutedEventArgs e)
        {
            commandCenter = new CommandCenter();
        }

        private void HandleHolding(LauncherCommand lc, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState == HoldingState.Started)
            {
                commandCenter.RunCommand(lc);
            }
            else if (e.HoldingState == HoldingState.Completed || e.HoldingState==HoldingState.Canceled)
            {
                //Stop
                System.Diagnostics.Debug.WriteLine("Stop Command");
                var stopLC = new LauncherCommand(Command.Stop);
                commandCenter.RunCommand(stopLC);
            }
            return;
        }

        private void button_Up(object sender, HoldingRoutedEventArgs e)
        {
            var lc = new LauncherCommand(Command.Up);
            HandleHolding(lc, e);
        }

        private void button_Down(object sender, HoldingRoutedEventArgs e)
        {
            var lc = new LauncherCommand(Command.Down);
            HandleHolding(lc, e);
        }

        private void button_Left(object sender, HoldingRoutedEventArgs e)
        {
            var lc = new LauncherCommand(Command.Left);
            HandleHolding(lc, e);
        }

        private void button_Right(object sender, HoldingRoutedEventArgs e)
        {
            var lc = new LauncherCommand(Command.Right);
            HandleHolding(lc, e);
        }

        private void button_Stop(object sender, RoutedEventArgs e)
        {
            var lc = new LauncherCommand(Command.Stop);
            commandCenter.RunCommand(lc);
        }

        private void button_Fire(object sender, RoutedEventArgs e)
        {
            var lc = new LauncherCommand(Command.Fire, 1);
            commandCenter.RunCommand(lc);
        }



        /*
        private async void InitializeDevice()
        {
            UInt32 vid = 0x1941;
            UInt32 pid = 0x8021;

            //UsbDevice.GetDeviceSelector() 
            //4d1e55b2-f16f-11cf-88cb-001111000030
            //a5dcbf10-6530-11d2-901f-00c04fb951ed
            string aqs = HidDevice.GetDeviceSelector(0xFFA0, 0x0001);

            var myDevices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqs);


            deviceInformation = myDevices[0];

            Boolean bUSBDeviceConnected = false;

            try
            {
                hidDevice = await HidDevice.FromIdAsync(deviceInformation.Id, FileAccessMode.ReadWrite);
                if(hidDevice!=null)
                {
                    bUSBDeviceConnected = true;
                }
                else
                {
                    var deviceAccessInfo = DeviceAccessInformation.CreateFromId(deviceInformation.Id);
                    Windows.Devices.Enumeration.DeviceAccessStatus deviceAccessStatus = deviceAccessInfo.CurrentStatus;
                    if (deviceAccessStatus == DeviceAccessStatus.DeniedByUser)
                    {
                        System.Diagnostics.Debug.WriteLine("Denied by user");
                    }
                    else if (deviceAccessStatus == Windows.Devices.Enumeration.DeviceAccessStatus.DeniedBySystem)
                    {
                        System.Diagnostics.Debug.WriteLine("Denied by system");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Genearl Error=" + deviceInformation.Id);
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message.ToString());
            }

            if(bUSBDeviceConnected)
            {
                System.Diagnostics.Debug.WriteLine("Opened device for communication.");
            }

        }
        */



    }
}
