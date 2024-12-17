using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using OEIKnapper;
using OEIKnapperGUI.Controls;

namespace OEIKnapperGUI;

public partial class MainWindow : Window
{
    public ObservableCollection<TabViewModel> Tabs { get; set; } = [];

    public MainWindow()
    {
        InitializeComponent();
        Loaded += (sender, args) =>
        {
            AddTab(new Homepage().GetViewModel());
        };
    }
    
    
    private void CloseTabClicked(object sender, RoutedEventArgs e)
    {
        RemoveTab(Tabs[TabView.SelectedIndex]);
    }

    private void AddTab(TabViewModel tabInfo)
    {
        Tabs.Add(tabInfo);
        foreach (var group in tabInfo.MenuItems)
        {
            group.Visibility = Visibility.Collapsed;
            menuRibbon.Items.Add(group);
        }
        TabView.SelectedItem = tabInfo;
    }

    private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.OriginalSource is not TabControl) return;
        
        var newTab = e.AddedItems.Cast<TabViewModel>().FirstOrDefault();
        var lastTab = e.RemovedItems.Cast<TabViewModel>().FirstOrDefault();

        if (lastTab != null)
        {
            foreach (var oldGroup in lastTab.MenuItems)
            {
                oldGroup.Visibility = Visibility.Collapsed;
            }
        }

        if (newTab != null)
        {
            foreach (var newGroup in newTab.MenuItems)
            {
                newGroup.Visibility = Visibility.Visible;
            }
        }
    }

    private void RemoveTab(TabViewModel tabInfo)
    {
        Tabs.Remove(tabInfo);
        foreach (var group in tabInfo.MenuItems)
        {
            menuRibbon.Items.Remove(group);
        }
    }
}