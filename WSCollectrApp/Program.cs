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

            // Check if database file exists, but don't exit if not found

            Application.Run(new MainForm());
        }
    }
}