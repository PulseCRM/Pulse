using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PulseInstallation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new Form2());
        }


    }
    public class Hello
    {
        public string InstallFolder { get; set; }
        public string SQLServer { get; set; }
        public string PulseDBLogin { get; set; }
        public string PulseDBPassword { get; set; }
        public string PulseDBName { get; set; }
        public string ServiceHost { get; set; }
        public string ServicePortNumber { get; set; }
        public decimal ADImportInterval { get; set; }
        public string AdminPassword { get; set; }
        public string ADAdminLogin { get; set; }
        public string Domain { get; set; }
        public string ADServerHost { get; set; }
        public decimal ScheduledImportInterval { get; set; }
        public string WinPointIni { get; set; }
        public string PointCentralDBName { get; set; }
        public string PointCentralDBPassword { get; set; }
        public string PointCentralDBLogin { get; set; }
        public string PontCentralSQLServer { get; set; }
        public string CardexFile { get; set; }
        public bool PointCentralEnabled { get; set; }
    }

}
