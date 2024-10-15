using CommunityToolkit.Mvvm.ComponentModel;
using LiteDesktop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiteDesktop.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Fields
        private const string _defauleBackgroundImage = @"Image\Background\background.jpg";

        private const string _desktopPathKey = "DesktopPath";
        #endregion

        #region Properties
        private Uri? _BackgroundImage;
        /// <summary>
        /// 背景图片
        /// </summary>
        public Uri? BackgroundImage
        {
            get => _BackgroundImage;
            set => SetProperty(ref _BackgroundImage, value);
        }

        private List<LiteFileInfo> _LiteFileInfos;

        public List<LiteFileInfo> LiteFileInfos
        {
            get => _LiteFileInfos;
            set => SetProperty(ref _LiteFileInfos, value);
        }

        #endregion

        public MainWindowViewModel()
        {
            SetUserBackgroundImage();

            GetLiteFileInfos();
        }

        /// <summary>
        /// 设置用户背景图片
        /// </summary>
        private void SetUserBackgroundImage()
        {
            BackgroundImage = Utils.WindowsApi.User32Extension.GetDesktopWallpaper(out string wallPaperPath)
                ? new Uri(wallPaperPath, UriKind.RelativeOrAbsolute)
                : new(Path.Combine(Environment.CurrentDirectory, _defauleBackgroundImage), UriKind.RelativeOrAbsolute);
        }

        private void GetLiteFileInfos()
        {
            string customDeskPath = Utils.Helpers.AppConfigHelper.GetAppSettingsValue(_desktopPathKey);

            LiteFileInfos = GetLiteFileInfosByPath(customDeskPath);
        }

        private List<LiteFileInfo> GetLiteFileInfosByPath(string orderPath)
        {
            var fileInfos = new List<LiteFileInfo>();
            var files = Directory.GetFiles(orderPath);
            foreach (var item in files)
            {
                fileInfos.Add(GetLiteFileInfo(item));
            }
            var directories = Directory.GetDirectories(orderPath);
            foreach (var item in directories)
            {
                fileInfos.Add(GetLiteFileInfo(item));
            }
            return fileInfos;
        }

        private LiteFileInfo GetLiteFileInfo(string path)
        {
            FileInfo fileInfo = new(path);
            return new LiteFileInfo()
            {
                Icon = GetIconFromFilePath(path),
                FilePath = path,
                Name = fileInfo.Name,
                Extension = fileInfo.Extension,
                CreateTime = fileInfo.CreationTime,
            };
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr ExtractIcon(IntPtr hInst, string lpIconFile, int nIconIndex);

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr hIcon);

        public static BitmapSource GetIconFromFilePath(string filePath)
        {
            // 从文件路径获取图标
            return GetIcon(filePath, 0);
        }

        public static BitmapSource GetIconFromFileExtension(string fileExtension)
        {
            // 从文件扩展名获取图标
            return GetIcon(fileExtension, -1);
        }

        private static BitmapSource GetIcon(string path, int index)
        {
            IntPtr hIcon = ExtractIcon(IntPtr.Zero, path, index);

            if (hIcon == IntPtr.Zero)
            {
                throw new Exception("无法提取图标");
            }

            try
            {
                using Icon icon = Icon.FromHandle(hIcon);
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty,BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DestroyIcon(hIcon);
            }
        }
    }
}
