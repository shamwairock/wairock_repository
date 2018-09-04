using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls
{
    public class MaxLengthRule : ValidationRule
    {
        public DependencyObjectWrapper DependencyObjectWrapper { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var inputString = value as string;
            if (inputString == null)
            {
                return new ValidationResult(false, "Object is not a string.");
            }

            if (inputString.Length > DependencyObjectWrapper.MaxLength)
            {
                return new ValidationResult(false, "String out of range.");
            }

            return new ValidationResult(true, null);
        }
    }
}
