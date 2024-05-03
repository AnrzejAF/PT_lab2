using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IOPath = System.IO.Path;
using System.IO;
using static PT_lab2.FileInfoViewModel;
using System.Globalization;
using System.Windows.Data;
using static PT_lab2.MainWindow;
using System.ComponentModel;
using static PT_lab2.CultureResources;


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
            _fileExplorer = new FileExplorer();
            DataContext = _fileExplorer;
            _fileExplorer.PropertyChanged += _fileExplorer_PropertyChanged;

        }
        private FileExplorer _fileExplorer;

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
                _fileExplorer.OpenRoot(path);
            }
        }

        private void _fileExplorer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FileExplorer.Lang))
                CultureResources.ChangeCulture(CultureInfo.CurrentUICulture);
        }
    }

    public class LangBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == (string)parameter)
                return true;
            return false;
        }
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
                return (string)parameter;
            return null;
        }
    }

    public class FileExplorer : ViewModelBase
    {
        public DirectoryInfoViewModel? Root { get; set; }

        public FileExplorer()
        {
            NotifyPropertyChanged(nameof(Lang));
        }

        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel();
            Root.Open(path);
        }

        public string Lang
        {
            get { return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName; }
            set
            {
                if (value != null)
                    if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName != value)
                    {
                        CultureInfo.CurrentUICulture = new CultureInfo(value);
                        NotifyPropertyChanged();
                    }
            }
        }
    }
}