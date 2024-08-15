using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LiteDesktop.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private Uri _BackgroundImage;
        /// <summary>
        /// 背景图片
        /// </summary>
        public Uri BackgroundImage
        {
            get => _BackgroundImage;
            set => SetProperty(ref _BackgroundImage, value);
        }


        public MainWindowViewModel()
        {
            if (Utils.WindowsApi.User32Extension.GetDesktopWallpaper(out string wallPaperPath))
            {
                BackgroundImage = new Uri(wallPaperPath, UriKind.RelativeOrAbsolute);
            }
            else
            {
                string defaultWallpaperPath = Environment.CurrentDirectory;
                BackgroundImage = new Uri(defaultWallpaperPath, UriKind.RelativeOrAbsolute);
            }
        }
    }
}
