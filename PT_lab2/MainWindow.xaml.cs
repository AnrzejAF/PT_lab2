using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IOPath = System.IO.Path;
using System.IO;


namespace PT_lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class FileExplorer : ViewModelBase
        {
            public  DirectoryInfoViewModel Root { get; set; }
            public void OpenRoot(string path)
            {
                Root = new DirectoryInfoViewModel();
                Root.Open(path);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            //makeTreeView(@"D:\temp");

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            //folderBrowserDialog.ShowDialog();
            //string path = folderBrowserDialog.SelectedPath;

            //makeTreeView(path);
            var dlg = new FolderBrowserDialog() { Description = "Select directory cdcdsto open" };
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = dlg.SelectedPath;
                var fileExplorer = new FileExplorer();
                fileExplorer.OpenRoot(path);
                DataContext = FoldersTreeView;
            }
        }

        private void FoldersTreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            item.ContextMenu = new ContextMenu();
            MenuItem createMenuItem = new MenuItem();
            createMenuItem.Header = "Create";
            createMenuItem.Click += CreateMenuItem_Click;
            MenuItem deleteMenuItem = new MenuItem();
            deleteMenuItem.Header = "Delete";
            deleteMenuItem.Click += DeleteMenuItem_Click;
            item.ContextMenu.Items.Add(createMenuItem);
            item.ContextMenu.Items.Add(deleteMenuItem);
        }

        private void FileTreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            item.ContextMenu = new ContextMenu();
            MenuItem deleteMenuItem = new MenuItem();
            deleteMenuItem.Header = "Delete";
            deleteMenuItem.Click += DeleteMenuItem_Click;
            MenuItem openMenuItem = new MenuItem();
            openMenuItem.Header = "Open";
            openMenuItem.Click += OpenFile_Click;
            item.ContextMenu.Items.Add(openMenuItem);
            item.ContextMenu.Items.Add(deleteMenuItem);
        }

        private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (FoldersTreeView.SelectedItem is TreeViewItem selectedItem)
            {
                var dialog = new Dialog();
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        string parentPath = selectedItem.Tag.ToString();
                        string newPath = IOPath.Combine(parentPath, dialog.CreatedName);

                        if (dialog.IsFile)
                        {
                            File.Create(newPath).Close();
                            File.SetAttributes(newPath, dialog.CreatedAttributes);
                        }
                        else
                        {
                            Directory.CreateDirectory(newPath);
                            File.SetAttributes(newPath, dialog.CreatedAttributes);
                        }
                            
                        var newItem = new TreeViewItem
                        {
                            Header = dialog.CreatedName,
                            Tag = newPath
                        };

                        selectedItem.Items.Add(newItem);
                        if (dialog.IsFile)
                        {
                            newItem.MouseRightButtonDown += FileTreeView_MouseRightButtonDown;
                            System.Windows.MessageBox.Show("Created file successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            newItem.MouseRightButtonDown += FoldersTreeView_MouseRightButtonDown;
                            System.Windows.MessageBox.Show("Created directory successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Error creating file or directory: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // delete selected item from treeView and from the file system
            if (FoldersTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is string path)
            {
                if (System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.Directory.Delete(path, true);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Error deleting directory: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }   
                }
                else if (System.IO.File.Exists(path))
                {
                   try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Error deleting file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }   
                }
                if (selectedItem.Parent is TreeViewItem parentItem)
                {
                    parentItem.Items.Remove(selectedItem);
                }
                else
                {
                    FoldersTreeView.Items.Remove(selectedItem);
                }
                selectedItem.Items.Clear();
                FoldersTreeView.Items.Remove(selectedItem);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FoldersTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is string filePath)
                {
                    FileContent.Text = "";
                    using (var textReader = System.IO.File.OpenText(filePath))
                    {
                        FileContent.Text = "";
                        string text = textReader.ReadToEnd();
                        FileContent.Text = text;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

        }

        private void makeTreeView(string path)
        {
            FoldersTreeView.Items.Clear();
            AddDirectoryItems(path, FoldersTreeView.Items);
        }

        private void AddDirectoryItems(string directoryPath, ItemCollection items)
        {
            if (!System.IO.Directory.Exists(directoryPath))
            {
                return;
            }

            // Add directories
            string[] directories = System.IO.Directory.GetDirectories(directoryPath);
            foreach (string directory in directories)
            {
                TreeViewItem directoryItem = new TreeViewItem();
                directoryItem.Header = System.IO.Path.GetFileName(directory);
                directoryItem.Tag = directory;
                directoryItem.MouseRightButtonDown += FoldersTreeView_MouseRightButtonDown;
                items.Add(directoryItem);

                // Recursively add subdirectories and files
                AddDirectoryItems(directory, directoryItem.Items);
            }

            // Add files
            string[] files = System.IO.Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                TreeViewItem fileItem = new TreeViewItem();
                fileItem.Header = System.IO.Path.GetFileName(file);
                fileItem.Tag = file;
                fileItem.MouseRightButtonDown += FileTreeView_MouseRightButtonDown;
                items.Add(fileItem);
            }
        }

        private void FoldersTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                String path = "";
                if (FoldersTreeView.SelectedItem is TreeViewItem selectedItem)
                {
                    path = selectedItem.Tag.ToString();
                }
                else
                {
                    return;
                }

                FileAttributes attributes = File.GetAttributes(path);

                string attributesString = "";

                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    attributesString += "r";
                else
                    attributesString += "-";

                if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
                    attributesString += "a";
                else
                    attributesString += "-";

                if ((attributes & FileAttributes.System) == FileAttributes.System)
                    attributesString += "s";
                else
                    attributesString += "-";

                if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    attributesString += "h";
                else
                    attributesString += "-";

                attributesTextBlock.Text = attributesString;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error occurs: {ex.Message}");
            }
        }
    }
}
