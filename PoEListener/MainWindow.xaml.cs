using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace PoEListener
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private FileSystemWatcher _logWatcher;
        public MainWindow()
        {
            InitializeComponent();

            TextBoxLogFile.Text = Properties.Settings.Default.LogPath;
            if (TextBoxLogFile.Text != string.Empty) InitilizeListener();
        }

        private void InitilizeListener()
        {
            var LogFile = new FileInfo(Properties.Settings.Default.LogPath);
            if (LogFile.Length > 2000000)
            {
                LabelLogFileInfo.Visibility = Visibility.Visible;
                LabelLogFileInfo.Content = $"Your log file is {LogFile.Length/1000} kb, this can potentially slow down usage, consider";
                LabelCompress.Visibility = Visibility.Visible;
            }
            var directory = Properties.Settings.Default.LogPath.Remove(Properties.Settings.Default.LogPath.Length - 10, 10);
            _logWatcher = new FileSystemWatcher
            {
                Path = directory,
                Filter = "Client.txt",
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                
            };
            _logWatcher.Changed += LogWatcherOnChanged;
        }

        private void LogWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            Console.WriteLine("Changed: ");
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var fileBrowser = new OpenFileDialog
            {
                FileName = "Client.txt",
                Filter = "Client.txt|Client.txt",
            };

            fileBrowser.ShowDialog();
            if (fileBrowser.FileName == string.Empty) return;
            if (!fileBrowser.FileName.Contains("Client.txt"))
            {
                MessageBox.Show(@"File selected does not match expected Client.txt");
                return;
            }

            TextBoxLogFile.Text = fileBrowser.FileName;
            Properties.Settings.Default.LogPath = fileBrowser.FileName;
            InitilizeListener();
        }
    }
}
