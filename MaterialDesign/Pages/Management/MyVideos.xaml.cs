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

namespace MaterialDesign2.Pages.Management
{
    /// <summary>
    /// Interaction logic for DetialsPage2.xaml
    /// </summary>
    public partial class MyVideos : Page
    {
        public MyVideos()
        {
            InitializeComponent();
        }

        public class Tile
        {
          public CustomButton Name { get; set; }
        }
        private void MyVideo_Loaded(object sender, RoutedEventArgs e)
        {

            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
               
            Thread t = new Thread(
                             o =>
                             {
                                     MaterialDesign2.Classes.Users channels = new MaterialDesign2.Classes.Users();
                                     List<MaterialDesign2.Classes.Content> my_contents = new List<MaterialDesign2.Classes.Content>();
                                     my_contents = channels.get_user_contents((MaterialDesign.App.Current as MaterialDesign.App).User.id);
                                     Dispatcher.BeginInvoke(
                                   (Action)(() =>
                                   {

                                       (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                               TextBlock Title = new TextBlock();
                                               Title.Foreground = Brushes.White;
                                               Title.Margin = new Thickness(5);
                                               if (my_contents.Count > 0)
                                               {
                                                   Title.Text = "ویدئو های شما";
                                               }
                                               else
                                               {
                                                   Title.Text = "هنوز محتوایی توسط شما ثبت نشده";
                                               }
                                               Detials_Content.Children.Add(Title);
                                               Addtolist2(my_contents);
                                               
                                         

                                       }));
                                       });
            t.Start();

            this.ShowsNavigationUI = false;
            
        }
        
        private void Addtolist2(List<MaterialDesign2.Classes.Content> Content1)
        {
            int index = 0;
            while (index < Content1.Count)
            {
                BitmapImage bitmap = new BitmapImage();
                ImageBrush bimg;
                
                bimg = new ImageBrush(new BitmapImage(new Uri("http://titar.ir/contents/thumbnail/" + Content1[index].thumbnail)));
                
                CustomButton card1 = new CustomButton();
                card1.id = Content1[index].id.ToString();
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
                Title.Text = Content1[index].title;
                Title.Style = (Style)FindResource("MaterialDesignTitleTextBlock");
                Title.Foreground = Brushes.White;
                Title.TextAlignment = TextAlignment.Center;
                temp.Children.Add(Title);

                TextBlock Edit = new TextBlock();
                Edit.FontSize = 15;
                Edit.Text = "ویرایش";
                var color = new BrushConverter();
                Edit.Foreground = (Brush)color.ConvertFrom("#bff442");
                Edit.HorizontalAlignment = HorizontalAlignment.Right;
                Edit.VerticalAlignment = VerticalAlignment.Bottom;
                Edit.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Edit);
                card1.Content = temp;

                TextBlock Delete = new TextBlock();
                Delete.FontSize = 15;
                Delete.Text = "حذف";
                Delete.Foreground = Brushes.Red;
                Delete.HorizontalAlignment = HorizontalAlignment.Left;
                Delete.VerticalAlignment = VerticalAlignment.Bottom;
                Delete.Margin = new Thickness(5, 5, 5, 5);
                temp.Children.Add(Delete);
                card1.Content = temp;
                listview.Items.Add(new Tile()
                {
                    Name = card1,
                });
        
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
                    elementName = mouseWasDownOn.id;
                }
              MaterialDesign1.Pages.DetialsPage1 details = new MaterialDesign1.Pages.DetialsPage1();

                if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                {
                    if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                    {
                        
                           (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).scrollbar1.ScrollToTop();
                        
                        details.selected_id = elementName;
                        this.NavigationService.Navigate(details);
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
        //// add a custom property
        public class CustomButton : Card
        {
            public string id;
        }
    }
}
