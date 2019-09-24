using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AutoScrollListView
{
    public class MainWindowModel
    {
        private readonly ICommand addMessageCommand;
        public ObservableCollection<string> Messages { get; set; }
        public ICommand AddMessageCommand => addMessageCommand;
        public MainWindowModel()
        {
            addMessageCommand = new RelayCommand<object>(AddMessage, CanAddMessage);
            Messages = new ObservableCollection<string>();
        }

        private void AddMessage(object obj)
        {
            //Task.Factory.StartNew(() =>
            //{
            //    for (int i = 0; i < 20; i++)
            //    {
            //        Thread.Sleep(1000);

            //        if (Application.Current.Dispatcher != null)
            //        {
            //            var i1 = i;
            //            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render,
            //                new Action(() => { Messages.Add($"Thread A {i1.ToString()}"); }));
            //        }
            //    }
            //});

            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(500);

                    if (Application.Current.Dispatcher != null)
                    {
                        var i1 = i;
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render,
                            new Action(() => { Messages.Add($"{DateTime.Now:yyyyMMddhhmmsss}=> Thread B G"); }));
                    }
                }
            });

        }

        private bool CanAddMessage(object obj)
        {
            return true;
        }
    }
}
