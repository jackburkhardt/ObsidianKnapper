using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Ribbon;

namespace OEIKnapperGUI;

public class TabViewModel : INotifyPropertyChanged
{
    public string Header
    {
        get => content.TabHeader;
        set
        {
            content.TabHeader = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Header)));
        }
    }

    public TabContentControl Content
    {
        get => content;
        set
        {
            content = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Content)));
        }
    }
    private TabContentControl content;
    
    public ObservableCollection<RibbonGroup> MenuItems { get; set; } = new();
   
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
}