using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT_lab2
{
    public class FileSystemInfoViewModel : ViewModelBase
    {
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

        public DateTime LastAccessTime
        {
            get { return _lastAccessTime; }
            set
            {
                if (_lastAccessTime != value)
                {
                    _lastAccessTime = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DateTime _lastAccessTime;

        public FileSystemInfo Model
        {
            get { return _fileSystemInfo; }
            set
            {
                if (_fileSystemInfo != value)
                {
                    _fileSystemInfo = value;
                    this.LastWriteTime = value.LastWriteTime;
                    this.LastAccessTime = value.LastAccessTime;

                    NotifyPropertyChanged();
                }
            }
        }
        private FileSystemInfo ?_fileSystemInfo;
    }
}
