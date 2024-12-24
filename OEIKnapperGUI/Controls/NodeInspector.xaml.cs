using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using OEIKnapper;
using OEIKnapper.Conversations;
using OEIKnapperGUI.Pages;

namespace OEIKnapperGUI.Controls;


public partial class NodeInspector : UserControl
{
    public DialogueEditor RelatedEditor { get; set; }
    
    public NodeInspector()
    {
        InitializeComponent();
    }
    
    public static string ReplaceParamGuidWithName(string original)
    {
        var condString = original;
        var guidMatches = GuidRegex().Matches(condString);
        foreach (var guid in guidMatches)
        {
            if (Guid.TryParse(guid.ToString(), out Guid g))
            {
                try
                {
                    condString = condString.Replace(guid.ToString(), Database.GlobalVariables[g].Tag);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
        
        return condString;
    }
    
    private void GoToLinkedNode_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as Button).Tag is int loc)
        {
            RelatedEditor.SetFocusOnNode(loc);
        }
    }
    
    [GeneratedRegex(@"\b[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}\b", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex GuidRegex(); // moved here for inclusion at compile-time
}

public class ExtendedPropertyConverter : IValueConverter
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

public partial class ScriptItemConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not List<NodeScriptItem> scripts) return value;
         StringBuilder sb = new();
         sb.Append("{\n");
         foreach (var item in scripts)
         {
             if (item.Functions.Parameters.Count > 0)
             {
                 sb.AppendLine(NodeInspector.ReplaceParamGuidWithName(item.Functions.ToString()));
             }
             else
             {
                 sb.AppendLine(item.Functions.ToString());
             }

             if (item.Conditional.Conditions.Count > 0)
             {
                 sb.AppendLine(NodeInspector.ReplaceParamGuidWithName(item.Conditional.ToString()));
             }
         }
         

         sb.AppendLine("}");
         return sb.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Editing scripts and conditions is not supported.");
    }
    
}

public partial class ConditionalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ConditionalExpression expression) return value;
        StringBuilder sb = new();
        if (expression.Conditions.Count > 0)
        {
            sb.AppendLine(NodeInspector.ReplaceParamGuidWithName(expression.ToString()));
        }
        return sb.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Editing scripts and conditions is not supported.");
    }
}