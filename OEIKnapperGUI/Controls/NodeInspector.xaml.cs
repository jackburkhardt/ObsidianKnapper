using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using OEIKnapper.Conversations;

namespace OEIKnapperGUI.Controls;

public partial class NodeInspector : UserControl
{
    public NodeInspector()
    {
        InitializeComponent();
    }
}

public class ExtendedPropertyNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string prop)
        {
            return prop.Split(',');
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string[] props)
        {
            return string.Join(",", props);
        }

        return value;
    }
}

public class ConditionalComponentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ConditionalExpression exp) return value;

        return exp.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}