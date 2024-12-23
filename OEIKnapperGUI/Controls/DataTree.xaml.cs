using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace OEIKnapperGUI;

public partial class DataTree : UserControl
{
    public delegate void PathDelegate(string fullPath);
    public event PathDelegate? OnPathSelected;
    
    public IEnumerable<string> ItemsSource
    {
        get => (IEnumerable<string>)GetValue(ItemsSourceProperty);
        set
        {
            SetValue(ItemsSourceProperty, value);
            GetItems(value);
        }
    }

    public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        "ItemsSource", typeof(IEnumerable<string>), typeof(DataTree), new PropertyMetadata(default(ObservableCollection<string>)));
    
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

    public void GetItems(IEnumerable<string> paths)
    {
        treeView.Items.Clear();
        var root = new TreeViewItem { Header = "", Focusable = false, IsExpanded = true };
       
        foreach (var path in paths)
        {
            var current = root;
            var parts = path.Split(Seperator);
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



