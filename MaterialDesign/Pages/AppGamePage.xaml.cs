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
using MaterialDesignThemes.Wpf;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using BespokeFusion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Animation;
namespace MaterialDesign2.Pages
{
    /// <summary>
    /// Interaction logic for DetialsPage1.xaml
    /// </summary>
    public partial class AppGamePage : Page
    {
        public MaterialDesign2.Classes.Content SelectedItem = new MaterialDesign2.Classes.Content();
        public List<MaterialDesign2.Classes.CommentClass> SelectedComment= new List<MaterialDesign2.Classes.CommentClass>();
        public MaterialDesign2.Classes.Channel Channel_Of_Content=new MaterialDesign2.Classes.Channel();
        public MaterialDesign2.Classes.Content theContent
        {
            get { return SelectedItem; }
            set { SelectedItem = value; }
        }
        public string selected_id;
        public bool liked=false;
        public List<MaterialDesign2.Classes.CommentClass> IComment
        {
            get { return SelectedComment; }
            set { SelectedComment = value; }
        }
        public AppGamePage()
        {
            InitializeComponent();
            this.ShowsNavigationUI = false;
            this.KeepAlive = false;
        }

        private void reload()
        {
            Thread t = new Thread(
                        o =>
                        {
                            MaterialDesign2.Classes.Caching Cachelayer = new MaterialDesign2.Classes.Caching();
                            MaterialDesign2.Classes.Content single_content = MaterialDesign2.Classes.Caching.Get_single_from_cache<MaterialDesign2.Classes.Content>("singlecontent" + selected_id);
                            if (single_content == null)
                            {
                                single_content = (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content.get_single_content(selected_id);
                            }
                            Dispatcher.BeginInvoke(
                              (Action)(() =>
                              {
                                  if (single_content.id != null)
                                  {

                                      if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                                      {

                                          MaterialDesign2.Classes.Channels channel = new MaterialDesign2.Classes.Channels();
                                          this.SelectedItem = single_content;
                                          this.theContent = single_content;
                                          this.IComment = single_content.comments;
                                          Channel_Of_Content = single_content.channel_of_single;
                                          Cachelayer.Store_single_to_cache(single_content, single_content.id);

                                          if (single_content.comments != null)
                                              Aaddcomment();
                                          lable_click Author = new lable_click();
                                          lable_click Channel = new lable_click();
                                          Author.Foreground = Brushes.White;
                                          Author.MouseDown += go_to_author;
                                          Author.Cursor = Cursors.Hand;
                                          Author.HorizontalAlignment = HorizontalAlignment.Left;
                                          Author.SelectId = SelectedItem.author_id;
                                          Label seperator = new Label();
                                          seperator.Foreground = Brushes.White;
                                          seperator.HorizontalAlignment = HorizontalAlignment.Right;
                                          seperator.Content = "|";
                                          Channel.Foreground = Brushes.White;
                                          Channel.MouseDown += go_to_channel;
                                          Channel.Cursor = Cursors.Hand;
                                          Channel.SelectId = SelectedItem.channel_id;
                                          Channel.HorizontalAlignment = HorizontalAlignment.Left;

                                          Author.Content = SelectedItem.author["name"];

                                          Channel.Content = Channel_Of_Content.name;

                                          Title_Video.Content = SelectedItem.title;
                                          Detials_Content.Children.Clear();
                                          Detials_Content.Children.Add(Author);

                                          Description_txt.Text = SelectedItem.description;
                                          BitmapImage bitmap = new BitmapImage();
                                          ImageBrush bimg;
                                          //try
                                          //{
                                          //    WebRequest req = WebRequest.Create("http://titar.ir/contents/thumbnail/" + SelectedItem.thumbnail);
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
                                          bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/contents/thumbnail/" + SelectedItem.thumbnail)));
                                          //}
                                          Thumbnail.Source = bimg.ImageSource;
                                          //backround.Background = bimg;
                                          //(MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).background.Background = bimg;
                                          (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                          liked = single_content.rated;
                                          if (liked == true)
                                          {
                                              Like.Foreground = Brushes.Red;
                                          }
                                        

                                          Addtolist1(single_content.related);
                                      }
                                      else
                                      {
                                          MaterialDesign2.Pages.Login LoginPage = new MaterialDesign2.Pages.Login();
                                          this.NavigationService.Navigate(LoginPage);
                                      }

                                  }
                                  else
                                  {                                     
                                      MaterialDesign2.Pages.NoInternet Nointernet = new MaterialDesign2.Pages.NoInternet();
                                      this.NavigationService.Navigate(Nointernet);
                                  }
                              }));
                        });
            t.Start();

        }
        private void AppGamePage_Loaded(object sender, RoutedEventArgs e)
        {
            reload();
        }

        private void Description_Click(object sender, MouseButtonEventArgs e)
        {
            Description_Content.Visibility = Visibility.Visible;
            Comment_Content.Visibility = Visibility.Hidden;
            Related_Content.Visibility = Visibility.Hidden;

            var bc = new BrushConverter();
            Description.Foreground = (Brush)bc.ConvertFrom("#bff442");
            Comment_Block.Foreground = Brushes.White;
            Related.Foreground = Brushes.White;
        }

        private void Comment_Click(object sender, MouseButtonEventArgs e)
        {
            Description_Content.Visibility = Visibility.Hidden;
            Comment_Content.Visibility = Visibility.Visible;
            Related_Content.Visibility = Visibility.Hidden;

            var bc = new BrushConverter();
            Description.Foreground = Brushes.White;
            Comment_Block.Foreground = (Brush)bc.ConvertFrom("#bff442");
            Related.Foreground = Brushes.White;
        }

        private void Related_Click(object sender, MouseButtonEventArgs e)
        {
            Description_Content.Visibility = Visibility.Hidden;
            Comment_Content.Visibility = Visibility.Hidden;
            Related_Content.Visibility = Visibility.Visible;

            var bc = new BrushConverter();
            Description.Foreground = Brushes.White;
            Comment_Block.Foreground = Brushes.White;
            Related.Foreground = (Brush)bc.ConvertFrom("#bff442");
        }

        private void Addtolist1(JToken related_content)
        {
            int index = 0;
            List1.Children.Clear(); 
            if(related_content!=null)
            { 
            while (index<related_content.Count())
            {
                BitmapImage bitmap = new BitmapImage();
                ImageBrush bimg;
                //try
                //{
                //    WebRequest req = WebRequest.Create("http://titar.ir/contents/thumbnail/" + tempjson["thumbnail"]);
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
                bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/contents/thumbnail/" + related_content[index]["thumbnail"].ToString())));
               // }
                CustomButton card1 = new CustomButton();
                card1.Answer = related_content[index]["id"].ToString();
                card1.type = related_content[index]["type"].ToString();
                card1.Margin = new Thickness(5, 5, 5, 5);
                card1.Width = 260;
                card1.Height = 260;
                card1.MouseLeftButtonUp += move_to_detail;
                card1.MouseLeftButtonDown += start_timer;
                card1.Cursor = Cursors.Hand;
                card1.FlowDirection = FlowDirection.RightToLeft;
                card1.Background = bimg;
                List1.Children.Add(card1);

                /// transparent background and wrapper tag
                Grid temp = new Grid();
                temp.VerticalAlignment = VerticalAlignment.Bottom;
                temp.FlowDirection = FlowDirection.RightToLeft;
                temp.Background = (Brush)FindResource("MytransparentBackground");
                temp.Height = 50;
                temp.Name = "item" + index.ToString();
                //temp.MouseEnter +=ShowDetialAnimationOnEnter;
                // temp.MouseLeave += ShowDetialAnimationOnLeave;
                /// title
                TextBlock Title = new TextBlock();
                Title.FontSize = 15;
                Title.Text = related_content[index]["title"].ToString();
                Title.Style = (Style)FindResource("MaterialDesignTitleTextBlock");
                Title.Foreground = Brushes.White;
                Title.TextAlignment = TextAlignment.Center;
                temp.Children.Add(Title);

                TextBlock Author = new TextBlock();
                Author.FontSize = 10;
                Author.Text = "ناشر : " + related_content[index]["author"]["name"].ToString();
                Author.Foreground = Brushes.White;
                Author.HorizontalAlignment = HorizontalAlignment.Right;
                Author.VerticalAlignment = VerticalAlignment.Bottom;
                Author.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Author);
                card1.Content = temp;

                TextBlock Seen_Count = new TextBlock();
                Seen_Count.FontSize = 10;
                Seen_Count.Text = "بازدید : " + related_content[index]["seen_count"].ToString();
                Seen_Count.Foreground = Brushes.White;
                Seen_Count.HorizontalAlignment = HorizontalAlignment.Left;
                Seen_Count.VerticalAlignment = VerticalAlignment.Bottom;
                Seen_Count.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Seen_Count);
                card1.Content = temp;
                index++;
            }
            }
        }

