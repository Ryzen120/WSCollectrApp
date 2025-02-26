using System;
using System.Windows.Forms;
using System.IO;

namespace TradingCardManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if database file exists
            string dbPath = Path.Combine(Application.StartupPath, "WSCards.db");
            if (!File.Exists(dbPath))
            {
                MessageBox.Show("Database file 'WSCards.db' not found. Please make sure it's in the application directory.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new MainForm());
        }
    }
}