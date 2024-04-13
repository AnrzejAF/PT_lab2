using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PT_lab2
{
    public class FileSystemInfoViewModel : ViewModelBase
    {
        public String Caption { get; set; }

        public DateTime LastWriteTime
        {
            get { return _lastWriteTime; }
            set
            {
                if (_lastWriteTime != value)
                {
                    _lastWriteTime = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DateTime _lastWriteTime;

        // add icon property for image
        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    NotifyPropertyChanged(nameof(Icon));
                }
            }
        }
        private ImageSource _icon;

        private void LoadImage(FileSystemInfo fileInfo)
        {
            var fileType = Path.GetExtension(fileInfo.FullName);
            switch (fileType)
            {
                case ".txt":
                    Icon = new BitmapImage(new Uri("pack://application:,,,/icons/txt.png"));
                    break;
                case ".zip":
                    Icon = new BitmapImage(new Uri("pack://application:,,,/icons/zip.png"));
                    break;
                default:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/icons/folder.png"));
                    break;
            }
        }


        public FileSystemInfo Model
        {
            get { return _fileSystemInfo; }
            set
            {
                if (_fileSystemInfo != value)
                {
                    _fileSystemInfo = value;
                    this.LastWriteTime = value.LastWriteTime;
                    this.Caption = value.Name;
                    LoadImage(value);
                    NotifyPropertyChanged();
                }
            }
        }
        private FileSystemInfo? _fileSystemInfo;


    }
}
