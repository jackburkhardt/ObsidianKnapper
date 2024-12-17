using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using OEIKnapper;
using OEIKnapperGUI.Controls;

namespace OEIKnapperGUI;

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
            AddTab(new Homepage().GetViewModel());
        };
    }
    
    public void OpenDialogueEditor(object sender, RoutedEventArgs e)
    {
        AddTab(new DialogueEditor().GetViewModel());
    }
    
    public void OpenGlobalVariableEditor(object sender, RoutedEventArgs e)
    {
        AddTab(new GlobalVarEditor().GetViewModel());
    }
    
    public void OpenStringTableEditor(object sender, RoutedEventArgs e)
    {
        AddTab(new StringTableEditor().GetViewModel());
    }
    
    private void CloseTabClicked(object sender, RoutedEventArgs e)
    {
        RemoveTab(Tabs[TabView.SelectedIndex]);
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

    public void RemoveTab(TabViewModel tabInfo)
    {
        Tabs.Remove(tabInfo);
        foreach (var group in tabInfo.MenuItems)
        {
            menuRibbon.Items.Remove(group);
        }
    }

    private async void RescanGame_OnClick(object sender, RoutedEventArgs e)
    {
        var progress = new Progress<Database.ProgressReport>();
        var progressWindow = new TaskProgress(progress);
        progressWindow.Show();
        await Database.LoadProjectAsync(Database.CurrentProject.GamePath, progress);
    }
}