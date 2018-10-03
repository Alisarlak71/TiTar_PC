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
using System.Windows.Threading;
using Dragablz;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Threading;
using MahApps.Metro.Controls.Dialogs;
namespace MaterialDesign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public bool status_loading=false;
        public string Contetnt_Type="video";
        private void draggableListView_mouseWheel(object sender, MouseWheelEventArgs e)
        {
            int i = e.Delta;             
            if (i>0)
            {
                
            }
            else
            {

            }
            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;
            scrollbar1.RaiseEvent(e2);
        }
        public void StartStopWait()
        {
            LoadingAdorner.Dispatcher.BeginInvoke(
                             (Action)(() =>
                             {
                                 //LoadingAdorner.IsAdornerVisible = !LoadingAdorner.IsAdornerVisible;
                                 if(status_loading==false)
                                 {
                                    loadingbar.Visibility = Visibility.Visible;
                                    status_loading = !status_loading;
                                 }
                                 else
                                 {
                                     loadingbar.Visibility = Visibility.Hidden;
                                     status_loading = !status_loading;
                                 }
                             }
         ));
            
        }
        public MainWindow()
        {
           
            //// show splash
            SplashScreen splashScreen = new SplashScreen("/Images/splash.jpg");
            splashScreen.Show(true);
            //// end show splash
            InitializeComponent();
            
            /////add mouse wheel handler for scrollviewer
            scrollbar1.AddHandler(PreviewMouseWheelEvent, new MouseWheelEventHandler(draggableListView_mouseWheel), true);
            
            /// logout hide
            LogoutTreeItem.Visibility = Visibility.Collapsed;
            AccountSettingTreeItem.Visibility = Visibility.Collapsed;
            AccountChannelsTreeItem.Visibility = Visibility.Collapsed;
            AccountContentTreeItem.Visibility = Visibility.Collapsed;
            AccountFavoriteTreeItem.Visibility = Visibility.Collapsed;
            /// end logout hide
            

            /// create event for mouse down on items of treeview
            Home.MouseDown += Update_tabcontrol;
            Home.Cursor = Cursors.Hand; 
            Login.MouseDown += go_to_login;
            Login.Cursor = Cursors.Hand;
            Logout.MouseDown += logout_from_account;
            Logout.Cursor = Cursors.Hand;
            Serial.MouseDown += go_to_serial;
            Serial.Cursor = Cursors.Hand;

            New.MouseDown += go_to_most_new;
            New.Cursor = Cursors.Hand;
            MostSell.MouseDown += go_to_most_sell;
            MostSell.Cursor = Cursors.Hand; 
            Favorite.MouseDown += go_to_most_love;
            Favorite.Cursor = Cursors.Hand;
            MostSeen.MouseDown += go_to_most_seen;
            MostSeen.Cursor = Cursors.Hand;
            MostDown.MouseDown += go_to_most_down;
            MostDown.Cursor = Cursors.Hand;

            AccountMyChannel.Cursor = Cursors.Hand;
            AccountMyChannel.MouseDown += go_to_my_channel;

            AccountAddVideo.Cursor = Cursors.Hand;
            AccountAddVideo.MouseDown += go_to_create_video;

            AccountMyVideo.Cursor = Cursors.Hand;
            AccountMyVideo.MouseDown += go_to_my_videos;

            AccountFavoriteChannel.Cursor = Cursors.Hand;
            AccountFavoriteChannel.MouseDown += go_to_my_followed_channels;

            AccountCreateChannel.Cursor = Cursors.Hand;
            AccountCreateChannel.MouseDown += go_to_create_channel;
            /// end create event for mouse down on items of treeview

            //// set placeholder for
            searchbox.Text = "جستجو";
            searchbox.Foreground = Brushes.White;
            searchbox.FontSize = 20;
            searchbox.GotFocus += RemoveText;
            searchbox.LostFocus += AddText;
        }
       
        public void RemoveText(object sender, RoutedEventArgs e)
            {
                searchbox.Text = "";
            }

        public void AddText(object sender, RoutedEventArgs e)
        {
           if (String.IsNullOrWhiteSpace(searchbox.Text))
               searchbox.Text = "جستجو";
        }
        private void Update_tabcontrol(object sender, MouseEventArgs e)
        {
            scrollbar1.ScrollToTop();
            StartStopWait();
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    MaterialDesign1.Pages.HomePage home = new MaterialDesign1.Pages.HomePage();
                    HomeFrame.Navigate(home);
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }
        private void go_to_login(object sender, MouseEventArgs e)
        {
            StartStopWait();
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                HomeFrame.Navigate(login_page);
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet); 
            }
        }
        private void logout_from_account(object sender, MouseEventArgs e)
        {
            StartStopWait();
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                (MaterialDesign.App.Current as MaterialDesign.App).check_registery.delete_from_registery();
                LogoutTreeItem.Visibility = Visibility.Collapsed;
                AccountChannelsTreeItem.Visibility = Visibility.Collapsed;
                AccountContentTreeItem.Visibility = Visibility.Collapsed;
                AccountFavoriteTreeItem.Visibility = Visibility.Collapsed;
                AccountSettingTreeItem.Visibility = Visibility.Collapsed;
                LoginTreeItem.Visibility = Visibility.Visible;
                (MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in = false;
                (MaterialDesign.App.Current as MaterialDesign.App).login_account.status = false;
                (MaterialDesign.App.Current as MaterialDesign.App).User=null;
                (MaterialDesign.App.Current as MaterialDesign.App).check_registery.decrypted_password = "";
                (MaterialDesign.App.Current as MaterialDesign.App).check_registery.decrypted_username = "";
                MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                HomeFrame.Navigate(login_page);
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }
        
        private void go_to_serial(object sender, MouseEventArgs e)
        {
            StartStopWait();
                scrollbar1.ScrollToTop();
                if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() ==  true)
                {
                    if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                    {
                        MaterialDesign2.Pages.DetialsPage2 SerialPage = new MaterialDesign2.Pages.DetialsPage2();
                        SerialPage.content_type = "most_seen";
                        HomeFrame.Navigate(SerialPage);
                    }
                    else
                    {
                        MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                        HomeFrame.Navigate(login_page);
                    }
                }

                else
                {
                    MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                    HomeFrame.Navigate(No_Internet);
                }

        }
        /// <summary>
        /// create event for videos
        /// </summary>
        private void go_to_most_seen(object sender, MouseEventArgs e)
        {
            StartStopWait();
            scrollbar1.ScrollToTop();
                         if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                         {
                             if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                             {
                                 MaterialDesign2.Pages.DetialsPage2 MostSeenPage = new MaterialDesign2.Pages.DetialsPage2();
                                 MostSeenPage.content_type = "most_seen";
                                 HomeFrame.Navigate(MostSeenPage);
                             }
                             else
                             {
                                 MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                                 HomeFrame.Navigate(login_page);
                             }
                         }

                         else
                         {
                             MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                             HomeFrame.Navigate(No_Internet);
                         }
        }
        private void go_to_most_new(object sender, MouseEventArgs e)
        {
            StartStopWait();
            scrollbar1.ScrollToTop();
                         if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                         {
                             if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                             {
                                 MaterialDesign2.Pages.DetialsPage2 MostNewPage = new MaterialDesign2.Pages.DetialsPage2();
                                 MostNewPage.content_type = "last_video";
                                 HomeFrame.Navigate(MostNewPage);
                             }
                             else
                             {
                                 MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                                 HomeFrame.Navigate(login_page);
                             }
                         }

                         else
                         {
                             MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                             HomeFrame.Navigate(No_Internet);
                         }

                 


        }
        private void go_to_most_down(object sender, MouseEventArgs e)
        {
            StartStopWait();
            scrollbar1.ScrollToTop();

                         if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                         {
                             if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                             {
                                 MaterialDesign2.Pages.DetialsPage2 MostDownPage = new MaterialDesign2.Pages.DetialsPage2();
                                 MostDownPage.content_type = "most_down";
                                 HomeFrame.Navigate(MostDownPage);
                             }
                             else
                             {
                                 MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                                 HomeFrame.Navigate(login_page);
                             }
                         }

                         else
                         {
                             MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                             HomeFrame.Navigate(No_Internet);
                         }

            


        }
        private void go_to_most_love(object sender, MouseEventArgs e)
        {

            StartStopWait();
            scrollbar1.ScrollToTop();

                         if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                         {
                             if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                             {
                                 MaterialDesign2.Pages.DetialsPage2 MostLovenPage = new MaterialDesign2.Pages.DetialsPage2();
                                 MostLovenPage.content_type = "most_love";
                                 HomeFrame.Navigate(MostLovenPage);
                             }
                             else
                             {
                                 MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                                 HomeFrame.Navigate(login_page);
                             }
                         }

                         else
                         {
                             MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                             HomeFrame.Navigate(No_Internet);
                         }

                  

        }
        private void go_to_most_sell(object sender, MouseEventArgs e)
        {
            StartStopWait();
            scrollbar1.ScrollToTop();

                         if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                         {
                             if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                             {
                                 MaterialDesign2.Pages.DetialsPage2 MostSellPage = new MaterialDesign2.Pages.DetialsPage2();
                                 MostSellPage.content_type = "most_sell";
                                 HomeFrame.Navigate(MostSellPage);
                             }
                             else
                             {
                                 MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                                 HomeFrame.Navigate(login_page);
                             }
                         }

                         else
                         {
                             MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                             HomeFrame.Navigate(No_Internet);
                         }

                   


        }

        private void go_to_my_channel(object sender, MouseEventArgs e)
        {
            StartStopWait();
            scrollbar1.ScrollToTop();

            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    MaterialDesign2.Pages.Management.MyChannels my_channels = new MaterialDesign2.Pages.Management.MyChannels();
                    HomeFrame.Navigate(my_channels);
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }

            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }




        }
        private void go_to_create_channel(object sender, MouseEventArgs e)
        {
            scrollbar1.ScrollToTop();

            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    MaterialDesign2.Pages.Management.CreateChannel create_channel = new MaterialDesign2.Pages.Management.CreateChannel();
                    HomeFrame.Navigate(create_channel);
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }
        private void go_to_create_video(object sender, MouseEventArgs e)
        {
            scrollbar1.ScrollToTop();
            StartStopWait();
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    MaterialDesign2.Pages.Management.CreateVideo create_video = new MaterialDesign2.Pages.Management.CreateVideo();
                    HomeFrame.Navigate(create_video);
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }

        private void go_to_my_followed_channels(object sender, MouseEventArgs e)
        {

            //scrollbar1.ScrollToTop();

            //if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            //{
            //    if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
            //    {
            //        MaterialDesign2.Pages.Management.FollowedChannels followed_channel = new MaterialDesign2.Pages.Management.FollowedChannels();
            //        HomeFrame.Navigate(followed_channel);
            //    }
            //    else
            //    {
            //        MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
            //        HomeFrame.Navigate(login_page);
            //    }
            //}

            //else
            //{
            //    HomeFrame.Navigate(No_Internet);
            //}

        }

        private void go_to_my_videos(object sender, MouseEventArgs e)
        {
            scrollbar1.ScrollToTop();

            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {

                    MaterialDesign2.Pages.Management.MyVideos my_videos_pages = new MaterialDesign2.Pages.Management.MyVideos();
                    HomeFrame.Navigate(my_videos_pages);
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }

        private void Search_All(object sender, RoutedEventArgs e)
        {
            scrollbar1.ScrollToTop();
            StartStopWait();
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    MaterialDesign2.Pages.SearchResult SearchResultPage = new MaterialDesign2.Pages.SearchResult();
                    SearchResultPage.query = searchbox.Text;
                    HomeFrame.Navigate(SearchResultPage);
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }

            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }


        }
        public void reload()
        {

            /// navigate to home page at startup
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                //
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    MaterialDesign1.Pages.HomePage homepage = new MaterialDesign1.Pages.HomePage();
                    HomeFrame.Navigate(homepage);
                }
                else
                {
                    (MaterialDesign.App.Current as MaterialDesign.App).check_registery.read_from_registery();

                    if ((MaterialDesign.App.Current as MaterialDesign.App).check_registery.decrypted_password != "" && (MaterialDesign.App.Current as MaterialDesign.App).check_registery.decrypted_username != "")
                    {
                        (MaterialDesign.App.Current as MaterialDesign.App).login_account.number_of_try = 0;
                        (MaterialDesign.App.Current as MaterialDesign.App).login_account.Login_To_Account((MaterialDesign.App.Current as MaterialDesign.App).check_registery.decrypted_username, (MaterialDesign.App.Current as MaterialDesign.App).check_registery.decrypted_password);

                        if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.status == true)
                        {
                            StartStopWait();
                            MaterialDesign1.Pages.HomePage homepage = new MaterialDesign1.Pages.HomePage();
                            HomeFrame.Navigate(homepage);
                            LoginTreeItem.Visibility = Visibility.Collapsed;
                            LogoutTreeItem.Visibility = Visibility.Visible;
                            AccountChannelsTreeItem.Visibility = Visibility.Visible;
                            AccountContentTreeItem.Visibility = Visibility.Visible;
                            AccountFavoriteTreeItem.Visibility = Visibility.Visible;
                            AccountSettingTreeItem.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            StartStopWait();
                            MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                            HomeFrame.Navigate(login_page);
                        }
                    }
                    else
                    {
                        StartStopWait();
                        MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                        HomeFrame.Navigate(login_page);
                    }
                }
            }
            else
            {
                StartStopWait();
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }
        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ///// reload mainwindow for navigate to home page or no internet or login 
            reload();
            ///// end reload mainwindow for navigate to home page or no internet or login
        }

        private void Photo_Click(object sender, RoutedEventArgs e)
        {
            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).Contetnt_Type = "photo";

            scrollbar1.ScrollToTop();
            StartStopWait();
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    var color = new BrushConverter();

                    MaterialDesign1.Pages.HomePage homePage = new MaterialDesign1.Pages.HomePage();
                    HomeFrame.Navigate(homePage);

                    PhotoButton.Background = (Brush)color.ConvertFrom("#bff442");
                    VideoButton.Background = Brushes.Transparent;
                    GameButton.Background = Brushes.Transparent;
                    AppButton.Background = Brushes.Transparent;
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }
        private void Video_Click(object sender, RoutedEventArgs e)
        {
            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).Contetnt_Type = "video";

            scrollbar1.ScrollToTop();
            StartStopWait();
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    var color = new BrushConverter();

                    MaterialDesign1.Pages.HomePage homePage = new MaterialDesign1.Pages.HomePage();
                    HomeFrame.Navigate(homePage);

                    PhotoButton.Background = Brushes.Transparent;
                    AppButton.Background = Brushes.Transparent;
                    GameButton.Background = Brushes.Transparent;
                    VideoButton.Background = (Brush)color.ConvertFrom("#bff442");
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }
        private void Game_Click(object sender, RoutedEventArgs e)
        {
            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).Contetnt_Type = "game";
            scrollbar1.ScrollToTop();
            StartStopWait();
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    var color = new BrushConverter();

                    MaterialDesign1.Pages.HomePage homePage = new MaterialDesign1.Pages.HomePage();
                    HomeFrame.Navigate(homePage);

                    PhotoButton.Background = Brushes.Transparent;
                    AppButton.Background = Brushes.Transparent;
                    GameButton.Background = (Brush)color.ConvertFrom("#bff442");
                    VideoButton.Background = Brushes.Transparent;
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }
        private void App_Click(object sender, RoutedEventArgs e)
        {
            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).Contetnt_Type = "app";
            scrollbar1.ScrollToTop();
            StartStopWait();
            if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
            {
                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                {
                    var color = new BrushConverter();

                    MaterialDesign1.Pages.HomePage homePage = new MaterialDesign1.Pages.HomePage();
                    HomeFrame.Navigate(homePage);

                    PhotoButton.Background = Brushes.Transparent;
                    AppButton.Background = (Brush)color.ConvertFrom("#bff442");
                    GameButton.Background = Brushes.Transparent;
                    VideoButton.Background = Brushes.Transparent;
                }
                else
                {
                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                    HomeFrame.Navigate(login_page);
                }
            }
            else
            {
                MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                HomeFrame.Navigate(No_Internet);
            }
        }
        //public bool dialogstatus = false;
        private void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (dialog.IsOpen == false)
            {
                dialog.IsOpen = true;
                //dialogstatus = true;
            }
            else
            {
                dialog.IsOpen = false;
               // dialogstatus = false;
            }
                
        }

        private void dialog_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            dialog.IsOpen = false;
        }

        private void HomeFrame_Navigated(object sender, NavigationEventArgs e)
        {
            dialog.IsOpen = false;
        }
    }
}
