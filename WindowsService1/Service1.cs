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
using System.Net;


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

        // On Start of the Service
        protected override void OnStart(string[] args)
        {
            log("Starting Server On Start");
            ExecuteSetup();
            log("Execution stopped");
        }

        // On Stop of the Service
        protected override void OnStop()
        {
            log("On Stop method executing");
            ExecuteSetup();
            log("On Stop Execution is closing");
        }

        
        // Execution of the script
        private void ExecuteSetup()
        {
            log("Starting python execution");
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = ConfigurationManager.AppSettings["compiler"]; 
            start.Arguments = ConfigurationManager.AppSettings["script"];   
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;

            try
            {
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        log(result);
                    }
                }
            }

            catch(Exception exp)
            {
                Debug.Write(exp);
                log(exp.ToString());
            }
            
        }

        public void log(string logging)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "ExecutionLog.txt";
            string[] log = { DateTime.Now.ToString("h:mm:ss tt") +" "+logging };
            File.AppendAllLines(file, log);
        }

    }
}
