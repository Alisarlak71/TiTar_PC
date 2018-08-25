using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace MaterialDesign2.Classes
{
    public class AppGameStuff
    {
        public string Url;
        public string LocalPath="";
        public string FileName;
        public void Start()
        {
            DownloadFile(Url, LocalPath);
        }
        /// <summary>
        /// Downloaf From Server 
        /// </summary>
        System.Net.WebClient m_WebClient;
        private void DownloadFile(string URL, string fileName)
        {
            try
            {
                m_WebClient = new System.Net.WebClient();
                m_WebClient.DownloadProgressChanged += DownloadProgressChanged;
                m_WebClient.DownloadFileCompleted += DownloadFileCompleted;
                Uri uri = new Uri(URL);
                m_WebClient.DownloadFileAsync(uri, fileName);
            }
            catch
            {
                //MessageBox.Show("اتصال به اینترنت را بررسی کنید");
            }
        }
        /// <summary>
        /// After Download Install Setup
        /// </summary>
        /// <param name="path"></param>
        public void OpenInstall(string path)
        {
            try
            {
                string ApplicationPath = path;
                string ApplicationArguments = "/qr";
                // Create a new process object
                Process ProcessObj = new Process();

                ProcessObj.StartInfo.FileName = ApplicationPath;
                ProcessObj.StartInfo.Arguments = ApplicationArguments;
                ProcessObj.StartInfo.CreateNoWindow = true;
                // Start the process
                ProcessObj.Start();

                // Wait that the process exits
                ///// after install call to open and delete
                ProcessObj.WaitForExit();
                OpenProgram();
                DeleteSetup();

            }
            catch
            {
                // MessageBox.Show("can not open installation");
            }

}

        /// <summary>
        /// After Install Open Installed App/Game
        /// </summary>
        public void OpenProgram()
        {
            try
            {
                Process firstProc = new Process();
                string path = FindByDisplayName(Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall"), FileName) + FileName + ".exe";
                firstProc.StartInfo.FileName = path;
                firstProc.EnableRaisingEvents = true;
                firstProc.Start();
            }
            catch
            {
              //  MessageBox.Show("can not open program");
            }
        }


        private string FindByDisplayName(RegistryKey parentKey, string name)
        {
            string[] nameList = parentKey.GetSubKeyNames();
            for (int i = 0; i < nameList.Length; i++)
            {
                RegistryKey regKey = parentKey.OpenSubKey(nameList[i]);
                try
                {
                    if (regKey.GetValue("DisplayName").ToString() == name)
                    {
                        return regKey.GetValue("InstallLocation").ToString();
                    }
                }
                catch
                {

                }
            }
            return "";
        }

        /// <summary>
        ///  Delete Setup From System
        /// </summary>
        public void DeleteSetup()
        {

            // try
            {
                File.Delete(LocalPath);
            }
            // catch 
            {
                //     MessageBox.Show("can not delete");
            }
        }

        /// <summary>
        /// UI Stuff Downlaoding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            double percentage = e.ProgressPercentage;
            //Update the progress bar value with the percentage 0-100
          //  progressbar.Value = percentage;
          //  prgs.Content = (percentage).ToString();
          //  prgs2.Content = (progressbar.Value).ToString();

        }

        private void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            OpenInstall(LocalPath);
        }
    }
}
