using System.Collections.ObjectModel;
using System.ComponentModel;
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
            Description = "This is the first tool", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.ConvoEditor)
        },
        new ToolInformation
        {
            Name = "Global Variable Editor", 
            Description = "This is the second tool", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.GlobalVarEditor)
        },
        new ToolInformation
        {
            Name = "StringTable Editor", 
            Description = "This is the third tool", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.StringTableEditor)
        },
        new ToolInformation
        {
            Name = "OAF Reader", 
            Description = "This is the fourth tool", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.OAFReader)
        },
        new ToolInformation
        {
            Name = "Quest Editor", 
            Description = "This is the fifth tool", 
            IsEnabled = Database.CurrentProject.SupportedFeatures.Contains(KnapperFeature.QuestEditor)
        }
    };
    public Homepage()
    {
        InitializeComponent();
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
        
    public new event PropertyChangedEventHandler? PropertyChanged;
}