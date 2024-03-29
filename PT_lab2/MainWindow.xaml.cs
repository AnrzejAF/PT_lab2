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
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }
        // create open browse path dialog and add them to TreeViewItem
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // if reinitiaze the dialog clear the treeView
            treeView.Items.Clear();
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            // foreach loop to add all directories and files to the treeView, but only show name of the file or directory
            foreach (string dir in System.IO.Directory.GetDirectories(path))
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = System.IO.Path.GetFileName(dir);
                treeView.Items.Add(item);
                foreach (string file in System.IO.Directory.GetFiles(dir))
                {
                    TreeViewItem fileItem = new TreeViewItem();
                    fileItem.Header = System.IO.Path.GetFileName(file);
                    item.Items.Add(fileItem);
                }
            }
        }

        // add contexr menu to the treeView to read the file content and show it in the textBox
        private void treeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
            System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
            menuItem.Header = "Read";
            menuItem.Click += new RoutedEventHandler(Read_Click);
            contextMenu.Items.Add(menuItem);
            item.ContextMenu = contextMenu;
        }

        private void Read_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            string path = item.Header.ToString();
            //textBox.Text = System.IO.File.ReadAllText(path)
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
