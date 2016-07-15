using IE_UI.Models;
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

namespace IE_UI.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();

            App.Current.MainWindow.Title = "Home";

            /// TODO: Check if paths are valid
            /// 
            var recentFilesList = RecentFileManager.GetRecentFilesList();

            if (recentFilesList.Count() > 0)
            {
                RecentFilesListView.ItemsSource = recentFilesList.Skip(Math.Max(0, recentFilesList.Count() - 3)).Take(3); ;
            }
            else
            {
                RecentFilesPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ExtractSetup());
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ViewSetup());
        }

        private void RecentFilesListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as RecentFile;

            if (item != null)
            {
                if (item.OperationType == Char.ConvertFromUtf32(0xE7E6))
                {
                    this.NavigationService.Navigate(new ExtractProcess(new ExtractConfig()
                    {
                        SourceFilePath = String.Format("{0}\\{1}.xml", item.SourcePath, item.Name),
                        DestinationFilePath = item.DestinationPath
                    }));
                }
                else if (item.OperationType == Char.ConvertFromUtf32(0xE8E5))
                {
                    this.NavigationService.Navigate(new ViewList(String.Format("{0}\\{1}.xml", item.SourcePath, item.Name)));
                }
            }
        }
    }
}
