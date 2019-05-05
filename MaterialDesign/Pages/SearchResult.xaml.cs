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
using System.Windows.Threading;
using System.Threading;
namespace MaterialDesign2.Pages
{
    /// <summary>
    /// Interaction logic for DetialsPage2.xaml
    /// </summary>
    public partial class SearchResult : Page
    {
        public string content_type = null;
        public string query;
        public string query_search
        {
            get { return query; }
            set { query = value; }
        }
        public string type
        {
            get { return content_type; }
            set { content_type = value; }
        }
        public SearchResult()
        {
            InitializeComponent();
        }

        public class Tile
        {
            public CustomButton Name { get; set; }
        }
        private void DetialsPage2_Loaded(object sender, RoutedEventArgs e)
        {

           // (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
            
            /// recive data 
            Thread t = new Thread(
                             o =>
                             {
                                
                                 (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.url = "http://titar.ir/api/pc/search/" + query;
                                 (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.get_connection();
                                 (MaterialDesign.App.Current as MaterialDesign.App).SearchResult = (MaterialDesign.App.Current as MaterialDesign.App).Recive_Content1.response_content["contents"];
                                 Dispatcher.BeginInvoke(
                                   (Action)(() =>
                                   {
                                       (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                               Addtolist2();
                                   }));
                             });
            t.Start();
            /// end recive
            this.ShowsNavigationUI = false;
            
        }
        private void Addtolist2()
        {
            int index = 0;
            while (index < (MaterialDesign.App.Current as MaterialDesign.App).SearchResult.Count())
            {
                BitmapImage bitmap = new BitmapImage();
                ImageBrush bimg;
                //try
                //{
                //    WebRequest req = WebRequest.Create("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).SearchResult[index].thumbnail.ToString());
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
                bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/contents/thumbnail/" + (MaterialDesign.App.Current as MaterialDesign.App).SearchResult[index]["thumbnail"].ToString())));
                //} 
                CustomButton card1 = new CustomButton();
                card1.Answer = (MaterialDesign.App.Current as MaterialDesign.App).SearchResult[index]["id"].ToString();
                card1.type = (MaterialDesign.App.Current as MaterialDesign.App).SearchResult[index]["type"].ToString();
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
                Title.Text = (MaterialDesign.App.Current as MaterialDesign.App).SearchResult[index]["title"].ToString();
                Title.Style = (Style)FindResource("MaterialDesignTitleTextBlock");
                Title.Foreground = Brushes.White;
                Title.TextAlignment = TextAlignment.Center;
                temp.Children.Add(Title);

                TextBlock Author = new TextBlock();
                Author.FontSize = 10;
                Author.Text = "ناشر : " + (MaterialDesign.App.Current as MaterialDesign.App).SearchResult[index]["author"]["name"].ToString() ;
                Author.Foreground = Brushes.White;
                Author.HorizontalAlignment = HorizontalAlignment.Right;
                Author.VerticalAlignment = VerticalAlignment.Bottom;
                Author.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Author);
                card1.Content = temp;

                TextBlock Seen_Count = new TextBlock();
                Seen_Count.FontSize = 10;
                Seen_Count.Text = "بازدید : " + (MaterialDesign.App.Current as MaterialDesign.App).SearchResult[index]["seen_count"];
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
