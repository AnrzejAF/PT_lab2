using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PT_lab2
{
    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        public Exception? Exception { get; private set; }

        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; }
            = new ObservableCollection<FileSystemInfoViewModel>();

        public bool Open(string path)
        {
            bool result = false;
            try
            {
                AddDirectoryItems(path, Items);
                result = true;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            return result;
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
