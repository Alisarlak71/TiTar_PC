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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dragablz;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;

namespace MaterialDesign2.Pages
{
    /// <summary>
    /// Interaction logic for DetialsPage2.xaml
    /// </summary>
    public partial class DetialsPage2 : Page
    {
        CustomButton follow = new CustomButton();
        public string content_type = null;
        public string channel_id = null;
        public string user_id = null;
        public bool followed;
        public string id
        {
            get { return channel_id; }
            set { channel_id = value; }
        }
        public string type
        {
            get { return content_type; }
            set { content_type = value; }
        }
        public DetialsPage2()
        {
            InitializeComponent();
            this.KeepAlive = false;

        }

        public class FollowClick : TextBlock
        {
            public string followid;
        }
        public class Tile
        {
            public CustomButton Name { get; set; }
        }
        private void DetialsPage2_Loaded(object sender, RoutedEventArgs e)
        {
            index_to_show = 0;
           string ctype= (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).Contetnt_Type;
            Thread t = new Thread(
                             o =>
                             {
                                 if (content_type == "mychannel")
                                 {
                                     JToken Channel_contents;
                                     (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.url = "http://titar.ir/api/pc/channels/" + (MaterialDesign.App.Current as MaterialDesign.App).User.id;
                                     (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.get_connection();

                                     Channel_contents = (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.response_content;

                                     Dispatcher.BeginInvoke(
                                   (Action)(() =>
                                   {
                                     
                                                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                               TextBlock Title = new TextBlock();
                                               Title.Foreground = Brushes.White;
                                               Title.Margin = new Thickness(5);
                                               if (Channel_contents.Count()>0)
                                               {
                                                   Title.Text = Channel_contents[0]["channel"]["name"].ToString();
                                                   Addtolist2(Channel_contents);
                                               }
                                               else
                                               {
                                                   Title.Text = "هنوز محتوایی در این کانال ثبت نشده";
                                               }
                                                Detials_Content.Children.Add(Title);
                                               
                                         

                                   }));
                                 }
                                 else if(content_type == "channel")
                                 {
                                     JToken Channel_contents;
                                     (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.url = "http://titar.ir/api/pc/channel/" + channel_id;
                                     (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.get_connection(); 
                                     Channel_contents = (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.response_content;
                                     Dispatcher.BeginInvoke(
                                   (Action)(() =>
                                   {

                                       (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                               TextBlock Title = new TextBlock();
                                               Title.Foreground = Brushes.White;
                                               Title.Margin = new Thickness(5);
                                                
                                               if (Channel_contents!=null)
                                               {
                                                   Detials_Content.Children.Clear();
                                                   Title.Text = Channel_contents["channel"]["name"].ToString();
                                                   Detials_Content.Children.Add(Title);
                                                   var color = new BrushConverter();
                                                   follow.Width = 80;
                                                   follow.Height = 30;
                                                   follow.Background = (Brush)color.ConvertFrom("#bff442");
                                                   follow.Foreground = Brushes.Black;
                                                   follow.MouseDown += follow_channel ;
                                                   follow.Content = "دنبال کردن";
                                                   follow.Padding = new Thickness(5);
                                                   follow.id = channel_id;
                                                   Detials_Content.Children.Add(follow);
                                                   Addtolist2(Channel_contents["contents"]);
                                               }
                                               else
                                               {
                                                   Title.Text = "هنوز محتوایی در این کانال ثبت نشده";
                                                   Detials_Content.Children.Add(Title);
                                               }
                                               
                                          
                                   }));
                                 }
                                 else if(content_type == "user")
                                 {
                                     MaterialDesign2.Classes.Users user = new MaterialDesign2.Classes.Users();
                                     JToken User_contents;
                                     (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.url = "http://titar.ir/api/pc/user_contents/" + user_id;
                                     (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.get_connection();
                                     
                                     User_contents = (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.response_content;
                                     int followed_int = user.check_follow((MaterialDesign.App.Current as MaterialDesign.App).User.id,User_contents["contents"][0]["id"].ToString());

                                     Dispatcher.BeginInvoke(
                                   (Action)(() =>
                                   {

                                       (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                               TextBlock Title = new TextBlock();
                                               Title.Foreground = Brushes.White;
                                               Title.Margin = new Thickness(5);

                                       if (User_contents != null)
                                       {

                                           Detials_Content.Children.Clear();
                                           Addtolist2(User_contents["contents"]);
                                           Title.Text = User_contents["contents"][0]["author"]["name"].ToString();
                                           Detials_Content.Children.Add(Title);
                                           var color = new BrushConverter();
                                           follow.Width = 80;
                                           follow.Height = 30;
                                           follow.Background = (Brush)color.ConvertFrom("#bff442");
                                           follow.Foreground = Brushes.Black;
                                           follow.MouseDown += follow_user;
                                           follow.Content = "دنبال کردن";
                                           if (followed_int == 1)
                                           {
                                               follow.Background = (Brush)color.ConvertFrom("#444444");
                                               follow.Foreground = Brushes.White;
                                               followed = true;
                                               follow.Content = "لغو دنبال کردن";
                                           }
                                           follow.Padding = new Thickness(5);
                                           follow.id = user_id;
                                           Detials_Content.Children.Add(follow);

                                       }
                                       else
                                       {
                                           Title.Text = "هنوز محتوایی توسط این کاربر ثبت نشده";
                                           Detials_Content.Children.Add(Title);
                                       }
                                               
                                        
                                   }));
                                 }
                                 else
                                 {
                                     /// recive data 
                                     
                                     /// end recive
                                     Dispatcher.BeginInvoke(
                                       (Action)(() =>
                                       {

                                           (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                                   switch (content_type)
                                                   {
                                                       case "most_seen":
                                                           {
                                                       (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.url = "http://titar.ir/api/pc/contents/2/" + ctype;
                                                       (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.get_connection();
                                                       Addtolist2((MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.response_content["contents"]);
                                                      
                                                   }
                                                   break;
                                                       case "last_video":
                                                   (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.url = "http://titar.ir/api/pc/contents/1/" + ctype;
                                                   (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.get_connection();
                                                   Addtolist2((MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.response_content["contents"]);
                                                   break;
                                                       case "most_down":
                                                   (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.url = "http://titar.ir/api/pc/contents/3/" + ctype;
                                                   (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.get_connection();
                                                   Addtolist2((MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.response_content["contents"]);
                                                   break;
                                                       case "most_love":
                                                   (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.url = "http://titar.ir/api/pc/contents/4/" + ctype;
                                                   (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.get_connection();
                                                   Addtolist2((MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.response_content["contents"]);
                                                   break;
                                                       case "most_sell":
                                                   (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.url = "http://titar.ir/api/pc/contents/5/" + ctype;
                                                   (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.get_connection();
                                                   Addtolist2((MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.response_content["contents"]);
                                                   break;
                                                   }
                                             

                                       }));
                                 }
                             });
            t.Start();

            this.ShowsNavigationUI = false;
            
        }
        int index_to_show = 0;
        public JToken content ;
        
        private void Addtolist2(JToken Content1)
        {
            content = Content1;
           // listview.Items.Clear();
            int x = 0;// content.Count;
           int t= Content1.Count();
            if (Content1.Count() - index_to_show >= 10)
            {
                x = (10 + index_to_show);
            }
            else
            {
                x = Content1.Count();
            }
            while (index_to_show < x)
            {

                BitmapImage bitmap = new BitmapImage();
                ImageBrush bimg;
                //try
                //{
                //    WebRequest req = WebRequest.Create("http://titar.ir/contents/thumbnail/" + Content1[index].thumbnail);
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
                bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/contents/thumbnail/" + Content1[index_to_show]["thumbnail"])));
                //} 

                CustomButton card1 = new CustomButton();
                card1.id = Content1[index_to_show]["id"].ToString();
                card1.type = Content1[index_to_show]["type"].ToString();
                card1.Width = 300;
                card1.Height = 300;
                card1.MouseLeftButtonUp += move_to_detail;
                card1.MouseLeftButtonDown += start_timer;
                card1.Cursor = Cursors.Hand;
                card1.FlowDirection = FlowDirection.RightToLeft;
                card1.Background = bimg;
                /// transparent background and wrapper tag

                Grid temp = new Grid();
                temp.VerticalAlignment = VerticalAlignment.Bottom;
                temp.FlowDirection = FlowDirection.RightToLeft;
                temp.Background = (Brush)FindResource("MytransparentBackground");
                temp.Height = 50;


                // temp.Name = "item" + index.ToString();
                //temp.MouseEnter +=ShowDetialAnimationOnEnter;
                // temp.MouseLeave += ShowDetialAnimationOnLeave;
                /// title
                TextBlock Title = new TextBlock();
                Title.FontSize = 15;
                Title.Text = Content1[index_to_show]["title"].ToString();
                Title.Style = (Style)FindResource("MaterialDesignTitleTextBlock");
                Title.Foreground = Brushes.White;
                Title.TextAlignment = TextAlignment.Center;
                temp.Children.Add(Title);

                TextBlock Author = new TextBlock();
                Author.FontSize = 10;
                //Author.Text = "ناشر : " + Content1[index_to_show]["author"]["name"].ToString();
                Author.Foreground = Brushes.White;
                Author.HorizontalAlignment = HorizontalAlignment.Right;
                Author.VerticalAlignment = VerticalAlignment.Bottom;
                Author.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Author);

                TextBlock Seen_Count = new TextBlock();
                Seen_Count.FontSize = 10;
                Seen_Count.Text = "بازدید : " + Content1[index_to_show]["seen_count"];
                Seen_Count.Foreground = Brushes.White;
                Seen_Count.HorizontalAlignment = HorizontalAlignment.Left;
                Seen_Count.VerticalAlignment = VerticalAlignment.Bottom;
                Seen_Count.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Seen_Count);

                card1.Content = temp;
                Dispatcher.Invoke(new Action(() => {
                    listview.Items.Add(new Tile()
                    {
                        Name = card1,
                    });
                }), DispatcherPriority.ContextIdle, null);
              
                index_to_show++;
            }

            if(index_to_show < Content1.Count())
            {
                CustomButton reload_more= new CustomButton();
                reload_more.Width = 300;
                reload_more.Height = 300;
                reload_more.MouseLeftButtonDown += load_more_click;
                reload_more.Cursor = Cursors.Hand;
                reload_more.FlowDirection = FlowDirection.RightToLeft;
                reload_more.Background = Brushes.Black;
                StackPanel stack = new StackPanel();
                stack.Width = 150;
                stack.Height = 150;
                stack.VerticalAlignment = VerticalAlignment.Center;
                stack.HorizontalAlignment = HorizontalAlignment.Center;
                stack.Background = Brushes.Transparent;
                stack.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/add.png")));
                reload_more.Content = stack;

                listview.Items.Add(new Tile()
                {
                    Name = reload_more,
                });
            }
            
            
        }

        private void load_more_click(object sender, MouseEventArgs e)
        {
            
            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
            listview.Items.RemoveAt(index_to_show);
            
            listview.Items.Refresh();
            Addtolist2(content);
            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
        }
        public int time;

        private void move_to_detail(object sender, MouseEventArgs e)
        {
            int i = e.Timestamp - time;
            string elementName = "";
            if (i < 150)
            {

                var mouseWasDownOn = sender as CustomButton;
                if (mouseWasDownOn != null)
                {
                    elementName = mouseWasDownOn.id;
                }
                (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content.number_of_try = 0;

                if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                {
                    if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                    {
                        
                            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                             (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).scrollbar1.ScrollToTop();

                             if (mouseWasDownOn.type == "app" || mouseWasDownOn.type == "game")
                             {
                                 AppGamePage details = new AppGamePage();
                                 details.selected_id = elementName;
                                 this.NavigationService.Navigate(details);
                             }
                             else
                             {
                                 MaterialDesign1.Pages.DetialsPage1 detialspage1 = new MaterialDesign1.Pages.DetialsPage1();
                                 detialspage1.selected_id = elementName;
                                 this.NavigationService.Navigate(detialspage1);
                             }
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
                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                    MaterialDesign2.Pages.NoInternet Nointernet = new MaterialDesign2.Pages.NoInternet();
                    this.NavigationService.Navigate(Nointernet);
                }



            }
        }
        private void follow_channel(object sender, MouseEventArgs e)
        {
            if (followed==false)
            {
                string elementName = "";
                var mouseWasDownOn = sender as CustomButton;
                if (mouseWasDownOn != null)
                {
                    elementName = mouseWasDownOn.id;
                }
                
                Thread t = new Thread(
                            o =>
                            {
                                Classes.Channels channel= new Classes.Channels();
                                int i=channel.follow_channel(channel_id,(MaterialDesign.App.Current as MaterialDesign.App).User.id);
                                Dispatcher.BeginInvoke(
                                  (Action)(() =>
                                  {
                                      if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable())
                                      {
                                         
                                              
                                                      if (i==0)
                                                      {
                                                          var color = new BrushConverter();
                                                          follow.Background = (Brush)color.ConvertFrom("#444444");
                                                          follow.Foreground = Brushes.White;
                                                          followed = true;
                                                          follow.Content = "لغو دنبال کردن";
                                                      }
                                                  
                                              
                                          }
                                     
                                      
                                  }));

                            });
                t.Start();
            }
            else
            {

                string elementName = "";
                var mouseWasDownOn = sender as CustomButton;
                if (mouseWasDownOn != null)
                {
                    elementName = mouseWasDownOn.id;
                }

                Thread t = new Thread(
                            o =>
                            {
                                Classes.Channels channel = new Classes.Channels();
                                int i = channel.unfollow_channel(channel_id, (MaterialDesign.App.Current as MaterialDesign.App).User.id);
                                Dispatcher.BeginInvoke(
                                  (Action)(() =>
                                  {
                                      if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable())
                                      {

                                          
                                                  if (i == 0)
                                                  {
                                                      var color = new BrushConverter();
                                                      follow.Background = (Brush)color.ConvertFrom("#bff442");
                                                      follow.Foreground = Brushes.Black;
                                                      followed = false;
                                                      follow.Content = "دنبال کردن";
                                                  }
                                              
                                      }


                                  }));

                            });
                t.Start();
            }
        }
        private void follow_user(object sender, MouseEventArgs e)
        {
            if (followed == false)
            {
                string elementName = "";
                var mouseWasDownOn = sender as CustomButton;
                if (mouseWasDownOn != null)
                {
                    elementName = mouseWasDownOn.id;
                }
                Thread t = new Thread(
                            o =>
                            {
                                Classes.Users user = new Classes.Users();
                                int i = user.follow_user(user_id, (MaterialDesign.App.Current as MaterialDesign.App).User.id);
                                Dispatcher.BeginInvoke(
                                  (Action)(() =>
                                  {
                                      if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable())
                                      {

                                          
                                                  if (i == 0)
                                                  {
                                                      var color = new BrushConverter();
                                                      follow.Background = (Brush)color.ConvertFrom("#444444");
                                                      follow.Foreground = Brushes.White;
                                                      followed = true;
                                                      follow.Content = "لغو دنبال کردن";
                                                  }
                                            
                                      }


                                  }));

                            });
                t.Start();
            }
            else
            {

                string elementName = "";
                var mouseWasDownOn = sender as CustomButton;
                if (mouseWasDownOn != null)
                {
                    elementName = mouseWasDownOn.id;
                }

                Thread t = new Thread(
                            o =>
                            {
                                Classes.Users user = new Classes.Users();
                                int i = user.unfollow_user(user_id, (MaterialDesign.App.Current as MaterialDesign.App).User.id);
                                Dispatcher.BeginInvoke(
                                  (Action)(() =>
                                  {
                                      if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable())
                                      {

                                        
                                                  if (i == 0)
                                                  {
                                                      var color = new BrushConverter();
                                                      follow.Background = (Brush)color.ConvertFrom("#bff442");
                                                      follow.Foreground = Brushes.Black;
                                                      followed = false;
                                                      follow.Content = "دنبال کردن";
                                                  }
                                            
                                      }


                                  }));

                            });
                t.Start();
            }
        }

        private void start_timer(object sender, MouseEventArgs e)
        {
            time = e.Timestamp;
        }
        //// add a custom property
        public class CustomButton : Card
        {
            public string id;
            public string type { set; get; }
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();


        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            IntPtr pHandle = GetCurrentProcess();
            SetProcessWorkingSetSize(pHandle, -1, -1);
        }
    }
}
