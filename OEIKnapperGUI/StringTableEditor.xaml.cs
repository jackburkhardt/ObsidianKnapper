using System.Windows.Controls;
using OEIKnapper;
using String = OEIKnapper.String;

namespace OEIKnapperGUI;

public partial class StringTableEditor : UserControl
{
    public StringTableEditor()
    {
        InitializeComponent();
        DataTree.OnPathSelected += UpdateViewedTable;
    }

    public void UpdateViewedTable(string path)
    {
        var table = MainWindow.Database.StringTable[path];
        stringTableDisplay.ItemsSource = table.Strings;
    }

    private void GotoBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!int.TryParse(gotoBox.Text, out var id))
        {
            return;
        }
        
        foreach (var node in stringTableDisplay.Items)
        {
            if (node is String entry && entry.ID == id)
            {
                stringTableDisplay.ScrollIntoView(node);
                stringTableDisplay.SelectedItem = node;
                break;
            }
        }
    }
}