using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PT_lab2
{
    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        public Exception? Exception { get; private set; }

        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; }
            = new ObservableCollection<FileSystemInfoViewModel>();

        public FileSystemWatcher? Watcher;

        public bool Open(string path)
        {
            bool result = false;
            try
            {
                AddWatcher(path);

                AddDirectoryItems(path, Items);
                result = true;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            return result;
        }

        public void AddWatcher(string path)
        {
            Watcher = new FileSystemWatcher(path);
            Watcher.Created += OnFileSystemChanged;
            Watcher.Renamed += OnFileSystemChanged;
            Watcher.Deleted += OnFileSystemChanged;
            Watcher.Changed += OnFileSystemChanged;
            Watcher.Error += OnWatcherError;
            Watcher.EnableRaisingEvents = true;
        }

        public void OnFileSystemChanged(object sender, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => OnFileSystemChanged(e));
        }

        private void OnFileSystemChanged(FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                if (e.Name != null)
                {
                    // if it is a directory add directory with items inside it
                    if (Directory.Exists(e.FullPath))
                    {
                        var dirInfo = new DirectoryInfo(e.FullPath);
                        DirectoryInfoViewModel directoryInfoViewModel = new DirectoryInfoViewModel();
                        directoryInfoViewModel.Model = dirInfo;
                        directoryInfoViewModel.Items = new ObservableCollection<FileSystemInfoViewModel>();

                        // Recursively add the directory's items to this new collection
                        AddDirectoryItems(e.FullPath, directoryInfoViewModel.Items);

                        // Finally, add the directory to the passed-in items collection
                        Items.Add(directoryInfoViewModel);

                        // add recursive call to add files
                        AddFileItems(e.FullPath, directoryInfoViewModel.Items);
                    }
                    else
                    {
                        var fileInfo = new FileInfo(e.FullPath);
                        FileInfoViewModel fileInfoViewModel = new FileInfoViewModel();
                        fileInfoViewModel.Model = fileInfo;
                        Items.Add(fileInfoViewModel);  
                    }
                }

            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                var item = Items.FirstOrDefault(i => i.Model.FullName == e.FullPath);
                if (item != null)
                {
                    Items.Remove(item);
                }
            }
            else if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                var item = Items.FirstOrDefault(i => i.Model.FullName == e.FullPath);
                if (item != null)
                {
                    Items.Remove(item);
                    var fileSystemInfo = new FileInfo(e.FullPath);
                    FileInfoViewModel fileInfoViewModel = new FileInfoViewModel();
                    fileInfoViewModel.Model = fileSystemInfo;
                    Items.Add(fileInfoViewModel);
                }
            }
        }

        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            // open a dialog box to show the error
            System.Windows.MessageBox.Show(e.GetException().Message);
        }

        private void AddDirectoryItems(string directoryPath, ObservableCollection<FileSystemInfoViewModel> items)
        {
            if (!System.IO.Directory.Exists(directoryPath))
            {
                return;
            }

            // Add directories
            string[] directories = System.IO.Directory.GetDirectories(directoryPath);
            foreach (string directory in directories)
            {
                var dirInfo = new DirectoryInfo(directory);
                DirectoryInfoViewModel directoryInfoViewModel = new DirectoryInfoViewModel();
                directoryInfoViewModel.Model = dirInfo;
                directoryInfoViewModel.Items = new ObservableCollection<FileSystemInfoViewModel>();

                // Recursively add the directory's items to this new collection
                AddDirectoryItems(directory, directoryInfoViewModel.Items);

                // Finally, add the directory to the passed-in items collection
                items.Add(directoryInfoViewModel);

                // add recursive call to add files
                AddFileItems(directory, directoryInfoViewModel.Items);
            }
            AddFileItems(directoryPath, items);
        }

        private void AddFileItems(string directoryPath, ObservableCollection<FileSystemInfoViewModel> items)
        {
            if (!System.IO.Directory.Exists(directoryPath))
            {
                return;
            }

            // Add files
            string[] files = System.IO.Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                var fileInfo = new FileInfo(file);
                FileInfoViewModel fileInfoViewModel = new FileInfoViewModel();
                fileInfoViewModel.Model = fileInfo;
                items.Add(fileInfoViewModel);
            }
        }
        
    }
}
