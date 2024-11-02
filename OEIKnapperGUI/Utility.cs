using System.ComponentModel;
using System.Windows.Data;

namespace OEIKnapperGUI;

public class Utility
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
            System.Globalization.CultureInfo culture)
        {
            Enum enumValue = default(Enum);
            if (parameter is Type)
            {
                enumValue = (Enum)Enum.Parse((Type)parameter, value.ToString());
            }
            return enumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
            System.Globalization.CultureInfo culture)
        {
            int returnValue = 0;
            if (parameter is Type)
            {
                returnValue = (int)Enum.Parse((Type)parameter, value.ToString());
            }
            return returnValue;
        }
    }

    public class ObservableProperty<T> : INotifyPropertyChanged
    {
        private T _value;
        public T Value
        {
            get { return _value; }
            set { _value = value; NotifyPropertyChanged(nameof(Value)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}