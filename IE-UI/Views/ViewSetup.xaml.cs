﻿using IE_UI.Models;
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
    /// Interaction logic for ViewSetup.xaml
    /// </summary>
    public partial class ViewSetup : Page
    {
        public ViewSetup()
        {
            InitializeComponent();

            App.Current.MainWindow.Title = "View";
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

                ProceedPanel.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Home());
        }

        private void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            if (SourceTextBox.Text.Any())
            {
                this.NavigationService.Navigate(new ViewList(SourceTextBox.Text));

                RecentFileManager.AddRecentFile(new RecentFile()
                {
                    OperationType = Char.ConvertFromUtf32(0xE8E5),
                    Name = System.IO.Path.GetFileNameWithoutExtension(SourceTextBox.Text),
                    SourcePath = System.IO.Path.GetDirectoryName(SourceTextBox.Text),
                });
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
