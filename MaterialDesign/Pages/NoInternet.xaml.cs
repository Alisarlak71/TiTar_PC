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
using MahApps.Metro;
namespace MaterialDesign2.Pages
{
    /// <summary>
    /// Interaction logic for NoInternet.xaml
    /// </summary>
    public partial class NoInternet : Page
    {
        public NoInternet()
        {
            InitializeComponent();
            this.KeepAlive = false;
        }
        private void refresh_connection(object sender, RoutedEventArgs e)
        {
            
          
                        if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                            {
                                
                               // (window as MaterialDesign.MainWindow).reload();
                                if (this.NavigationService.CanGoBack)
                                {
                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                    this.NavigationService.GoBack(); 
                                }
                                else
                                {
                                    /// navigate to home page at startup
                                    if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true)
                                    {
                                        //
                                        if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.Loged_in == true)
                                        {
                                            MaterialDesign1.Pages.HomePage homepage = new MaterialDesign1.Pages.HomePage();
                                            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate(homepage);
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
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                                    MaterialDesign1.Pages.HomePage homepage = new MaterialDesign1.Pages.HomePage();
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate(homepage);
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).LoginTreeItem.Visibility = Visibility.Collapsed;
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).LogoutTreeItem.Visibility = Visibility.Visible;
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).AccountChannelsTreeItem.Visibility = Visibility.Visible;
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).AccountContentTreeItem.Visibility = Visibility.Visible;
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).AccountFavoriteTreeItem.Visibility = Visibility.Visible;
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).AccountSettingTreeItem.Visibility = Visibility.Visible;
                                                }
                                                else
                                                {
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                                    MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                                                    (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate(login_page);
                                                }
                                            }
                                            else
                                            {
                                                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                                MaterialDesign2.Pages.Login login_page = new MaterialDesign2.Pages.Login();
                                                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate(login_page);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                                         MaterialDesign2.Pages.NoInternet No_Internet = new MaterialDesign2.Pages.NoInternet();
                                        (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate(No_Internet);
                                    }
                                }
                            }
             
          
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
                (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
             
        }
        
    }
}
