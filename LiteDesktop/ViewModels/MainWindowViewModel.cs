using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Windows.Win32;

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
            var imagePath = GetDesktopBackgroundImage();

            //"C:\\Users\\LL734\\AppData\\Roaming\\Tencent\\DeskGo\\1___6130381___0___.jpg"
            BackgroundImage = new Uri(imagePath, UriKind.RelativeOrAbsolute);
        }


        unsafe string GetDesktopBackgroundImage()
        {
            string path = string.Empty;

            IntPtr ptr = Marshal.AllocHGlobal(200);

            char* ptrChar = (char*)ptr;

            bool isSucc = PInvoke.SystemParametersInfo(Windows.Win32.UI.WindowsAndMessaging.SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETDESKWALLPAPER
                   , 200
                   , ptrChar
                   , 0);
            if (isSucc)
            {
                path = Marshal.PtrToStringUni(ptr);
            }
            // 释放非托管内存
            Marshal.FreeHGlobal(ptr);
            return path;
        }
    }
}
