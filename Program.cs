using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Herring
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Mutex m = null;
            if (!CheckUniqueInstance(out m))
            {
                if (m != null)
                    GC.KeepAlive(m);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static bool CheckUniqueInstance(out Mutex m)
        {
            string mutexName = "Herring-" + WindowsIdentity.GetCurrent().User.ToString();

            // check if application instance is already executed
            bool createdNew;
            m = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show(
                    "Only one instance of Herring Activity Tracker is allowed at a time.",
                    "Herring Activity Tracker");
                return false;
            }
            return true;
        }
    }
}
