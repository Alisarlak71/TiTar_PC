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

namespace MaterialDesign2.Pages
{
    /// <summary>
    /// Interaction logic for WelcomeNotification.xaml
    /// </summary>
    public partial class WelcomeNotification : UserControl
    {
        public WelcomeNotification()
        {
            InitializeComponent();
           
        }

        private void loadwelcome(object sender, RoutedEventArgs e)
        {

            UserName.Text = (MaterialDesign.App.Current as MaterialDesign.App).User.name.ToString();
            
        }
    }
}
