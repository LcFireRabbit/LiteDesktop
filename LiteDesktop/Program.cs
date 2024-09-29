using LiteDesktop.ViewModels;
using LiteDesktop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LiteDesktop
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application app = new();
                MainWindowViewModel mainWindowViewModel = new();
                MainWindow mainWindow = new()
                {
                    DataContext = mainWindowViewModel
                };
                app.Run(mainWindow);
                app.Shutdown();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