        private void Aaddcomment()
        {
            int index=0;
            while (index < SelectedComment.Count)
            {
                CommentHolder.Children.Clear();
                var color = new BrushConverter();

                TextBlock name = new TextBlock();
                name.Foreground = (Brush)color.ConvertFrom("#bff442");
                if (SelectedComment[index].username["name"].ToString() != "")
                {
                    name.Text = SelectedComment[index].username["name"].ToString();
                }
                else
                {
                    name.Text = "مهمان";
                }
                CommentHolder.Children.Add(name);
                ///
                TextBlock body = new TextBlock();
                body.Margin = new Thickness(8, 8, 8, 8);
                body.Text = SelectedComment[index].body;
                body.TextWrapping = TextWrapping.Wrap;
                CommentHolder.Children.Add(body);
                ///
                if (SelectedComment[index].answer != "")
                {
                    TextBlock answertext = new TextBlock();
                    answertext.Margin = new Thickness(15, 15, 15, 5);
                    answertext.Foreground = Brushes.Red;
                    answertext.Text = SelectedItem.author["name"].ToString();//"پاسخ ";
                    CommentHolder.Children.Add(answertext);
                    ///
                    TextBlock answer = new TextBlock();
                    answer.Margin = new Thickness(25,10, 25, 10);
                    answer.Text = SelectedComment[index].answer;
                    CommentHolder.Children.Add(answer);
                }
                Separator sp = new Separator();
                CommentHolder.Children.Add(sp);
                index++;
            }
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
                    elementName = mouseWasDownOn.Answer;
                }
                
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
                        MaterialDesign2.Pages.Login LoginPage = new MaterialDesign2.Pages.Login();
                        this.NavigationService.Navigate(LoginPage);
                    }
                }
                else
                {

                    // (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();

                    MaterialDesign2.Pages.NoInternet Nointernet = new MaterialDesign2.Pages.NoInternet();
                    this.NavigationService.Navigate(Nointernet);
                }



            }
        }
        
        private void start_timer(object sender, MouseEventArgs e)
        {
            time = e.Timestamp;
        }
        public string device_selected;
        private void Down_App_Open(object sender, RoutedEventArgs e)
        {
            if (SelectedItem.type == "video")
            {
                MaterialDesign2.Pages.Controls.SelectDevice PopupDevice = new MaterialDesign2.Pages.Controls.SelectDevice();
                PopupDevice.SelectedItem = SelectedItem;
                PopupDevice.ShowDialog();
            }
            else if (SelectedItem.type == "photo")
            {
                MaterialDesign2.Pages.Controls.ImagePlayer player = new MaterialDesign2.Pages.Controls.ImagePlayer();
                player.url = "http://titar.ir/contents/photo/" + SelectedItem.file_name;
                player.typeContent = "image";
                player.Show();
            }
            
        }

        public class CustomButton : Card
        {
            public string type { set; get; }
            public string test;
            public string Answer
            {
                get { return test; }
                set { test = value; }
            }
        }

        private void Send_Comment(object sender, RoutedEventArgs e)
        {
            int i;
            if (Comment_Body.Text != "")
            {
                string body = Comment_Body.Text;


                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();


                Thread t = new Thread(
            o =>
            {
                MaterialDesign2.Classes.Comment Send = new MaterialDesign2.Classes.Comment();
                i = Send.Post_Comment(SelectedItem.id, (MaterialDesign.App.Current as MaterialDesign.App).User.id, body);
                Dispatcher.BeginInvoke(
                                  (Action)(() =>
                                  {


                                      (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                      if (i == 0)
                                      {
                                          Comment_Body.Text = "";
                                          MaterialMessageBox.Show("نظر شما برای این ویدئو ارسال شد", "تایید");
                                      }


                                  }));
            });
                t.Start();

            }
        }

        private void Follow_Channel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Follow_Author_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Like_Video_Click(object sender, RoutedEventArgs e)
        {
            MaterialDesign2.Classes.SendLike send_like_class = new MaterialDesign2.Classes.SendLike();
            if (liked == false)
            {
                string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                string url = variables["BaseUrl"] + "rate/like";

                if (send_like_class.send_like(SelectedItem.id, (MaterialDesign.App.Current as MaterialDesign.App).User.id, url) == 0)
                {
                    MaterialDesign2.Classes.Caching Cachelayer = new MaterialDesign2.Classes.Caching();
                    SelectedItem.rated = true;
                    Cachelayer.Store_single_to_cache(SelectedItem, SelectedItem.id);

                    AnimateLikeButton(360);
                    Like.Foreground = Brushes.Red;
                    liked = true;
                }
            }
            else
            {
                string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                string url = variables["BaseUrl"] + "rate/like/delete";
                if (send_like_class.send_like(SelectedItem.id, (MaterialDesign.App.Current as MaterialDesign.App).User.id, url) == 0)
                {
                    MaterialDesign2.Classes.Caching Cachelayer = new MaterialDesign2.Classes.Caching();
                    SelectedItem.rated = false;

                    AnimateLikeButton(-360);
                    Like.Foreground = Brushes.White;
                    liked = false;
                }
            }
        }
        private void AnimateLikeButton(double to, double miliseconds = 1000)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.To = to;
            da.Duration = TimeSpan.FromMilliseconds(miliseconds);
            LikeButtonRotateTransform.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        public class lable_click:Label
        {
            public string SelectId;
        }
        private void go_to_channel(object sender, RoutedEventArgs e)
        {

                MaterialDesign2.Pages.DetialsPage2 details = new MaterialDesign2.Pages.DetialsPage2();
                var mouseWasDownOn = sender as lable_click;
                if (mouseWasDownOn != null)
                {
                    details.id = mouseWasDownOn.SelectId;
                }
                details.content_type = "channel";
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
        private void go_to_author(object sender, RoutedEventArgs e)
        {
            MaterialDesign2.Pages.DetialsPage2 details = new MaterialDesign2.Pages.DetialsPage2();
            var mouseWasDownOn = sender as lable_click;
            if (mouseWasDownOn != null)
            {
                details.user_id = mouseWasDownOn.SelectId;
            }
            details.content_type = "user";
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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //(MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).background.Background = Brushes.Transparent;
        }
    }
}
