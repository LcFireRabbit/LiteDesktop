using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiteDesktop.Models
{
    public class LiteFileInfo : ObservableObject
    {
        //icon
        private ImageSource? _Icon;

        public ImageSource? Icon
        {
            get => _Icon;
            set => SetProperty(ref _Icon, value);
        }

        //name
        private string? _Name;

        public string? Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        //url
        private string? _FilePath;

        public string? FilePath
        {
            get => _FilePath;
            set => SetProperty(ref _FilePath, value);
        }

        //extension
        private string? _Extension;

        public string? Extension
        {
            get => _Extension;
            set => SetProperty(ref _Extension, value);
        }

        //CreateTime
        private DateTime _CreateTime;

        public DateTime CreateTime
        {
            get => _CreateTime;
            set => SetProperty(ref _CreateTime, value);
        }
    }
}
