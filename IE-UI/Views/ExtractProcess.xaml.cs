using Hammer.SpinningWheel;
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
    public partial class ExtractProcess : Page
    {
        private ExtractConfig Config;
        private SpinningWheel ProgressRing;
        private BackgroundWorker Worker;

        public ExtractProcess(ExtractConfig config)
        {
            this.InitializeComponent();

            App.Current.MainWindow.Title = "Extracting";

            Config = config;

            ProgressRing = new SpinningWheel();

            ProgressRing.CircleCount = 10;
            ProgressRing.Radius = 35;
            ProgressRing.Speed = 0.9;

            ProgressRingContainer.Children.Add(ProgressRing);
            Grid.SetRow(ProgressRing, 0);

            Worker = new BackgroundWorker();
            //Worker.WorkerSupportsCancellation = true;

            StatusTextBlock.Text = "extracting";

            Worker.DoWork += delegate (object s, DoWorkEventArgs args)
            {
                args.Result = StartProcess();
            };

            Worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
            {
                if (!(bool)args.Result)
                {
                    MessageBox.Show(Application.Current.MainWindow,
                        "Invalid input file.",
                        "Process failed");

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
        }

        private bool StartProcess()
        {
            return Task.Run(() => IE_lib.Main.Extract(Config.SourceFilePath, Config.DestinationFilePath)).Result;
        }

        private void EndProcess()
        {
            ProgressRing.Visibility = Visibility.Hidden;
            //RepeatButton.Visibility = Visibility.Visible;

            StatusTextBlock.Margin = new Thickness(0);
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Home());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Worker.RunWorkerAsync();
        }
    }
}
