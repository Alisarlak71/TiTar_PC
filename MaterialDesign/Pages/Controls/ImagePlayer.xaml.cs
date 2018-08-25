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
using System.Windows.Shapes;
using Image360Player.View;
using Video360Player.View;
using MaterialDesignThemes.Wpf;
using MahApps.Metro;
namespace MaterialDesign2.Pages.Controls
{
    /// <summary>
    /// Interaction logic for ImagePlayer.xaml
    /// </summary>
    public partial class ImagePlayer : MahApps.Metro.Controls.MetroWindow
    {
        public string url;
        public string typeContent;
        public bool IsFullscreen { get; private set; }
        public bool IsLoading { get; private set; }
        public ImagePlayer()
        {
            InitializeComponent();
            
        }
        public void  Openimage(string path)
        {
            PanoView p= new PanoView();
            Card c = new Card();
            c.Width = 50;
            c.Height = 50;
            c.HorizontalAlignment = HorizontalAlignment.Right;
            c.VerticalAlignment = VerticalAlignment.Bottom;
            c.Background = Brushes.Transparent;
            c.MouseDown += FullScreen;

            PackIcon fullscreen = new PackIcon();
            fullscreen.Kind = MaterialDesignThemes.Wpf.PackIconKind.Fullscreen;
            fullscreen.HorizontalAlignment = HorizontalAlignment.Right;
            fullscreen.VerticalAlignment = VerticalAlignment.Bottom;
            fullscreen.Width = 50;
            fullscreen.Height = 50;
            
            c.Content = fullscreen;

            grid1.Children.Add(p);
            grid1.Children.Add(c);

            IsLoading = true;
            p.Image = new BitmapImage();
            p.Image.BeginInit();
            p.Image.CacheOption = BitmapCacheOption.OnLoad;
            p.Image.UriSource = new Uri(path);
            p.Image.EndInit();
            IsLoading = false; 
        }
        public void Openvideo(string path)
        {
            VideoView p = new VideoView();
            Card c = new Card();
            c.Width = 50;
            c.Height = 50;
            c.HorizontalAlignment = HorizontalAlignment.Right;
            c.VerticalAlignment = VerticalAlignment.Bottom;
            c.Background = Brushes.Transparent;
            c.MouseDown += FullScreen;

            PackIcon fullscreen = new PackIcon();
            fullscreen.Kind = MaterialDesignThemes.Wpf.PackIconKind.Fullscreen;
            fullscreen.HorizontalAlignment = HorizontalAlignment.Right;
            fullscreen.VerticalAlignment = VerticalAlignment.Bottom;
            fullscreen.Width = 50;
            fullscreen.Height = 50;

            c.Content = fullscreen;

            grid1.Children.Add(p);
            grid1.Children.Add(c);

            IsLoading = true;
            MediaElement m = new MediaElement();
           // m.Play();
            p.Image = m;
            
            p.Image.BeginInit();
            p.Image.Source = new Uri(path);
            p.Image.EndInit();
            IsLoading = false;
        }

        // Toggle fullscreen
        private void FullScreen(object sender, RoutedEventArgs e)
        {
            if (IsFullscreen == false)
            {
                this.ShowTitleBar = false;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.ShowTitleBar = true;
                this.WindowStyle = WindowStyle.ThreeDBorderWindow;
                this.WindowState = System.Windows.WindowState.Normal;
            }
            IsFullscreen = !IsFullscreen;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IsFullscreen = false;
            IsLoading = false;
            if (typeContent=="image")
                 Openimage(url);
            else if (typeContent=="video")
            {
                Openvideo(url);
            }
            
        }
    }
}
