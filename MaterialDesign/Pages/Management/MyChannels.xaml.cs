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
using System.Net;
using System.IO;
using System.Threading;
namespace MaterialDesign2.Pages.Management
{
    /// <summary>
    /// Interaction logic for MyChannels.xaml
    /// </summary>
    public partial class MyChannels : Page
    {
        public MyChannels()
        {
            InitializeComponent();
        }
        private void MyChannel_Loaded(object sender, RoutedEventArgs e)
        {
           
            Thread t = new Thread(
                             o =>
                             {
                                 /// recive data 
                                 MaterialDesign2.Classes.Channels channels = new MaterialDesign2.Classes.Channels();
                                 List<MaterialDesign2.Classes.Channel> content_channel = new List<Classes.Channel>();
                                 content_channel = channels.get_my_channel((MaterialDesign.App.Current as MaterialDesign.App).User.id);
                                 /// end recive
                                 Dispatcher.BeginInvoke(
                                   (Action)(() =>
                                   {

                                       (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                               InitializeTable(content_channel);
                                         

                                   }));
                             });
            t.Start();
            this.ShowsNavigationUI = false;
           ////
        }
        private void InitializeTable(List<MaterialDesign2.Classes.Channel> channel)
        {
            int index=0;
            channel_holder.HorizontalAlignment = HorizontalAlignment.Center;
            channel_holder.VerticalAlignment = VerticalAlignment.Top;

            while (index < channel.Count)
            {
                StackPanel items = new StackPanel();
                items.Orientation = Orientation.Horizontal;
                Image img = new Image();
                img.HorizontalAlignment = HorizontalAlignment.Center;
                img.Margin = new Thickness(10, 10, 10, 10);
                BitmapImage bitmap = new BitmapImage();
                ImageBrush bimg;
                //try
                //{
                //    WebRequest req = WebRequest.Create("http://titar.ir/thumbnails/channels/" + channel[index].thumbnail);
                //    WebResponse response = req.GetResponse();
                //    Stream stream = response.GetResponseStream();
                //    bitmap.BeginInit();
                //    bitmap.StreamSource = stream;
                //    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                //    bitmap.EndInit();
                //    bimg = new ImageBrush(bitmap);
                //}
                //catch (Exception)
                //{
                    bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/thumbnail/" + channel[index].thumbnail)));
                //}
                img.Source = bimg.ImageSource;
                img.Width = 120;
                img.Height = 120;
                items.Children.Add(img);
                Grid SP = new Grid();
                clickTitle Title_Text = new clickTitle();
                Title_Text.FontSize = 15;
                Title_Text.SelectId=channel[index].id;
                Title_Text.MouseLeftButtonUp += move_to_detail;
                Title_Text.MouseLeftButtonDown += start_timer;
                Title_Text.Margin = new Thickness(10, 10, 10, 10);
                Title_Text.Width = 300;
                Title_Text.Text = channel[index].name;
                Title_Text.VerticalAlignment = VerticalAlignment.Center;
                Title_Text.HorizontalAlignment= HorizontalAlignment.Center;
                Title_Text.Foreground = Brushes.White;
                Title_Text.TextAlignment = TextAlignment.Center;
                SP.Children.Add(Title_Text);

                Button Delete = new Button();
                Delete.FontSize = 10;
                Delete.Content = "حذف";
                Delete.BorderThickness = new Thickness(0);
                Delete.Background = Brushes.Transparent;
                Delete.Foreground = Brushes.Red;
                Delete.HorizontalAlignment = HorizontalAlignment.Right;
                Delete.VerticalAlignment = VerticalAlignment.Bottom;
                Delete.Margin = new Thickness(5, 5, 5, 5);
                SP.Children.Add(Delete);

                Button Edit = new Button();
                Edit.FontSize = 10;
                Edit.Content = "ویرایش";
                Edit.BorderThickness =new Thickness(0);
                var color = new BrushConverter();
                Edit.Background = Brushes.Transparent;
                Edit.Foreground = (Brush)color.ConvertFrom("#bff442");
                Edit.HorizontalAlignment = HorizontalAlignment.Left;
                Edit.VerticalAlignment = VerticalAlignment.Bottom;
                Edit.Margin = new Thickness(5, 5, 5, 5);
                SP.Children.Add(Edit);
                items.Children.Add(SP);

                TextBlock Date_Text = new TextBlock();
                Date_Text.FontSize = 15;
                Date_Text.Width = 120;
                Date_Text.VerticalAlignment = VerticalAlignment.Center;
                Date_Text.HorizontalAlignment = HorizontalAlignment.Center;
                Date_Text.Margin = new Thickness(10, 10, 10, 10);
                Date_Text.Text = channel[index].create_date;
                Date_Text.Foreground = Brushes.White;
                Date_Text.TextAlignment = TextAlignment.Center;
                items.Children.Add(Date_Text);

                Separator seprator = new Separator();
                channel_holder.Children.Add(seprator);

                channel_holder.Children.Add(items);
                index++;
            }
            
        }

        public int time;
        private void move_to_detail(object sender, MouseEventArgs e)
        {
            int i = e.Timestamp - time;
            if (i < 150)
            {
                MaterialDesign2.Pages.DetialsPage2 details = new MaterialDesign2.Pages.DetialsPage2();
                var mouseWasDownOn = sender as clickTitle;
                if (mouseWasDownOn != null)
                {
                    details.id=mouseWasDownOn.SelectId;
                }
                details.content_type = "mychannel";
                if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable())
                {
                    if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                    {
                       
                              (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).scrollbar1.ScrollToTop();
                          
                        this.NavigationService.Navigate(details);
                    }
                    else
                    {
                        (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                        MaterialDesign2.Pages.Login LoginPage = new MaterialDesign2.Pages.Login();
                        this.NavigationService.Navigate(LoginPage);
                    }

                }
                else
                {
                    //(MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                    MaterialDesign2.Pages.NoInternet Nointernet = new MaterialDesign2.Pages.NoInternet();
                    this.NavigationService.Navigate(Nointernet);
                }
            }
        }
        private void start_timer(object sender, MouseEventArgs e)
        {
            time = e.Timestamp;
        }
        public class clickTitle : TextBlock
        {
            public string id;
            public string SelectId
            {
                get { return id; }
                set { id = value; }
            }
        }
    }
}
