using System.Configuration;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Controls.Entity;
using Controls.Tool;
using NLog;
using Util;

namespace FormOpenSocket
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GlobalVar.logger.Info("³ÌÐòÆô¶¯");
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Console.WriteLine("Unhandled Exception: " + e.ExceptionObject.ToString()); };
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}