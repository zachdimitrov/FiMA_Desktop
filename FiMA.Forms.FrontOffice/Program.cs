using Ninject;
using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace FiMA.Forms.FrontOffice
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var kernel = new StandardKernel();
            kernel.Load(new FormsModule());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(kernel.Get<MasterFrontOffice>());
        }
    }
}
