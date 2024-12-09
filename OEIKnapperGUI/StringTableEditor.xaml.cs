﻿using System.Windows.Controls;
using OEIKnapper;
using String = OEIKnapper.String;

namespace OEIKnapperGUI;

public partial class StringTableEditor : UserControl
{
    public StringTableEditor()
    {
        InitializeComponent();
        
        Database.LoadStringTable("enus", table => { Database.OnFileParsed(table, typeof(List<StringTable>), "");});
        dataTree.OnPathSelected += UpdateViewedTable;
        
        Loaded += (sender, args) =>
        {
            dataTree.ItemsSource = Database.StringTable.Keys;
        };
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
}