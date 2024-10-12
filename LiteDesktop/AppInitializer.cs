using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LiteDesktop
{
    public class AppInitializer
    {
        public static void Initialize(Application app)
        {
            ResourceDictionary dict = new()
            {
                Source = new Uri("pack://application:,,,/Icons/SvgIcons.xaml")
            };
            app.Resources.MergedDictionaries.Add(dict);
        }
    }
}
