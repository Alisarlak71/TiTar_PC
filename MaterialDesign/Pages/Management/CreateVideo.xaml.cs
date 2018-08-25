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
using System.ComponentModel;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Media.Animation;
using NReco.VideoInfo;
using System.IO;
using BespokeFusion;
namespace MaterialDesign2.Pages.Management
{
    /// <summary>
    /// Interaction logic for CreateVideo.xaml
    /// </summary>
    public partial class CreateVideo : Page
    {
        public static string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
        public static Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
        public static string url = variables["FtpUrl"];
        public static string user = variables["FtpUser"];
        public static string pass = variables["FtpPass"];

        public MaterialDesign2.Classes.FtpWorks UploadClass = new MaterialDesign2.Classes.FtpWorks(url , user , pass);
        List<MaterialDesign2.Classes.Channel> content_channel = new List<Classes.Channel>();
        public string FileName;
        public string ServerFileName;
        public string Format;
        public string Duration;
        public string Size;
        public CreateVideo()
        {
            InitializeComponent();
            FileName = "";
            ServerFileName = "";
            Format = "";
            Duration = "";
            Size = "";
            content_channel = null;
        }
        private void CreateVideoLoad(object sender, RoutedEventArgs e)
        {
            wait_lable.Content = "";
            Title.Text = "";
            File_Address.Content = "";
            Channel_Name.Text = "";
            Description.Text = "";
            Age.Text = "";
            Price.Text = "";
            Tags.Text = "";
            Language.Text = "";

            Upload_Content.Visibility = Visibility.Hidden;
            Information_Content.Visibility = Visibility.Visible;
            UploadProgressBar.Visibility = Visibility.Hidden;
            Final_Register.Visibility = Visibility.Hidden;
            UploadFile.Visibility = Visibility.Visible;
            wait_lable.Foreground = Brushes.White;
            var bc = new BrushConverter();
            Upload_label.Foreground = Brushes.White;
            Information_label.Foreground = (Brush)bc.ConvertFrom("#bff442");
            this.Final_Register.Visibility = Visibility.Hidden;
             ////get contnent
            
            Thread t = new Thread(
                             o =>
                             {
                                 /// recive data 
                                 MaterialDesign2.Classes.Channels channels = new MaterialDesign2.Classes.Channels();
                                 content_channel = channels.get_my_channel((MaterialDesign.App.Current as MaterialDesign.App).User.id);
                                 /// end recive
                                 Dispatcher.BeginInvoke(
                                   (Action)(() =>
                                   {
                                       
                                               Channel_Name.Items.Clear();
                                               int i=0;
                                               while(i<content_channel.Count){
                                                   Channel_Name.Items.Clear();
                                                   Channel_Name.Items.Add(content_channel[i].name);
                                                   i++;
                                               }
                                           

                                   }));
                             });
            t.Start();
            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();

            this.ShowsNavigationUI = false;
        }
        private void Register_Countinue(object sender, RoutedEventArgs e)
        {


            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
             
            string channelname = Channel_Name.SelectedItem.ToString();

            MaterialDesign2.Classes.Channel channel = new Classes.Channel();
            int index = 0;
            while(index<content_channel.Count)
            {
                if (content_channel[index].name==channelname)
                {
                    channel=content_channel[index];
                }
                index++;
            }
            
            if (Title.Text != "" && Description.Text != "" && Age.Text != "" && Language.Text != "" && Tags.Text != "" && Price.Text != "" && Format!=""&& Size!=""&&Duration!="")
            {
                string title = Title.Text;
                string descrpition = Description.Text;
                string age = Age.Text;
                string price = Price.Text;
                string lan = Language.Text;
                string tags=Tags.Text;

                Thread t = new Thread(
                                 o =>
                                 {
                                     /// send content data 
                                     MaterialDesign2.Classes.SendContent send = new Classes.SendContent();
                                     ServerFileName = send.send_content_video((MaterialDesign.App.Current as MaterialDesign.App).User.id, channel.id, title, descrpition, age,lan, tags,price, Format, Size, Duration);
                                     /// end send content data
                                     Dispatcher.BeginInvoke(
                                       (Action)(() =>
                                       {

                                           (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                                   if (ServerFileName != "")
                                                   {
                                                       Upload_Content.Visibility = Visibility.Visible;
                                                       Information_Content.Visibility = Visibility.Hidden;
                                                       Final_Register.Visibility = Visibility.Hidden;

                                                       var bc = new BrushConverter();
                                                       Information_label.Foreground = Brushes.White;
                                                       Upload_label.Foreground = (Brush)bc.ConvertFrom("#bff442");
                                                       this.Final_Register.Visibility = Visibility.Hidden;

                                                       UploadProgressBar.Visibility = Visibility.Visible;
                                                       wait_lable.Content = "لطفا شکیبا باشید";
                                                       BackgroundWorker worker = new BackgroundWorker();
                                                       worker.WorkerReportsProgress = true;
                                                       worker.DoWork += upload;
                                                       worker.ProgressChanged += worker_ProgressChanged;
                                                       worker.RunWorkerAsync();
                                                   }
                                                   else
                                                   {
                                                       /// warnning
                                                       MaterialMessageBox.Show("مشکلی در ثبت اطلاعات پیش امده است!", "اخطار");
                                                   }
                                              

                                       }));


                                 });
                t.Start();
            }
            else
            {

                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                 
                /// warnning
                MaterialMessageBox.Show("تمام فیلد ها را پر کنید!", "اخطار");
            }      

        }

        private void Upload_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName!="")
            {
                var ffProbe = new FFProbe();
                try
                {
                    var videoInfo = ffProbe.GetMediaInfo(openFileDialog.FileName);
                    Duration = (videoInfo.Duration.TotalSeconds).ToString();//videoInfo.Duration.Hours.ToString() + "." + videoInfo.Duration.Minutes.ToString() + "." + videoInfo.Duration.Seconds.ToString();
                    Format = System.IO.Path.GetExtension(openFileDialog.FileName).Trim('.');

                    FileName = openFileDialog.FileName;
                    FileStream fs = File.OpenRead(openFileDialog.FileName);
                    Size = fs.Length.ToString();
                    File_Address.Content = openFileDialog.FileName;
                }
                catch
                {
                    Duration = "";
                    MaterialMessageBox.Show("فایل ورودی نامعتبر است","اخطار");
                }
                
            }
        }

        private void Final_Register_Click(object sender, RoutedEventArgs e)
        {

            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).scrollbar1.ScrollToTop();

                    if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                    {
                        if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                        {
                            (MaterialDesign.App.Current as MaterialDesign.App).CreateVideoPage = new CreateVideo();
                            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate((MaterialDesign.App.Current as MaterialDesign.App).CreateVideoPage);
                        }
                        else
                        {
                            MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate(login_page);
                        }
                    }
                    else
                    {
                        MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                        (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate(No_Internet);
                    }
              
        }
        public void upload(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker = (sender as BackgroundWorker);
            UploadClass.upload("contents/video/"+ServerFileName+"."+Format, FileName, worker);
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UploadProgressBar.Value = e.ProgressPercentage;
        }

        
    }
}
