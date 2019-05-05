using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using MaterialDesignThemes.Wpf;
using MahApps.Metro;
using MahApps.Metro.Controls;
using BespokeFusion;
namespace MaterialDesign2.Pages.Controls
{
    /// <summary>
    /// Interaction logic for SelectDevice.xaml
    /// </summary>
    public partial class SelectDevice : MahApps.Metro.Controls.MetroWindow
    {
        public SelectDevice()
        {
            InitializeComponent();
        }
        public Newtonsoft.Json.Linq.JToken SelectedItem;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MaterialDesign2.Pages.Controls.ImagePlayer player = new MaterialDesign2.Pages.Controls.ImagePlayer();
           
                if (htc.IsChecked == true)
                {
                    String counter = "http://titar.ir/contents/video/" + SelectedItem["content"]["file_name"];
                    
                    System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
                    pProcess.StartInfo.FileName = @"Player\ViveTitarPlayer.exe";
                    pProcess.Start();
                    SendUDP("127.0.0.1", 41181, counter.ToString(), counter.ToString().Length);
                }
                else if (oculus.IsChecked == true)
                {
                //MaterialMessageBox.Show("این امکان با مساعدت مهندس جعفری در آینده ای نزدیک افزوده خواهد شد", "هشدار");
                //CustomMaterialMessageBox m = new CustomMaterialMessageBox
                //{
                //    TxtMessage = { Text = "این امکان با مساعدت مهندس جعفری در آینده ای نزدیک افزوده خواهد شد", Foreground = Brushes.White },
                //    TxtTitle = { Text = "پیام", Foreground = Brushes.White },
                //    BtnOk = { Content = "تایید", HorizontalAlignment = HorizontalAlignment.Center },
                //    MainContentControl = { Background = Brushes.Black },
                //    TitleBackgroundPanel = { Background = Brushes.Black },

                //    BorderBrush = Brushes.BlueViolet,
                //};
                //m.Show();   
                String counter = "http://titar.ir/contents/video/" + SelectedItem["content"]["file_name"];

                System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
                pProcess.StartInfo.FileName = @"Player\ViveTitarPlayer.exe";
                pProcess.Start();
                SendUDP("127.0.0.1", 41181, counter.ToString(), counter.ToString().Length);
            }
                else
                {
                    player.typeContent = "video";
                    player.url = "http://titar.ir/contents/video/" + SelectedItem["content"]["file_name"];
                    player.Show();
                }
                this.Close();
            
            
        }
        
        public void SendUDP(string hostNameOrAddress, int destinationPort, string data, int count)
        {
            IPAddress destip = Dns.GetHostAddresses(hostNameOrAddress)[0];
            TcpListener listener = new TcpListener(destip, destinationPort);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            byte[] message = Encoding.Unicode.GetBytes(data);
            stream.Write(message, 0, message.Length);
            client.Close();
            listener.Stop();

        }

    }
}
