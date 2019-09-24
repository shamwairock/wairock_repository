using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace ProgressBarTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProgressbarWindow pbw = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Calls the method that does work of which the progress will be shown.
            ShowProgress();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Notifying the progress bar window of the current progress.
            pbw.UpdateProgress(e.ProgressPercentage);
        }

        void ShowProgress()
        {
            try
            {
                // Using background worker to asynchronously run work method.
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += ProcessLogsAsynch;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void ProcessLogsAsynch(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                // Disabling parent window controls while the work is being done.
                btnStart.IsEnabled = false;

                // Launch the progress bar window using Show()
                // Note: ShowDialog() wouldn't work as it waits for the child window to close.
                pbw = new ProgressbarWindow();
                pbw.Show();
            });

            for (int i = 1; i <= 100; i++)
            {
                // Simulates work being done
                Thread.Sleep(100);

                // Reports progress
                (sender as BackgroundWorker).ReportProgress(i);
            }

            Dispatcher.Invoke(() =>
            {
                // Enables parent window controls
                btnStart.IsEnabled = true;
            });
        }
    }
}
