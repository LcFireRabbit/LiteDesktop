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
using static Utils.WindowsApi.Shell32Base;

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
            LiteFileInfos = GetLiteFileInfosByPath(GetCustomDeskPath());
        }

        private string GetCustomDeskPath()
        {
            string customDeskPath = Utils.Helpers.AppConfigHelper.GetAppSettingsValue(_desktopPathKey);
            if (string.IsNullOrEmpty(customDeskPath) || !Directory.Exists(customDeskPath))
            {
                customDeskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            return customDeskPath;
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

            var icon = System.Drawing.Icon.ExtractAssociatedIcon(path);

            return new LiteFileInfo()
            {
                Icon = ConvertIconToImageSource(GetFileIcon(path)),
                FilePath = path,
                Name = fileInfo.Name,
                Extension = fileInfo.Extension,
                CreateTime = fileInfo.CreationTime,
            };
        }

        // 定义结构体
        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        // 声明API函数
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbSizeFileInfo,
            uint uFlags);

        // 调用 Win32 API 释放 HICON
        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr handle);

        // 标志位定义
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;    // 大图标（32x32）
        private const uint SHGFI_SMALLICON = 0x1;    // 小图标（16x16）
        private const uint SHGFI_SYSICONINDEX = 0x4000; // 系统图标索引

        public static Icon GetFileIcon(string filePath, bool isLargeIcon = true)
        {
            if (!File.Exists(filePath) && !Directory.Exists(filePath))
                throw new FileNotFoundException("文件或文件夹不存在。");

            SHFILEINFO shinfo = new SHFILEINFO();
            uint flags = SHGFI_ICON | (isLargeIcon ? SHGFI_LARGEICON : SHGFI_SMALLICON);

            IntPtr handle = SHGetFileInfo(
                filePath,
                0,
                ref shinfo,
                (uint)Marshal.SizeOf(shinfo),
                flags);

            if (handle != IntPtr.Zero)
            {
                return Icon.FromHandle(shinfo.hIcon);
            }
            return null;
        }

        public ImageSource ConvertIconToImageSource(Icon icon)
        {
            if (icon == null)
                return null;

            // 获取图标句柄（HICON）
            IntPtr hicon = icon.Handle;

            // 创建 BitmapSource
            var bitmap = Imaging.CreateBitmapSourceFromHIcon(
                hicon,
                new Int32Rect(0, 0, icon.Width, icon.Height), // 源矩形（整个图标）
                BitmapSizeOptions.FromEmptyOptions() // 或其他选项
            );

            // 冻结对象以优化性能（可选）
            if (bitmap != null)
                bitmap.Freeze();

            // 释放 HICON 资源（重要！）
            DestroyIcon(hicon);

            return bitmap;
        }
    }
}
