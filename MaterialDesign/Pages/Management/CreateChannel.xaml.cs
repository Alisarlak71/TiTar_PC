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
using BespokeFusion;
using Microsoft.Win32;
using System.Threading;
namespace MaterialDesign2.Pages.Management
{
    /// <summary>
    /// Interaction logic for CreateChannel.xaml
    /// </summary>
    public partial class CreateChannel : Page
    {
        public CreateChannel()
        {
            InitializeComponent();
            imgpath = "";
            ChannelImagePath.Content = "";
        }
        public string imgpath;
        private void CreateChannel_Click(object sender, RoutedEventArgs e)
        {
            if (ChannelName.Text != "" && imgpath != "")
            {

                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                   
                string channelname = ChannelName.Text;
                Thread t = new Thread(
                                 o =>
                                 {
                                     Classes.Channels channel = new Classes.Channels();
                                     int handle = channel.create_channel((MaterialDesign.App.Current as MaterialDesign.App).User.id, channelname, imgpath);
                                     Dispatcher.BeginInvoke(
                                                            (Action)(() =>
                                                            {

                                                                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                                                        if (handle == 0)
                                                                        {
                                                                            /// all things true
                                                                            MaterialMessageBox.Show("کانال با نام '" + channelname + "' ایجاد شد", "تایید");
                                                                            ChannelName.Text = "";
                                                                        }
                                                                        else if (handle == -1)
                                                                        {
                                                                            //// maby channel name is repetitive
                                                                            Warninnglable.Content = "نام کانال تکراری است";
                                                                        }
                                                                        else
                                                                        {
                                                                            //// error when conncetion 
                                                                            Warninnglable.Content = "در اتصال به اینترنت مشکلی به وجود امده است";
                                                                        }
                                                                    
                                                            }));
                                 });
                t.Start();
            }
            else
            {
                Warninnglable.Content = "نام کانال را وارد کنید";
            }
            
        }
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
           OpenFileDialog openFileDialog = new OpenFileDialog();
           openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
           if (openFileDialog.ShowDialog() == true && openFileDialog.FileName != "")
           {
               imgpath = openFileDialog.FileName;
               ChannelImagePath.Content = openFileDialog.FileName;
           }
        }
    }

}
