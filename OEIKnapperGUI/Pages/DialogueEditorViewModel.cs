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
    public event PropertyChangedEventHandler PropertyChanged;
}

public class ConnectionViewModel
{
    public NodeViewModel SourceNode { get; set; }
    public NodeViewModel TargetNode { get; set; }
    public ConnectorViewModel SourceConn { get => SourceNode.OutConnector; }
    public ConnectorViewModel TargetConn { get => TargetNode.InConnector; }
    
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
    
    public ConditionalExpression Conditional { get; set; }

    private bool _hasExtendedProperties;
    public bool HasExtendedProperties
    {
        set
        {
            _hasExtendedProperties = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasExtendedProperties)));
        }
        get => _hasExtendedProperties;
    }
    
    public List<string> ExtendedProperties { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
}

public class NodeViewModel : INotifyPropertyChanged
{
    public string NodeContentContext { get; set; }
    public Node AffiliatedNode { get; set; }
    public int ID { get => AffiliatedNode.NodeID;  set => AffiliatedNode.NodeID = value; }

    public ConditionalExpression Conditionals
    {
        get => AffiliatedNode.Conditionals;
        set => AffiliatedNode.Conditionals = value;
    }
    
    public List<string> ExtendedProperties
    {
        get => AffiliatedNode.ExtendedProperties;
        set => AffiliatedNode.ExtendedProperties = value;
    }

    public List<NodeScriptItem> OnEnterScript
    {
        get => AffiliatedNode.OnEnterScripts;
        set => AffiliatedNode.OnEnterScripts = value;
    }

    public List<NodeScriptItem> OnExitScript
    {
        get => AffiliatedNode.OnExitScripts;
        set => AffiliatedNode.OnExitScripts = value;
    }
    
    public List<NodeScriptItem> OnUpdateScript
    {
        get => AffiliatedNode.OnUpdateScripts;
        set => AffiliatedNode.OnUpdateScripts = value;
    }
    
    public List<ConnectionViewModel> Connections { get; set; } = [];
    
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

public class DialogueEditorViewModel : INotifyPropertyChanged
{
    public ObservableCollection<NodeViewModel> Nodes { get; set; } = [];
    public ObservableCollection<ConnectionViewModel> Connections { get; } = [];
    private NodeViewModel _selectedNode;
    public NodeViewModel SelectedNode
    {
        get => _selectedNode;
        set
        {
            _selectedNode = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNode)));
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public DialogueEditorViewModel()
    {

    }
}