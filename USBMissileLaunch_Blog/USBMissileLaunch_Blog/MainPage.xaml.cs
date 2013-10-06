using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace USBMissileLaunch_Blog
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HidDevice hidDevice;
        private DeviceInformation deviceInformation;
 

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Initialize(0x0001, 0xFFA0, 0x1941, 0x8021);
        }

        private void Fire_Click(object sender, RoutedEventArgs e)
        {
            SendData(0x10);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            SendData(0x20);
        }

        public async void Initialize(ushort usageId, ushort usagePage, ushort vendorId, ushort deviceId)
        {
            //Create AQS - Advanced Query Syntax String.
            //This is used to search for a device.
            string aqs = HidDevice.GetDeviceSelector(usagePage, usageId);//, vendorId, deviceId);

            //Find a device with a specific AQS.
            //Leaving out the aqs will find all HID and USB devices
            var myDevices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqs);
            deviceInformation = myDevices[0];

            try
            {
                hidDevice = await HidDevice.FromIdAsync(deviceInformation.Id, FileAccessMode.ReadWrite);
                if (hidDevice != null)
                {
                    SendData(0x40);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message.ToString());
            }
        }

        public async void SendData(ushort data)
        {
            //device.Write(data);
            ushort reportId = 0x00;
            var outReport = hidDevice.CreateOutputReport(reportId);

            var dataWriter = new DataWriter();
            dataWriter.WriteByte((Byte)reportId);
            dataWriter.WriteByte((Byte)data);

            IBuffer newbuf = dataWriter.DetachBuffer();
            byte[] newbufArray = WindowsRuntimeBufferExtensions.ToArray(newbuf);

            try
            {
                byte[] array = WindowsRuntimeBufferExtensions.ToArray(outReport.Data);
                for (int i = 0; i < array.Length && i < newbufArray.Length; i++)
                {
                    array[i] = newbufArray[i];
                }

                IBuffer db2 = WindowsRuntimeBufferExtensions.AsBuffer(array);
                outReport.Data = db2;

                try
                {
                    await hidDevice.SendOutputReportAsync(outReport);
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex2)
            {
            }
            return;
        }


        public void Dispose()
        {
            hidDevice.Dispose();
            hidDevice = null;
        }
    }
}
