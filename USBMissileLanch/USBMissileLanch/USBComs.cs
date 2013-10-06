using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;

namespace USBMissileLanch
{
    public class USBComs
    {
        private HidDevice hidDevice;
        private DeviceInformation deviceInformation;
        private bool isRegisteredForInputReportsEvent = false;
        

        public async void Initialize(ushort usageId, ushort usagePage, ushort vendorId, ushort deviceId)
        {
            //Create AQS - Advanced Query Syntax String.
            //This is used to search for a device.
            string aqs = HidDevice.GetDeviceSelector(usagePage, usageId);//, vendorId, deviceId);

            //Find a device with a specific AQS.
            //Leaving out the aqs will find all HID and USB devices
            var myDevices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqs);

            deviceInformation = myDevices[0];

            Boolean bUSBDeviceConnected = false;

            try
            {
                hidDevice = await HidDevice.FromIdAsync(deviceInformation.Id, FileAccessMode.ReadWrite);
                if (hidDevice != null)
                {
                    System.Diagnostics.Debug.WriteLine("Device Connected");
                    bUSBDeviceConnected = true;

                    //Hook up the reports.
                    //All data flow through HID are done through reports.  
                    //Reports are containers for databuffers
                    //Three types of reports in HID:  
                    //  Input:  Source of data FROM the device
                    //  Output:  Sink for data TO the device
                    //  Feature:  Bi-directional and used for confugration and settings
                    //

                    //Register for Input reports
                    if (!isRegisteredForInputReportsEvent)
                    {
                        System.Diagnostics.Debug.WriteLine("Registering for Device Input");
                        //Create a listener for any input reports
                        var inputReportEventHandler = new TypedEventHandler<HidDevice, HidInputReportReceivedEventArgs>(this.OnInputReportEvent);
                        hidDevice.InputReportReceived += inputReportEventHandler;
                        isRegisteredForInputReportsEvent = true;
                    }

                    //??? why are we sending this?
                    SendData(0x40);
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

            if (bUSBDeviceConnected)
            {
                System.Diagnostics.Debug.WriteLine("Opened device for communication.");
            }
        }


        public void Dispose()
        {
            hidDevice.Dispose();
            hidDevice = null;
        }

        public bool IsReady
        {
            get
            {
                return (hidDevice != null);
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
                for (int i = 0; i < array.Length && i<newbufArray.Length;i++)
                {
                    array[i] = newbufArray[i];
                }

                IBuffer db2 = WindowsRuntimeBufferExtensions.AsBuffer(array);
                outReport.Data = db2;

                System.Diagnostics.Debug.WriteLine("Sending:");
                DebugBytes(new byte[] { (Byte)outReport.Id,(Byte)data });

                try
                {
                    await hidDevice.SendOutputReportAsync(outReport);
                }
                catch (Exception ex)
                {
                    //System.Diagnostics.Debug.WriteLine("Exception sending Output Report:" + ex.ToString());
                }
            }
            catch(Exception ex2)
            {
                System.Diagnostics.Debug.WriteLine("Exception sending Output Report:" + ex2.ToString());
            }
            return;
        }

        /// <summary>
        /// This is a launcher command - made up of 2 send data pieces
        /// </summary>
        /// <param name="data"></param>
        public void SendCommand(ushort data)
        {
            SendData(data);
        }

        private void OnInputReportEvent(HidDevice sender, HidInputReportReceivedEventArgs eventArgs)
        {
            HidInputReport inputReport = eventArgs.Report;
            IBuffer buffer = inputReport.Data;

            //Debug the buffer out
            
            Byte[] bytes;

            bytes = System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeBufferExtensions.ToArray(buffer);
            foreach(byte b in bytes)
            {
                if (b != 0x00)
                {
                    System.Diagnostics.Debug.WriteLine("Received From Device:");
                    DebugBytes(bytes);
                    break;
                }
            }
        }

        private void DebugBytes(byte[] bytes)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (byte b in bytes)
                strBuilder.AppendFormat("{0:x2} ", b);
            System.Diagnostics.Debug.WriteLine(strBuilder.ToString());
            return;
        }

    }
}
