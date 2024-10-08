using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        #region Fields
        private const string _defauleBackgroundImage = @"Image\Background\background.jpg";
        #endregion

        #region Properties
        private Uri _BackgroundImage = new(Path.Combine(Environment.CurrentDirectory, _defauleBackgroundImage)
            , UriKind.RelativeOrAbsolute);
        /// <summary>
        /// 背景图片
        /// </summary>
        public Uri BackgroundImage
        {
            get => _BackgroundImage;
            set => SetProperty(ref _BackgroundImage, value);
        }
        #endregion

        public MainWindowViewModel()
        {
            SetUserBackgroundImage();

            string customDeskPath = Utils.Helpers.AppConfigHelper.GetAppSettingsValue("DesktopPath");

            var diectorys = System.IO.Directory.GetFiles(customDeskPath);
        }

        /// <summary>
        /// 设置用户背景图片
        /// </summary>
        private void SetUserBackgroundImage()
        {
            if (Utils.WindowsApi.User32Extension.GetDesktopWallpaper(out string wallPaperPath))
            {
                _BackgroundImage = new Uri(wallPaperPath, UriKind.RelativeOrAbsolute);
            }
        }
    }
}
