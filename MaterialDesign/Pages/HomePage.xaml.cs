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
using MahApps.Metro;
using System.Windows.Threading;

namespace MaterialDesign1.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            this.KeepAlive = false;
        }
        public void reload()
        {

            string ctype = (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).Contetnt_Type;
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                Thread t = new Thread(
                     o =>
                     {
                         (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content.recive_content("last_video", ctype);
                         (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content.recive_content("most_seen", ctype);
                         (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content.recive_content("most_love", ctype);
                         (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content.recive_content("most_down", ctype);

                         Dispatcher.BeginInvoke(
                           (Action)(() =>
                           {

                               (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();

                               List1.Children.Clear();
                               List2.Children.Clear();
                               List3.Children.Clear();
                               List4.Children.Clear();
                               Addtolist1();
                               Addtolist2();
                               Addtolist3();
                               Addtolist4();
                               addbanner();
                           }));
                     });
                t.Start();
                this.ShowsNavigationUI = false;
                //

            }
            else
            {
                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate(No_Internet);
            }


        }
        private void load_home(object sender, RoutedEventArgs e)
        {
            reload();
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(MaterialDesign.MainWindow))
            //    {
            //        (window as MaterialDesign.MainWindow).StartStopWait();
            //    }
            //}
        }

        public void Addtolist1()
        {

            int index = 0;
            while (index < (MaterialDesign.App.Current as MaterialDesign.App).LasteContent.Count)
            {
                RecentTitle.Text = "ویدئو های اخیر";
                ImageBrush bimg = null;

                BitmapImage bitmap = new BitmapImage();

                //try
                //{
                //WebRequest req = WebRequest.Create("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).LasteContent[index].thumbnail.ToString());
                //WebResponse response = req.GetResponse();
                //Stream stream = response.GetResponseStream();
                //bitmap.BeginInit();
                //bitmap.StreamSource = stream;
                //bitmap.CacheOption = BitmapCacheOption.OnLoad;
                //bitmap.EndInit();
                //bimg = new ImageBrush(bitmap);
                //}
                //catch (Exception)
                //{
                bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).LasteContent[index].thumbnail.ToString())));
                //}
                Grid cover = new Grid();
                List1.Children.Add(cover);
                //MahApps.Metro.Controls.ProgressRing ss = new MahApps.Metro.Controls.ProgressRing();
                //ss.VerticalAlignment = VerticalAlignment.Center;
                //ss.HorizontalAlignment = HorizontalAlignment.Center;
                //ss.Width = 50;
                //ss.Height = 50;
                //var color = new BrushConverter();
                //ss.Foreground = (Brush)color.ConvertFrom("#bff442");
                //cover.Children.Add(ss);

                CustomButton card1 = new CustomButton();
                card1.Answer = (MaterialDesign.App.Current as MaterialDesign.App).LasteContent[index].id.ToString();
                card1.type = (MaterialDesign.App.Current as MaterialDesign.App).LasteContent[index].type.ToString(); ;
                card1.Margin = new Thickness(5, 5, 5, 5);
                card1.Width = 300;
                card1.Height = 300;
                card1.MouseLeftButtonUp += move_to_detail;
                card1.MouseLeftButtonDown += start_timer;
                card1.Cursor = Cursors.Hand;
                card1.Background = bimg;
                cover.Children.Add(card1);

                /// transparent background and wrapper tag
                Grid temp = new Grid();
                temp.VerticalAlignment = VerticalAlignment.Bottom;
                temp.FlowDirection = FlowDirection.RightToLeft;
                temp.Background = (Brush)FindResource("MytransparentBackground");
                temp.Height = 50;
                temp.Name = "item" + index.ToString();
                //temp.MouseEnter += ShowDetialAnimationOnEnter;
                //temp.MouseLeave += ShowDetialAnimationOnLeave;
                // title
                TextBlock Title = new TextBlock();
                Title.FontSize = 15;
                Title.Text = (MaterialDesign.App.Current as MaterialDesign.App).LasteContent[index].title;
                Title.Style = (Style)FindResource("MaterialDesignTitleTextBlock");
                Title.Foreground = Brushes.White;
                Title.TextAlignment = TextAlignment.Center;
                temp.Children.Add(Title);

                TextBlock Author = new TextBlock();
                Author.FontSize = 10;
                Author.Text = "ناشر : " + (MaterialDesign.App.Current as MaterialDesign.App).LasteContent[index].author["name"];
                Author.Foreground = Brushes.White;
                Author.HorizontalAlignment = HorizontalAlignment.Right;
                Author.VerticalAlignment = VerticalAlignment.Bottom;
                Author.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Author);
                card1.Content = temp;

                TextBlock Seen_Count = new TextBlock();
                Seen_Count.FontSize = 10;
                Seen_Count.Text = "بازدید : " + (MaterialDesign.App.Current as MaterialDesign.App).LasteContent[index].seen_count;
                Seen_Count.Foreground = Brushes.White;
                Seen_Count.HorizontalAlignment = HorizontalAlignment.Left;
                Seen_Count.VerticalAlignment = VerticalAlignment.Bottom;
                Seen_Count.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Seen_Count);
                card1.Content = temp;
                index++;
            }
        }
        public void Addtolist2()
        {

            int index = 0;
            while (index < (MaterialDesign.App.Current as MaterialDesign.App).MostSeen.Count)
            {
                Animation.Text = "انیمیشن";
                BitmapImage bitmap = new BitmapImage();
                ImageBrush bimg;
                //try
                //{
                //    WebRequest req = WebRequest.Create("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).MostSeen[index].thumbnail.ToString());
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
                bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).MostSeen[index].thumbnail.ToString())));
                //}
                Grid cover = new Grid();
                cover.Width = 300;
                cover.Height = 300;
                List2.Children.Add(cover);

                //MahApps.Metro.Controls.ProgressRing ss = new MahApps.Metro.Controls.ProgressRing();
                //ss.VerticalAlignment = VerticalAlignment.Center;
                //ss.HorizontalAlignment = HorizontalAlignment.Center;
                //ss.Width = 50;
                //ss.Height = 50;
                //var color = new BrushConverter();
                //ss.Foreground = (Brush)color.ConvertFrom("#bff442");
                //cover.Children.Add(ss);

                CustomButton card1 = new CustomButton();
                card1.Answer = (MaterialDesign.App.Current as MaterialDesign.App).MostSeen[index].id.ToString();
                card1.type = (MaterialDesign.App.Current as MaterialDesign.App).MostSeen[index].type.ToString();
                card1.Margin = new Thickness(5, 5, 5, 5);
                card1.Width = 300;
                card1.Height = 300;
                card1.MouseLeftButtonUp += move_to_detail;
                card1.MouseLeftButtonDown += start_timer;
                card1.Cursor = Cursors.Hand;
                card1.Background = bimg;
                cover.Children.Add(card1);

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
                Title.Text = (MaterialDesign.App.Current as MaterialDesign.App).MostSeen[index].title;
                Title.Style = (Style)FindResource("MaterialDesignTitleTextBlock");
                Title.Foreground = Brushes.White;
                Title.TextAlignment = TextAlignment.Center;
                temp.Children.Add(Title);

                TextBlock Author = new TextBlock();
                Author.FontSize = 10;
                Author.Text = "ناشر : " + (MaterialDesign.App.Current as MaterialDesign.App).MostSeen[index].author["name"];
                Author.Foreground = Brushes.White;
                Author.HorizontalAlignment = HorizontalAlignment.Right;
                Author.VerticalAlignment = VerticalAlignment.Bottom;
                Author.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Author);
                card1.Content = temp;

                TextBlock Seen_Count = new TextBlock();
                Seen_Count.FontSize = 10;
                Seen_Count.Text = "بازدید : " + (MaterialDesign.App.Current as MaterialDesign.App).MostSeen[index].seen_count;
                Seen_Count.Foreground = Brushes.White;
                Seen_Count.HorizontalAlignment = HorizontalAlignment.Left;
                Seen_Count.VerticalAlignment = VerticalAlignment.Bottom;
                Seen_Count.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Seen_Count);
                card1.Content = temp;
                index++;
            }
        }
        public void Addtolist3()
        {

            int index = 0;
            while (index < (MaterialDesign.App.Current as MaterialDesign.App).MostLove.Count)
            {
                Music.Text = "موزیک";
                BitmapImage bitmap = new BitmapImage();
                ImageBrush bimg;
                //try
                //{
                //    WebRequest req = WebRequest.Create("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).MostLove[index].thumbnail.ToString());
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
                bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).MostLove[index].thumbnail.ToString())));
                //}
                Grid cover = new Grid();
                cover.Width = 300;
                cover.Height = 300;
                List3.Children.Add(cover);

                //MahApps.Metro.Controls.ProgressRing ss = new MahApps.Metro.Controls.ProgressRing();
                //ss.VerticalAlignment = VerticalAlignment.Center;
                //ss.HorizontalAlignment = HorizontalAlignment.Center;
                //ss.Width = 50;
                //ss.Height = 50;
                //var color = new BrushConverter();
                //ss.Foreground = (Brush)color.ConvertFrom("#bff442");
                //cover.Children.Add(ss);

                CustomButton card1 = new CustomButton();
                card1.Answer = (MaterialDesign.App.Current as MaterialDesign.App).MostLove[index].id.ToString();
                card1.type = (MaterialDesign.App.Current as MaterialDesign.App).MostLove[index].type.ToString();

                card1.Margin = new Thickness(5, 5, 5, 5);
                card1.Width = 300;
                card1.Height = 300;
                card1.MouseLeftButtonUp += move_to_detail;
                card1.MouseLeftButtonDown += start_timer;
                card1.Cursor = Cursors.Hand;
                card1.Background = bimg;
                cover.Children.Add(card1);

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
                Title.Text = (MaterialDesign.App.Current as MaterialDesign.App).MostLove[index].title;
                Title.Style = (Style)FindResource("MaterialDesignTitleTextBlock");
                Title.Foreground = Brushes.White;
                Title.TextAlignment = TextAlignment.Center;
                temp.Children.Add(Title);

                TextBlock Author = new TextBlock();
                Author.FontSize = 10;
                Author.Text = "ناشر : " + (MaterialDesign.App.Current as MaterialDesign.App).MostLove[index].author["name"];
                Author.Foreground = Brushes.White;
                Author.HorizontalAlignment = HorizontalAlignment.Right;
                Author.VerticalAlignment = VerticalAlignment.Bottom;
                Author.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Author);
                card1.Content = temp;

                TextBlock Seen_Count = new TextBlock();
                Seen_Count.FontSize = 10;
                Seen_Count.Text = "بازدید : " + (MaterialDesign.App.Current as MaterialDesign.App).MostLove[index].seen_count;
                Seen_Count.Foreground = Brushes.White;
                Seen_Count.HorizontalAlignment = HorizontalAlignment.Left;
                Seen_Count.VerticalAlignment = VerticalAlignment.Bottom;
                Seen_Count.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Seen_Count);
                card1.Content = temp;

                index++;
            }
        }
        public void Addtolist4()
        {

            int index = 0;
            while (index < (MaterialDesign.App.Current as MaterialDesign.App).MostDown.Count)
            {
                Sport.Text = "ورزشی";
                BitmapImage bitmap = new BitmapImage();
                ImageBrush bimg;
                //try
                //{
                //    WebRequest req = WebRequest.Create("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).MostDown[index].thumbnail.ToString());
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
                bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).MostDown[index].thumbnail.ToString())));
                //}
                Grid cover = new Grid();
                cover.Width = 300;
                cover.Height = 300;
                List4.Children.Add(cover);

                //MahApps.Metro.Controls.ProgressRing ss = new MahApps.Metro.Controls.ProgressRing();
                //ss.VerticalAlignment = VerticalAlignment.Center;
                //ss.HorizontalAlignment = HorizontalAlignment.Center;
                //ss.Width = 50;
                //ss.Height = 50;
                //var color = new BrushConverter();
                //ss.Foreground = (Brush)color.ConvertFrom("#bff442");
                //cover.Children.Add(ss);

                CustomButton card1 = new CustomButton();
                card1.Answer = (MaterialDesign.App.Current as MaterialDesign.App).MostDown[index].id.ToString();
                card1.type = (MaterialDesign.App.Current as MaterialDesign.App).MostDown[index].type.ToString();

                card1.Margin = new Thickness(5, 5, 5, 5);
                card1.Width = 300;
                card1.Height = 300;
                card1.MouseLeftButtonUp += move_to_detail;
                card1.MouseLeftButtonDown += start_timer;
                card1.Cursor = Cursors.Hand;
                card1.Background = bimg;
                cover.Children.Add(card1);

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
                Title.Text = (MaterialDesign.App.Current as MaterialDesign.App).MostDown[index].title;
                Title.Style = (Style)FindResource("MaterialDesignTitleTextBlock");
                Title.Foreground = Brushes.White;
                Title.TextAlignment = TextAlignment.Center;
                temp.Children.Add(Title);

                TextBlock Author = new TextBlock();
                Author.FontSize = 10;
                Author.Text = "ناشر : " + (MaterialDesign.App.Current as MaterialDesign.App).MostDown[index].author["name"];
                Author.Foreground = Brushes.White;
                Author.HorizontalAlignment = HorizontalAlignment.Right;
                Author.VerticalAlignment = VerticalAlignment.Bottom;
                Author.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Author);
                card1.Content = temp;

                TextBlock Seen_Count = new TextBlock();
                Seen_Count.FontSize = 10;
                Seen_Count.Text = "بازدید : " + (MaterialDesign.App.Current as MaterialDesign.App).MostDown[index].seen_count;
                Seen_Count.Foreground = Brushes.White;
                Seen_Count.HorizontalAlignment = HorizontalAlignment.Left;
                Seen_Count.VerticalAlignment = VerticalAlignment.Bottom;
                Seen_Count.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Seen_Count);
                card1.Content = temp;

                index++;
            }
        }

        private void ShowDetialAnimationOnEnter(object sender, MouseEventArgs e)
        {
            playDetialAnimation((StackPanel)sender);
        }
        private void playDetialAnimation(StackPanel sp)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = sp.Height;
            da.To = 100;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            sp.BeginAnimation(StackPanel.HeightProperty, da);
        }
        private void ShowDetialAnimationOnLeave(object sender, MouseEventArgs e)
        {
            HideDetialAnimation((StackPanel)sender);

        }
        private void HideDetialAnimation(StackPanel sp)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = sp.Height;
            da.To = 50;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            sp.BeginAnimation(StackPanel.HeightProperty, da);
        }
        /// <summary>
        /// // click items setting
        /// </summary>
        /// 
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

                (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content.number_of_try = 0;

                if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                {
                    if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                    {

                        (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();

                        (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).scrollbar1.ScrollToTop();

                        if (mouseWasDownOn.type == "app" || mouseWasDownOn.type == "game")
                        {
                            MaterialDesign2.Pages.AppGamePage details = new MaterialDesign2.Pages.AppGamePage();
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

        /// <summary>
        /// add banner
        /// </summary>

        public List<string> bannertext= new List<string>();
        public void addbanner()
        {
            MaterialDesign2.Classes.Banners bannercontent = new MaterialDesign2.Classes.Banners();
            List<MaterialDesign2.Classes.Banners> bannercontent1 = bannercontent.get_banners();
            if (bannercontent1 != null)
            {
                int index = 0;
                while (index < bannercontent1.Count)
                {
                    //Image thumbnail = new Image();
                    //BitmapImage image = new BitmapImage(new Uri("http://titar.ir/thumbnails/banners/" + bannercontent1[index].thumbnail));
                    //thumbnail.Source = image;

                    banneritems grid = new banneritems();
                    ImageBrush bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/thumbnails/banners/" + bannercontent1[index].thumbnail)));

                    grid.Background = bimg;
                    
                    grid.link = bannercontent1[index].link;
                    grid.MouseDown += open_link;
                    FlipView.Items.Add(grid);
                    bannertext.Add( bannercontent1[index].title);
                    index++;
                }
                int change = 1;

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Tick += (o, a) =>
                {
                    // If we'd go out of bounds then reverse
                    int newIndex = FlipView.SelectedIndex + change;
                    if (newIndex >= FlipView.Items.Count || newIndex < 0)
                    {
                        change *= -1;
                    }

                    FlipView.SelectedIndex += change;
                };
                timer.Start();
            }

        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipview = ((MahApps.Metro.Controls.FlipView)sender);
            if(bannertext.Count!=0)
            FlipView.BannerText = bannertext[flipview.SelectedIndex];
        }
        private void open_link(object sender, MouseEventArgs e)
        {
            string link = "";
        
                var mouseWasDownOn = sender as banneritems;
            if (mouseWasDownOn != null)
            {
                try { 
                link = mouseWasDownOn.link;
               // link = "https://mahapps.com/guides/";
                System.Diagnostics.Process.Start(link);
                }
                catch
                {
                     ////sentry
                }
            }
        }

        public class banneritems : Grid
        { 
            public string link;
        }
        //// add a custom property
        public class CustomButton : Card
        {
              public string type { set; get; }
              public string test;
              public string Answer
              {
                  get { return test; }
                  set { test = value; }
              }
            //public int SelectedItem_Id
            //{
            //    get { return (int)GetValue(id); }
            //    set { SetValue(id, value); }
            //}

            //// Using a DependencyProperty as the backing store for MyProperty. This enables animation, styling, binding, etc...
            //public static readonly DependencyProperty id =
            //  DependencyProperty.Register("MyProperty", typeof(int), typeof(CustomButton), new UIPropertyMetadata(0));


        }

       
    }
}
