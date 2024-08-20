using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
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
        private Uri _BackgroundImage = new(Path.Combine(Environment.CurrentDirectory, @"Image\Background\background.jpg"), UriKind.RelativeOrAbsolute);
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
            SetUserBackgroundImage();
        }

        private void SetUserBackgroundImage()
        {
            if (Utils.WindowsApi.User32Extension.GetDesktopWallpaper(out string wallPaperPath))
            {
                _BackgroundImage = new Uri(wallPaperPath, UriKind.RelativeOrAbsolute);
            }
        }
    }
}
