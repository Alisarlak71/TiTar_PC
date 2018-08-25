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

        protected override void OnStartup(StartupEventArgs e)
        {
        
        
    }
    }
    
}
