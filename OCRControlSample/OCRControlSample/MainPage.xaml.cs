using Bing.Ocr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OCRControlSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Bennyskywalker_OCRSample
        //OCRControlSample
        //pJneQvE9IHNC/o7/YSzzj0lsdKGbctCZfmGFpoD88zE=
        private string clientID = "OCRControlSample";
        private string clientSecret = "pJneQvE9IHNC/o7/YSzzj0lsdKGbctCZfmGFpoD88zE=";
        

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            OCR.Completed += OCR_Completed;
            OCR.Failed += ocrControl_ErrorOccurred;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            OCR.ClientId = "Bennyskywalker_OCRSample";
            OCR.ClientSecret = "pJneQvE9IHNC/o7/YSzzj0lsdKGbctCZfmGFpoD88zE=";
            await OCR.StartPreviewAsync();
        }

        private async void OCR_Completed(object sender, OcrCompletedEventArgs e)
        {
            // Make sure there is text.
            if (e.Result.Lines.Count == 0)
            {
                tbResults.Text = "No text found.";
                await OCR.StartPreviewAsync(null);
                return;
            }

            // Read the text and print it to a TextBlock.
            var sb = new System.Text.StringBuilder();
            foreach (Line l in e.Result.Lines)
            {
                foreach (Word w in l.Words)
                {
                    sb.AppendFormat("{0} ", w.Value);
                }
                sb.AppendLine();
            }
            tbResults.Text = sb.ToString();

            await OCR.StartPreviewAsync(null);
        }


        private async void ocrControl_ErrorOccurred(object sender, Bing.Ocr.OcrErrorEventArgs e)
        {
            tbResults.Text = string.Format("Error code = {0}, Message = {1}", e.ErrorCode, e.ErrorMessage ?? "<null>");
            switch (e.ErrorCode)
            {
                case Bing.Ocr.ErrorCode.Success:
                case Bing.Ocr.ErrorCode.InputInvalid:
                case Bing.Ocr.ErrorCode.InternalError:
                case Bing.Ocr.ErrorCode.Unauthorized:
                case Bing.Ocr.ErrorCode.NetworkUnavailable:
                    await OCR.StartPreviewAsync();
                    break;
                default:
                    await OCR.ResetAsync();
                    break;
            }
        }


    }
}
