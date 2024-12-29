using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using OEIKnapperGUI.Windows;
using String = OEIKnapper.String;

namespace OEIKnapperGUI.Pages;

public partial class StringTableEditor : TabContentControl
{
    private ObservableCollection<string> AvailableLocales = [];
    public StringTableEditor()
    {
        SetTabHeader($"StringTable ({Database.CurrentProject.SelectedLocale})");
        InitializeComponent();
        
        dataTree.OnPathSelected += UpdateViewedTable;
        Loaded += OnLoad;
    }

    public void OnLoad(object source, RoutedEventArgs e)
    {
        dataTree.Paths = Database.StringTable.Keys;
        AvailableLocales.Clear();
        foreach (var locale in Database.CurrentProject.AvailableLocales)
        {
            AvailableLocales.Add(locale);
        }
        localeSelectorBtn.ItemsSource = AvailableLocales;
    }

    public void UpdateViewedTable(string path)
    {
        var table = Database.StringTable[path];
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

    private void FilterBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        stringTableDisplay.Items.Filter = o =>
        {
            if (o is not String entry) return false;
            return entry.DefaultText.Contains(filterBox.Text);
        };
        
        stringTableDisplay.Items.Refresh();
    }

    private void StringTableDisplay_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        var col = e.Column;
        if (col is DataGridTextColumn textCol)
        {
            textCol.ElementStyle = (Style)FindResource("TextWrap");
        }
    }

   private async void SwitchLocale_OnClick(object sender, RoutedEventArgs e)
    {
       if (e.OriginalSource is not RibbonMenuItem item) return;
       if (item.Header is not string locale) return;
       
       await Database.SetLocaleAsync(locale);
       SetTabHeader($"StringTable ({locale})");
    }
}