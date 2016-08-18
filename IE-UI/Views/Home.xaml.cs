using IE_UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// <seealso cref="System.Windows.Controls.Page" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class Home : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            App.Current.MainWindow.Title = "Home";
            
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

        /// <summary>
        /// Handles the Click event of the ExtractButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ExtractSetup());
        }

        /// <summary>
        /// Handles the Click event of the ViewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ViewSetup());
        }

        /// <summary>
        /// Handles the PreviewMouseLeftButtonDown event of the RecentFilesListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void RecentFilesListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as RecentFile;

            if (item != null)
            {
                if (item.OperationType == "/assets/images/extract.png")
                {
                    ExtractConfig config = new ExtractConfig()
                    {
                        SourceFilePath = String.Format("{0}\\{1}.xml", item.SourceFilePath, item.Name),
                        DestinationFilePath = item.DestinationFilePath
                    };

                    if (Directory.Exists(System.IO.Path.GetDirectoryName(config.DestinationFilePath)) && File.Exists(config.SourceFilePath))
                    {
                        this.NavigationService.Navigate(new ExtractProcess(config));
                    }
                    else
                    {
                        MessageBox.Show(Application.Current.MainWindow,
                            "Please enter valid file paths.",
                            "Invalid file paths");
                    }
                }
                else if (item.OperationType == "/assets/images/view.png")
                {
                    string sourceFilePath = String.Format("{0}\\{1}.xml", item.SourceFilePath, item.Name);

                    if (File.Exists(sourceFilePath))
                    {
                        this.NavigationService.Navigate(new ViewList(sourceFilePath));
                    }
                    else
                    {
                        MessageBox.Show(Application.Current.MainWindow,
                            "Please enter valid file path.",
                            "Invalid file path");
                    }
                }
            }
        }
    }
}
