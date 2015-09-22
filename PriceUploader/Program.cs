using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceUploader
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

            FormSetDatabase formSetDatabase = new FormSetDatabase();
            PriceModel pm = new PriceModel();
            formSetDatabase.Init(ref pm);
            formSetDatabase.StartPosition = FormStartPosition.CenterScreen;
            formSetDatabase.ShowDialog();
            pm = null;

            Application.Run(new PriceUploader());
        }
    }
}
