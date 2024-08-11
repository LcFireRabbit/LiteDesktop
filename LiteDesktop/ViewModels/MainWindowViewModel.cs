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
        private string _BackgroundImage = string.Empty;
        /// <summary>
        /// 背景图片
        /// </summary>
        public string BackgroundImage
        {
            get => _BackgroundImage;
            set => SetProperty(ref _BackgroundImage, value);
        }


        public MainWindowViewModel()
        {
            GetDesktopBackgroundImage();
        }

        
        unsafe void GetDesktopBackgroundImage()
        {
            FixedSizeCharStruct wallPaperPath = new FixedSizeCharStruct();
            bool isSucc = PInvoke.SystemParametersInfo(Windows.Win32.UI.WindowsAndMessaging.SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETDESKWALLPAPER
                    , 200
                    , &wallPaperPath
                    , 0);
            BackgroundImage = BitConverter.ToString(wallPaperPath.Name);
        }

        public struct FixedSizeCharStruct
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public byte[]? Name;

            public FixedSizeCharStruct()
            {

            }
        }
    }
}
