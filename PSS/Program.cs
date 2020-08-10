using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace PSS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        // Mutex can be made static so that GC doesn't recycle
        // same effect with GC.KeepAlive(mutex) at the end of main

        static Mutex mutex = new Mutex(false, "PSS2018");
        public static MDIPSS mdi;
        [STAThread]
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LogIn());

            // if you like to wait a few seconds in case that the instance is just 
            // shutting down
            if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                MessageBox.Show("PTS is already running on this machine!", "", MessageBoxButtons.OK);
                return;
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (LogIn login = new LogIn())
                {
                    //Enable these 2 lines for Production Version, then compile, before deployment
                    //Disable these 2 lines for Development Version
                    //=====================================================
                    //LogIn.strUserID = args[0];
                    //LogIn.strPassword = args[1];
                    //=====================================================

                    //create new login form
                    login.ShowDialog(); //show i
                    if (login.DialogResult == DialogResult.Cancel)
                    {
                        login.Dispose();
                        Application.Exit();
                        return; // This exits your application
                    }
                }
                mdi = new MDIPSS();
                Application.Run(mdi);
            }
            finally { mutex.ReleaseMutex(); } // I find this more explicit
        }
    }
}
