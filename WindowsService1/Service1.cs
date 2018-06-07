using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        
        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            //System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStart.txt");
            string[] log = { "Starting server at "+ DateTime.Now.ToString("h:mm:ss tt") };
            System.IO.File.WriteAllLines((AppDomain.CurrentDomain.BaseDirectory + "ExecutionLog.txt"), log);

            string output = ExecuteSetup();
     
            System.IO.File.AppendAllLines((AppDomain.CurrentDomain.BaseDirectory + "ExecutionLog.txt"),new []{ output});
        }

        protected override void OnStop()
        {
            string[] log = { "Stoping server at " + DateTime.Now.ToString("h:mm:ss tt") };
            System.IO.File.AppendAllLines((AppDomain.CurrentDomain.BaseDirectory + "ExecutionLog.txt"),log);
        }

        private string ExecuteSetup()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = ConfigurationManager.AppSettings["compiler"]; 
            start.Arguments = ConfigurationManager.AppSettings["script"];   
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }
    }
}
