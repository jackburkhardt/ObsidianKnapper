using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace OEIKnapperGUI.Controls;

public partial class DataTree : UserControl
{
    public delegate void PathDelegate(string fullPath);
    public event PathDelegate? OnPathSelected;
    
    private IEnumerable<string> _paths = [];
    public IEnumerable<string> Paths
    {
        get => _paths;
        set
        {
            _paths = value;
            BuildTree(value);
        }
    }
    
    public char Seperator
    {
        get => (char)GetValue(SeperatorProperty);
        set => SetValue(SeperatorProperty, value);
    }
    
    public static DependencyProperty SeperatorProperty = DependencyProperty.Register(
        "Seperator", typeof(char), typeof(DataTree), new PropertyMetadata('/'));
    
    public DataTree()
    {
        InitializeComponent();
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
        var header = item.Header.ToString() ?? "";
        if (header.Contains(search, StringComparison.CurrentCultureIgnoreCase))
        {
            item.Visibility = Visibility.Visible;
        }
        else
        {
            item.Visibility = Visibility.Collapsed;
        }
        
        if (item.Parent is TreeViewItem parent)
        {
            parent.Visibility = parent.Items.Cast<TreeViewItem>().Any(i => i.Visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
            parent.IsExpanded = parent.Visibility == Visibility.Visible;
        }
        
        // le classic depth first search
        foreach (TreeViewItem child in item.Items)
        {
            ToggleVisibleBySearch(child, search);
        }
        
    }

    public void BuildTree(IEnumerable<string> paths)
    {
        var rootNodes = new List<TreeViewItem>();
       
        foreach (var path in paths)
        {
            TreeViewItem? current = null;
            var parts = path.Split(Seperator);
            for (int i = 0; i < parts.Length; i++)
            {
                if (i == 0)
                {
                    // see if we have a root node with the same name
                    var existingRoot = rootNodes.FirstOrDefault(x => (string)x.Header == parts[i]);
                    if (existingRoot != null)
                    {
                        current = existingRoot;
                        continue;
                    }
                    var tag = i == parts.Length - 1 ? "file" : "folder";
                    current = new TreeViewItem { Header = parts[i], Tag = tag };
                    rootNodes.Add(current);
                    continue;
                }
                
                var found = false;
                foreach (var item in current.Items)
                {
                    if (item is TreeViewItem tvi && (string)tvi.Header == parts[i])
                    {
                        current = tvi;
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

        treeView.ItemsSource = rootNodes;
    }


    private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        var item = (TreeViewItem)treeView.SelectedItem;
        if (item == null) return;
        if (item.Tag.ToString() == "file")
        {
            var path = item.Header.ToString();
            while (item.Parent is TreeViewItem parent)
            {
                item = parent;
                path = item.Header + "/" + path;
            }
            OnPathSelected?.Invoke(path);
        }
    }
}

public class TreeItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate? FolderTemplate { get; set; }
    public DataTemplate? FileTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is TreeViewItem treeViewItem)
        {
            return treeViewItem.Tag.ToString() == "file" ? FileTemplate : FolderTemplate;
        }
        
        return base.SelectTemplate(item, container);
    }
}