﻿using Hammer.SpinningWheel;
using IE_UI.Models;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IE_UI.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Page" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class ExtractProcess : Page
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private ExtractConfig Config;
        /// <summary>
        /// The progress ring
        /// </summary>
        private SpinningWheel ProgressRing;
        /// <summary>
        /// The worker
        /// </summary>
        private BackgroundWorker Worker;
        /// <summary>
        /// The status text
        /// </summary>
        private string StatusText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractProcess"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public ExtractProcess(ExtractConfig config)
        {
            this.InitializeComponent();

            App.Current.MainWindow.Title = "Extracting";

            Config = config;

            Worker = new BackgroundWorker();
            //Worker.WorkerSupportsCancellation = true;
            Worker.WorkerReportsProgress = true;

            Worker.DoWork += delegate (object s, DoWorkEventArgs args)
            {
                args.Result = StartProcess();
            };

            Worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
            {
                if (!(bool)args.Result)
                {
                    MessageBox.Show(Application.Current.MainWindow,
                        "There was a problem in extracting the 5Ws. The input file does not contain any articles. \n\nKindly check the input file.",
                        "Extracting");

                    StatusTextBlock.Text = "process failed";

                    this.NavigationService.Navigate(new Home());
                }
                else
                {
                    StatusTextBlock.Text = "process completed";

                    this.NavigationService.Navigate(new ViewList(Config.DestinationFilePath));
                }

                EndProcess();
            };

            Worker.ProgressChanged += delegate (object s, ProgressChangedEventArgs args)
            {
                int percentage = args.ProgressPercentage;

                ProgressBar.Value = percentage;

                StatusTextBlock.Text = StatusText;
            };
        }

        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <returns></returns>
        private bool StartProcess()
        {
            return Task.Run(() => IE_lib.Main.Extract(Config.SourceFilePath, Config.DestinationFilePath, Worker, ref StatusText)).Result;
        }

        /// <summary>
        /// Ends the process.
        /// </summary>
        private void EndProcess()
        {
            //ProgressRing.Visibility = Visibility.Hidden;

            StatusTextBlock.Margin = new Thickness(0);
        }

        /// <summary>
        /// Handles the Click event of the RepeatButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Home());
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Worker.RunWorkerAsync();
        }
    }
}
