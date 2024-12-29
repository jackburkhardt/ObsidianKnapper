using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using OEIKnapperGUI.Pages;

namespace OEIKnapperGUI.Windows;

public partial class MainWindow : Window
{
    public static MainWindow Instance;
    public ObservableCollection<TabViewModel> Tabs { get; set; } = [];

    public MainWindow()
    {
        InitializeComponent();
        Loaded += (sender, args) =>
        {
            Instance = this;
            AddTab(new Homepage().ViewModel);
        };
    }
    
    public void OpenDialogueEditor(object sender, RoutedEventArgs e)
    {
        AddTab(new DialogueEditor().ViewModel);
    }
    
    public void OpenGlobalVariableEditor(object sender, RoutedEventArgs e)
    {
        AddTab(new GlobalVarEditor().ViewModel);
    }
    
    public void OpenStringTableEditor(object sender, RoutedEventArgs e)
    {
        AddTab(new StringTableEditor().ViewModel);
    }
    
    private void CloseTabClicked(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is TabViewModel tab)
        {
            RemoveTab(tab);
        }
    }

    public void AddTab(TabViewModel tabInfo)
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
        if (sender is not TabControl) return;
        
        var lastTab = e.RemovedItems.Count > 0 ? e.RemovedItems[0] : null;
        var newTab = e.AddedItems.Count > 0 ? e.AddedItems[0] : null;

        if (lastTab is TabViewModel ltab)
        {
            foreach (var oldGroup in ltab.MenuItems)
            {
                oldGroup.Visibility = Visibility.Collapsed;
            }
        }

        if (newTab is TabViewModel ntab)
        {
            foreach (var newGroup in ntab.MenuItems)
            {
                newGroup.Visibility = Visibility.Visible;
            }
        }
    }

    public void RemoveTab(TabViewModel tabInfo)
    {
        Tabs.Remove(tabInfo);
        foreach (var group in tabInfo.MenuItems)
        {
            menuRibbon.Items.Remove(group);
        }

        if (Tabs.Count == 0)
        {
            AddTab(new Homepage().ViewModel);
        }
    }

    private async void RescanGame_OnClick(object sender, RoutedEventArgs e)
    {
        var progress = new Progress<Database.ProgressReport>();
        var progressWindow = new TaskProgress(progress);
        progressWindow.Show();
        Database.CurrentProject.SearchForGameAssets();
        await Database.LoadProjectAsync(Database.CurrentProject.GamePath, progress);
    }
}