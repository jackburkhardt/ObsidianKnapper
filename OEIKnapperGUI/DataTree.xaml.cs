using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using OEIKnapper;

namespace OEIKnapperGUI;

public partial class DataTree : UserControl
{

    public delegate void PathDelegate(string fullPath);
    public static event PathDelegate OnPathSelected;
    
    public TreeView TreeView => treeView;
    
    public DataTree()
    {
        InitializeComponent();
        
         this.Loaded += DataTree_Loaded; ;
    }
    
    private void DataTree_Loaded(object sender, RoutedEventArgs e)
    {
        GetItems(MainWindow.Database.StringTable.Keys);
    }

    private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        foreach (TreeViewItem item in treeView.Items)
        {
            ToggleVisibleBySearch(item, searchBox.Text);
        }
    }
    
    private void ToggleVisibleBySearch(TreeViewItem item, string search)
    {
        var header = item.Header.ToString();
        if (header.Contains(search))
        {
            item.Visibility = Visibility.Visible;
        }
        else
        {
            item.Visibility = Visibility.Collapsed;
        }
        
        foreach (TreeViewItem child in item.Items)
        {
            ToggleVisibleBySearch(child, search);
        }
    }

    private void GetItems(IEnumerable<string> paths, char seperator = '/')
    {
        treeView.Items.Clear();
        var root = new TreeViewItem { Header = "", Focusable = false, IsExpanded = true };
       
        foreach (var path in paths)
        {
            var current = root;
            var parts = path.Split(seperator);
            for (int i = 0; i < parts.Length; i++)
            {
                var found = false;
                foreach (var item in current.Items)
                {
                    if (((TreeViewItem)item).Header.ToString() == parts[i])
                    {
                        current = (TreeViewItem)item;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var tag = i == parts.Length - 1 ? "file" : "folder";
                    var newItem = new TreeViewItem { Header = parts[i], Tag = tag };
                    current.Items.Add(newItem);
                    current = newItem;
                }
            }
        }

        // removes the extra root node
        List<TreeViewItem> rootNodes = root.Items.Cast<TreeViewItem>().ToList();
        foreach (TreeViewItem branch in rootNodes)
        {
            root.Items.Remove(branch);
            treeView.Items.Add(branch);
        }
    }



}



