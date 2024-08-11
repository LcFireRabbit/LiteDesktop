using LiteDesktop.ViewModels;
using LiteDesktop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDesktop
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            App app = new();
            app.InitializeComponent();
            MainWindowViewModel mainWindowViewModel = new();
            MainWindow mainWindow = new()
            {
                DataContext = mainWindowViewModel
            };
            app.MainWindow = mainWindow;
            app.Run();
        }
    }
}
