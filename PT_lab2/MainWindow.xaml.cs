using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IOPath = System.IO.Path;
using System.IO;
using static PT_lab2.FileInfoViewModel;


namespace PT_lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class FileExplorer : ViewModelBase
        {
            public DirectoryInfoViewModel? Root { get; set; }
            public void OpenRoot(string path)
            {
                Root = new DirectoryInfoViewModel();
                Root.Open(path);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = dlg.SelectedPath;
                var fileExplorer = new FileExplorer();
                fileExplorer.OpenRoot(path);
                DataContext = fileExplorer;
            }
        }
    }
}

