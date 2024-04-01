using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using IOPath = System.IO.Path;

namespace PT_lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            makeTreeView(@"D:\temp");

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // create open browse path dialog and add them to TreeViewItem
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            
            makeTreeView(path);
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
            MenuItem createMenuItem = new MenuItem();
            createMenuItem.Header = "Create";
            createMenuItem.Click += CreateMenuItem_Click;
            MenuItem deleteMenuItem = new MenuItem();
            deleteMenuItem.Header = "Delete";
            deleteMenuItem.Click += DeleteMenuItem_Click;
            MenuItem openMenuItem = new MenuItem();
            openMenuItem.Header = "Open";
            openMenuItem.Click += OpenFile_Click;
            item.ContextMenu.Items.Add(openMenuItem);
            item.ContextMenu.Items.Add(createMenuItem);
            item.ContextMenu.Items.Add(deleteMenuItem);
        }

        private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // delete selected item from treeView and from the file system
            if (FoldersTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is string path)
            {
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.Delete(path, true);
                }
                else if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
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
                    using (var textReader = System.IO.File.OpenText(filePath))
                    {
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

        private void makeTreeView(string path) // TODO
        {
            FoldersTreeView.Items.Clear();

            if (System.IO.Directory.Exists(path))
            {
                string[] directories = System.IO.Directory.GetDirectories(path);
                string[] files = System.IO.Directory.GetFiles(path);

                foreach (string directory in directories)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = IOPath.GetFileName(directory);
                    item.Tag = directory;
                    item.MouseRightButtonDown += FoldersTreeView_MouseRightButtonDown;
                    FoldersTreeView.Items.Add(item);
                    // add all files in the directory
                    string[] subFiles = System.IO.Directory.GetFiles(directory);
                    foreach (string subFile in subFiles)
                    {
                        TreeViewItem subItem = new TreeViewItem();
                        subItem.Header = IOPath.GetFileName(subFile);
                        subItem.Tag = subFile;
                        subItem.MouseRightButtonDown += FileTreeView_MouseRightButtonDown;
                        item.Items.Add(subItem);
                    }
                    // add also subdirectories and files
                    string[] subDirectories = System.IO.Directory.GetDirectories(directory);
                    foreach (string subDirectory in subDirectories)
                    {
                        TreeViewItem subItem = new TreeViewItem();
                        subItem.Header = IOPath.GetFileName(subDirectory);
                        subItem.Tag = subDirectory;
                        subItem.MouseRightButtonDown += FoldersTreeView_MouseRightButtonDown;
                        item.Items.Add(subItem);
                        // add all files in the subdirectory
                        string[] subSubFiles = System.IO.Directory.GetFiles(subDirectory);
                        foreach (string subFile in subSubFiles)
                        {
                            TreeViewItem subSubItem = new TreeViewItem();
                            subSubItem.Header = IOPath.GetFileName(subFile);
                            subSubItem.Tag = subFile;
                            subSubItem.MouseRightButtonDown += FileTreeView_MouseRightButtonDown;
                            subItem.Items.Add(subSubItem);
                        }
                    }
                }
            }   
        }

        private void addTreeViewItem(TreeViewItem item, string path)
        {
            item.Header = IOPath.GetFileName(path);
            item.Tag = path;
            item.MouseRightButtonDown += FoldersTreeView_MouseRightButtonDown;
            FoldersTreeView.Items.Add(item);
        }
        // refresh the treeView automatically when the selected item is changed
        private void FoldersTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FoldersTreeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is string path)
            {
                makeTreeView(path);
            }

        }
    }
}
