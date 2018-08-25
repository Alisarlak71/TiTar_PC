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
using MaterialDesignThemes;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Controls.Primitives;
namespace MaterialDesign2.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// 
    
    public partial class Login : UserControl
    {
        
        public Login()
        {
            InitializeComponent();
        }
        private void LoginToAccount_Click(object sender, RoutedEventArgs e)
        {
            if(password.Password!="" && email.Text!="")
            {
                (MaterialDesign.App.Current as MaterialDesign.App).login_account.number_of_try = 0;
            (MaterialDesign.App.Current as MaterialDesign.App).login_account.Login_To_Account(email.Text,password.Password);

           // (MaterialDesign.App.Current as MaterialDesign.App).login_account.status = true;
        
            if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.status==true)
            {
               
                     (MaterialDesign.App.Current as MaterialDesign.App).check_registery.write_to_registery(email.Text,password.Password);
                     (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).LogoutTreeItem.Visibility = Visibility.Visible;
                     (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).AccountChannelsTreeItem.Visibility = Visibility.Visible;
                     (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).AccountContentTreeItem.Visibility = Visibility.Visible;
                     (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).AccountFavoriteTreeItem.Visibility = Visibility.Visible;
                     (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).AccountSettingTreeItem.Visibility = Visibility.Visible;
                     MaterialDesign1.Pages.HomePage homepage = new MaterialDesign1.Pages.HomePage();
                     (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                     (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).HomeFrame.Navigate(homepage);
                     (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).LoginTreeItem.Visibility = Visibility.Collapsed;
                     
                      var balloon = new MaterialDesign2.Pages.WelcomeNotification();
                      balloon.Width = 450;
                      balloon.Height = 150;
                      (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).tb.ShowCustomBalloon(balloon, PopupAnimation.Slide, 12000);
                    
            }
            else if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected==false)
            {
                Warninnglable.Content = "اتصال خود به اینترنت را بررسی کنید";
            }
            else
            {
                Warninnglable.Content = "نام کاربری یا پسورد اشتباه است";
            }

            }
            else
            {
                Warninnglable.Content = "ایمیل و پسورد خود را وارد کنید";
            }
        }

        private void RegisterInTitar_Click(object sender, RoutedEventArgs e)
        {
            if (r_ConfirmPassword.Password != "" && EmailTexbox.Text !=""&& NameTextBox.Text!="" && r_Password.Password!="")
            {
                if(r_ConfirmPassword.Password==r_Password.Password)
                {
                    (MaterialDesign.App.Current as MaterialDesign.App).login_account.number_of_try = 0;
                    (MaterialDesign.App.Current as MaterialDesign.App).login_account.Register_In_Titar(NameTextBox.Text,EmailTexbox.Text,r_Password.Password);

                // (MaterialDesign.App.Current as MaterialDesign.App).login_account.status = true;

                if ((MaterialDesign.App.Current as MaterialDesign.App).login_account.status == true)
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(MaterialDesign.MainWindow))
                        {
                            (MaterialDesign.App.Current as MaterialDesign.App).check_registery.write_to_registery(EmailTexbox.Text,r_Password.Password);
                            (window as MaterialDesign.MainWindow).LogoutTreeItem.Visibility = Visibility.Visible;
                            (window as MaterialDesign.MainWindow).AccountChannelsTreeItem.Visibility = Visibility.Visible;
                            (window as MaterialDesign.MainWindow).AccountContentTreeItem.Visibility = Visibility.Visible;
                            (window as MaterialDesign.MainWindow).AccountSettingTreeItem.Visibility = Visibility.Visible;
                            (window as MaterialDesign.MainWindow).AccountFavoriteTreeItem.Visibility = Visibility.Visible;
                            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();
                            MaterialDesign1.Pages.HomePage homepage = new MaterialDesign1.Pages.HomePage();
                            (window as MaterialDesign.MainWindow).HomeFrame.Navigate(homepage); (window as MaterialDesign.MainWindow).LoginTreeItem.Visibility = Visibility.Collapsed;
                            var balloon = new MaterialDesign2.Pages.WelcomeNotification();
                            balloon.Width = 450;
                            balloon.Height = 150;
                            (window as MaterialDesign.MainWindow).tb.ShowCustomBalloon(balloon, PopupAnimation.Slide, 12000);
                        }

                    }
                }
                else if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected == false)
                {
                    Warninnglable2.Content = "اتصال خود به اینترنت را بررسی کنید";
                }
                else
                {
                    Warninnglable2.Content = "نام کاربری یا ایمیل تکراری است";
                }
              }
                else{
                    Warninnglable2.Content = "تکرار پسورد با پسورد وارد شده یکی نیست";
                }
            }
            else
            {
                Warninnglable2.Content = "ایمیل و پسورد خود را وارد کنید";
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            (MaterialDesign.App.Current.MainWindow as MaterialDesign.MainWindow).StartStopWait();

        }

    }
}
