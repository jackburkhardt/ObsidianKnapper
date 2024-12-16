using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using OEIKnapper;
using OEIKnapper.Conversations;

namespace OEIKnapperGUI;

public class ConnectorViewModel
{
    private Point _anchor;
    public Point Anchor
    {
        set
        {
            _anchor = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Anchor)));
        }
        get => _anchor;
    }
    
    private bool _isConditional;
    public bool IsConditional
    {
        set
        {
            _isConditional = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConditional)));
        }
        get => _isConditional;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}

public class ConnectionViewModel
{
    public ConnectorViewModel Source { get; set; }
    public ConnectorViewModel Target { get; set; }
}

public class NodeViewModel : INotifyPropertyChanged
{
    public string NodeContentContext { get; set; }
    public Node AffiliatedNode { get; set; }
    public int ID { get => AffiliatedNode.NodeID;  set => AffiliatedNode.NodeID = value; }
    
    private Point _location;
    public Point Location
    {
        set
        {
            _location = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Location)));
        }
        get => _location;
    }

    public ConnectorViewModel InConnector { get; set; } = new();
    public ConnectorViewModel OutConnector { get; set; } = new();
    public event PropertyChangedEventHandler? PropertyChanged;
}

public class PlayerResponseNodeViewModel : NodeViewModel, INotifyPropertyChanged
{
    public string Content
    {
        get => Database.StringTable[NodeContentContext][ID];
        set
        {
            Database.StringTable[NodeContentContext][ID] = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Content)));
        }
    }

    public new event PropertyChangedEventHandler? PropertyChanged;
}

public class TalkNodeViewModel : NodeViewModel, INotifyPropertyChanged
{
    private string _speaker;
    private string _listener;
    
    public string Speaker { get => _speaker; set { _speaker = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speaker))); } }
    public string Listener { get => _listener; set { _listener = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Listener))); } }

    public string Content
    {
        get => Database.StringTable[NodeContentContext][ID];
        set
        {
            Database.StringTable[NodeContentContext][ID] = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Content)));
        }
    }
    
    public new event PropertyChangedEventHandler? PropertyChanged;
}

public class ScriptNodeViewModel : NodeViewModel
{
    
}

public class BankNodeViewModel : NodeViewModel
{
    
}

public class EditorViewModel
{
    public ObservableCollection<NodeViewModel> Nodes { get; set; } = [];
    public ObservableCollection<ConnectionViewModel> Connections { get; } = [];
    
    public EditorViewModel()
    {

    }
}