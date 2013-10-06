using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpheroPOC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private RfcommDeviceService rfcommService;
        private StreamSocket socket;
        private DataWriter writer;
        private DataReader reader;
        private DeviceInformation devInfo;
        private SpheroCommands sc = new SpheroCommands();
        private Boolean bConnected;
        private Task readTask = null;

        private byte r = 0xff, g = 0, b = 0;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;
            App.Current.Suspending += Current_Suspending;
        }

        void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            Disconnect();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void EnumerateDevices()
        {
            try
            {
                var aqs = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort);
                var serviceInfoCollection = await DeviceInformation.FindAllAsync(aqs);

                foreach(var serviceInfo in serviceInfoCollection)
                {
                    devInfo = serviceInfo;
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error enum Devices ex=" + ex.ToString());
            }
        }

        private async void StartDevice()
        {
            try
            {
                rfcommService = await RfcommDeviceService.FromIdAsync(devInfo.Id);

                if (rfcommService != null)
                {
                    socket = new StreamSocket();
                    await socket.ConnectAsync(rfcommService.ConnectionHostName,
                        rfcommService.ConnectionServiceName,
                        SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

                    writer = new DataWriter(socket.OutputStream);

                    bConnected = true;

                    System.Diagnostics.Debug.WriteLine("Device Connected");

                    //Start receieve thread
                    WaitForData();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("rfcommService==null");
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Start Device exception ex=" + ex.ToString());
            }
        }

        private async void WaitForData()
        {
            if (bConnected == false)
                return;

            var dataReader = new DataReader(socket.InputStream);
            dataReader.InputStreamOptions = InputStreamOptions.Partial;

            try
            {
                var payload = await dataReader.LoadAsync(256);
                if (payload != 0)
                {
                    var bytes = new byte[payload];
                    dataReader.ReadBytes(bytes);

                    System.Diagnostics.Debug.WriteLine("Received Length=" + bytes.Length);

                    //Piece together a message
                    DebugBytes(bytes);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception reading stream");
            }

            dataReader.DetachStream();
            WaitForData();
        }

        private void DebugBytes(byte[] bytes)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (byte b in bytes)
                strBuilder.AppendFormat("{0:x2} ", b);
            System.Diagnostics.Debug.WriteLine(strBuilder.ToString());
            return;
        }

        private void Disconnect()
        {
            bConnected = false;

            if(writer!=null)
            {
                writer.DetachStream();
                writer = null;
            }

            if(reader!=null)
            {
                reader.DetachStream();
                reader = null;
            }

            if(socket!=null)
            {
                socket.Dispose();
                socket = null;
            }

            if(rfcommService!=null)
            {
                rfcommService = null;
            }
        }

        private void Button_Connect(object sender, RoutedEventArgs e)
        {
            EnumerateDevices();
            StartDevice();
        }

        private async void Button_Color(object sender, RoutedEventArgs e)
        {
            byte[] packet = sc.CreateColorChangePacket(r, g, b);

            System.Diagnostics.Debug.WriteLine("Output:");
            DebugBytes(packet);
            writer.WriteBytes(packet);
            await writer.StoreAsync();

            if(r==0xff)
            {
                r = 0;
                g = 0xff;
                b = 0;
            }
            else if(g==0xff)
            {
                r = 0;
                g = 0;
                b = 0xff;
            }
            else if(b==0xff)
            {
                r = 0xff;
                g = 0;
                b = 0;
            }
        }

        private void Button_Disconnect(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }
    }
}
