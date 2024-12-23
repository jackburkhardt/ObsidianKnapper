using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using OEIKnapper;

namespace OEIKnapperGUI.Controls;

public partial class Homepage : TabContentControl
{
    public ObservableCollection<ToolInformation> Tools { get; set; } = new()
    {
        new ToolInformation
        {
            Name = "Dialogue Editor", 
            Description = "View and edit conversations within the game in a graphical interface.", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.ConvoEditor),
            OnClick = (o,e) => MainWindow.Instance.AddTab(new DialogueEditor().ViewModel)
        },
        new ToolInformation
        {
            Name = "Global Variable Editor", 
            Description = "View and edit variables that are used for dialogue, questing, and other game features.", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.GlobalVarEditor),
            OnClick = (o,e) => MainWindow.Instance.AddTab(new GlobalVarEditor().ViewModel)
        },
        new ToolInformation
        {
            Name = "StringTable Editor", 
            Description = "View and edit \"strings\", lines of text that are used in dialogue and user interfaces. Supports toggling between languages.", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.StringTableEditor),
            OnClick = (o,e) => MainWindow.Instance.AddTab(new StringTableEditor().ViewModel)
        },
        new ToolInformation
        {
            Name = "OAF Reader", 
            Description = "Some games (i.e. Stick of Truth) use this .OAF file for bundling game data. This tool lets you extract those files.", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.OAFReader)
        },
        new ToolInformation
        {
            Name = "Quest Editor", 
            Description = "View game quests, including their dependencies and rewards.", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.QuestEditor)
        }
    };
    public Homepage()
    {
        InitializeComponent();
    }

    private void OpenTool_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is ToolInformation tool)
        {
            tool.OnClick?.Invoke(sender, e);
        }
    }
}

public class ToolInformation : INotifyPropertyChanged
{
    public string Name { get; set; }
    public string Description { get; set; } 

    private bool _isEnabled;
    public bool IsEnabled
    {
        set
        {
            _isEnabled = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
        }
        get => _isEnabled;
    }
    
    public RoutedEventHandler? OnClick { get; set; }
        
    public new event PropertyChangedEventHandler? PropertyChanged;
}