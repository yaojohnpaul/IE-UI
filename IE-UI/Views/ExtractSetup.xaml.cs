using IE_UI.Models;
using Microsoft.Win32;
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
    /// Interaction logic for ExtractSetup.xaml
    /// </summary>
    public partial class ExtractSetup : Page
    {
        public ExtractSetup()
        {
            InitializeComponent();

            App.Current.MainWindow.Title = "Extract";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Home());
        }

        private void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            /// TODO: Check if paths are valid

            if (SourceTextBox.Text.Any() && DestinationTextBox.Text.Any())
            {
                this.NavigationService.Navigate(new ExtractProcess(new ExtractConfig()
                {
                    SourceFilePath = SourceTextBox.Text,
                    DestinationFilePath = DestinationTextBox.Text
                }));
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, 
                    "Please enter valid file paths.",
                    "Invalid file paths");
            }
        }

        private void SourceBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.Filter = "XML Document|*.xml";

            Nullable<bool> result = ofd.ShowDialog();

            if (result == true)
            {
                string filename = ofd.FileName;
                SourceTextBox.Text = filename;

                if (DestinationTextBox.Text.Any())
                {
                    ProceedPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void DestinationBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".xml";
            sfd.Filter = "XML Document|*.xml";
            sfd.FileName = "result";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            Nullable<bool> result = sfd.ShowDialog();

            if (result == true)
            {
                string filename = sfd.FileName;
                DestinationTextBox.Text = filename;

                if (SourceTextBox.Text.Any())
                {
                    ProceedPanel.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
