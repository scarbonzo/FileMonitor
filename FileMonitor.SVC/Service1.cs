using FileMonitor.SVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;

namespace FileMonitor.SVC
{
    public partial class Service1 : ServiceBase
    {
        //Service Timer Info
        private static System.Timers.Timer m_mainTimer;
        private static int interval = 86400 * 1000; //How often to run in milliseconds (seconds * 1000)
        private static string ServerName = Environment.MachineName;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Create the Main timer
            m_mainTimer = new System.Timers.Timer
            {
                //Set the timer interval
                Interval = interval
            };
            
            //Dictate what to do when the event fires
            m_mainTimer.Elapsed += m_mainTimer_Elapsed;
            
            //Something to do with something, I forgot since it's been a while
            m_mainTimer.AutoReset = true;

#if DEBUG
#else
            m_mainTimer.Start(); //Start timer only in Release
#endif
            //Run 1st Tick Manually
            Console.Beep();
            Routine();
        }

        protected override void OnStop()
        {
        }

        public void OnDebug()
        {
            //Manually kick off the service when debugging
            OnStart(null);
        }

        void m_mainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Routine();
        }

        private void Routine()
        {
            //TODO: Use a table to store paths and call them here, ideally each servername would map to a set of locations
            var files = ScanFolder(@"C:\temp\ssl");
            
            //TODO: Create an entity hook to write the file list to. 
        }

        private static List<file> ScanFolder(string FolderPath)
        {
            var directories = new List<DirectoryInfo>();
            var files = new List<file>();

            try
            {
                directories = new DirectoryInfo(FolderPath).EnumerateDirectories().ToList();

                foreach (var directory in directories)
                {
                    files.AddRange(ScanFolder(directory.FullName.ToString()));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            try
            {
                var fileinfos = new DirectoryInfo(FolderPath).EnumerateFiles().ToList();

                foreach (var fileinfo in fileinfos)
                {
                    files.Add(new file(ServerName, fileinfo));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return files;
        }
    }
}
