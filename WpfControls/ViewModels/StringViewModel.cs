using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace WpfControls
{
    public class StringViewModel : UiModel, INotifyDataErrorInfo
    {
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        private readonly ConcurrentDictionary<string, List<string>> propErrors = new ConcurrentDictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errors;
            if (propertyName == null) return null;
            propErrors.TryGetValue(propertyName, out errors);

            return errors;
        }

        public bool HasErrors
        {
            get { return propErrors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler == null) return;
            var arg = new DataErrorsChangedEventArgs(propertyName);
            handler.Invoke(this, arg);
        }

        private void Validate()
        {
            if (propErrors.IsEmpty)
            {
                propErrors.TryAdd("Value", new List<string>());
            }

            var errorMessages = propErrors["Value"];

            if (Value.Length > 10)
            {
                errorMessages.Add("Length cannot more than 10.");
            }
            else
            {
                errorMessages.Clear();
            }
            RaiseErrorsChanged("Value");
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "Value")
            {
                Validate();
            }

            base.OnPropertyChanged(propertyName);
        }

        private RelayCommand lostFocusCommand;
        public RelayCommand LostFocusCommand => lostFocusCommand ?? new RelayCommand(OnLostFocus);

        private void OnLostFocus(object obj)
        {
            
        }
    }
}
