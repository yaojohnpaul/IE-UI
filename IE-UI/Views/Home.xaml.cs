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

            List<Test> test = new List<Test>();

            test.Add(new Test()
            {
                OperationType = Char.ConvertFromUtf32(0xE7E6),
                Name = "input",
                SourcePath = "D:\\Documents",
                DestinationPath = "D:\\Documents"
            });

            test.Add(new Test()
            {
                OperationType = Char.ConvertFromUtf32(0xE8E5),
                Name = "result",
                SourcePath = "D:\\Documents"
            });

            RecentFilesListView.ItemsSource = test;
        }

        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ExtractSetup());
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ViewSetup());
        }

        private void RecentFilesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Test;

            if (item != null)
            {
                if (item.OperationType == Char.ConvertFromUtf32(0xE7E6))
                {
                    this.NavigationService.Navigate(new ExtractProcess(new ExtractConfig()
                    {
                        SourceFilePath = String.Format("{0}\\{1}.xml", item.SourcePath, item.Name),
                        DestinationFilePath = String.Format("{0}\\{1}.xml", item.DestinationPath, "results")
                    }));
                }
                else if (item.OperationType == Char.ConvertFromUtf32(0xE8E5))
                {
                    this.NavigationService.Navigate(new ViewList(String.Format("{0}\\{1}.xml", item.SourcePath, item.Name)));
                }
            }
        }
    }

    public class Test
    {
        public string OperationType { get; set; }

        public string Name { get; set; }

        public string SourcePath { get; set; }

        public string DestinationPath { get; set; }
    }
}
