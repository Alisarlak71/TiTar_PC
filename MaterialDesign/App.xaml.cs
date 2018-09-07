using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
namespace MaterialDesign
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string DeptName { get; set; }
        public MaterialDesign2.Classes.InternetConnect Internet_connect = new MaterialDesign2.Classes.InternetConnect(); 

        /// <summary>
        /// Recive Content from server
        /// </summary>
        public MaterialDesign2.Classes.ReciveContent Recive_Content = new MaterialDesign2.Classes.ReciveContent();

        public List <MaterialDesign2.Classes.Content>LasteContent = new List<MaterialDesign2.Classes.Content>();
        public List<MaterialDesign2.Classes.Content> MostSeen = new List<MaterialDesign2.Classes.Content>();
        public List<MaterialDesign2.Classes.Content> MostLove = new List<MaterialDesign2.Classes.Content>();
        public List<MaterialDesign2.Classes.Content> MostDown = new List<MaterialDesign2.Classes.Content>();
        public List<MaterialDesign2.Classes.Content> MostSell = new List<MaterialDesign2.Classes.Content>();
        //Searching 
        public List<MaterialDesign2.Classes.Content> SearchResult = new List<MaterialDesign2.Classes.Content>();

        /// <summary>
        /// log in 
        /// </summary>
        public MaterialDesign2.Classes.registerysetting check_registery = new MaterialDesign2.Classes.registerysetting();
        public MaterialDesign2.Classes.Login login_account = new MaterialDesign2.Classes.Login();
        public MaterialDesign2.Classes.User User = new MaterialDesign2.Classes.User();
        /// <summary>
        /// Comments
        /// </summary>
        public MaterialDesign2.Classes.Comment Get_comment = new MaterialDesign2.Classes.Comment();

        public MaterialDesign2.Pages.Management.CreateVideo CreateVideoPage=new MaterialDesign2.Pages.Management.CreateVideo();

        public string CustomerCode { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            if(e.Args.Length==1)
            CustomerCode = e.Args[0].ToString();
           // base.OnStartup(e);
            MessageBox.Show(CustomerCode);
        }
        public void register_app_Location()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Titar.exe";
            Microsoft.Win32.RegistryKey Software = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE");
            Microsoft.Win32.RegistryKey Classes = Software.OpenSubKey("Classes");

            Microsoft.Win32.RegistryKey Titar = Classes.CreateSubKey("Titar");
            Titar.SetValue("", "URL:Custom Protocol");
            Titar.SetValue("URL Protocol", "");

            Microsoft.Win32.RegistryKey DefaultIcon = Titar.CreateSubKey("DefaultIcon");
            DefaultIcon.SetValue("", "\"" + path +", 0\"");
            Microsoft.Win32.RegistryKey shell = Titar.CreateSubKey("shell");
            Microsoft.Win32.RegistryKey open = shell.CreateSubKey("open");
            Microsoft.Win32.RegistryKey command = open.CreateSubKey("command");
            command.SetValue("", "\""+path+"\""+" \"%1\"");
            Titar.Close();
        }
    }
    
}
