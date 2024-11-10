using System.Collections.ObjectModel;
using System.ComponentModel;
using OEIKnapper.Conversations;

namespace OEIKnapperGUI;

public class ConnectorViewModel
{
    public string Title { get; set; }
}

public class ConnectionViewModel
{
    public ConnectorViewModel Source { get; set; }
    public ConnectorViewModel Target { get; set; }
}

public class NodeViewModel : INotifyPropertyChanged
{
    public virtual Node AffiliatedNode { get; set; }
    public int ID { get => AffiliatedNode.NodeID;  set => AffiliatedNode.NodeID = value; }

    public ObservableCollection<ConnectorViewModel> Input { get; set; } = new ObservableCollection<ConnectorViewModel>();
    public ObservableCollection<ConnectorViewModel> Output { get; set; } = new ObservableCollection<ConnectorViewModel>();
    public event PropertyChangedEventHandler? PropertyChanged;
}

public class PlayerResponseNodeViewModel : NodeViewModel
{
    
}

public class TalkNodeViewModel : NodeViewModel, INotifyPropertyChanged
{
    private string _speaker;
    private string _listener;
    
    public string Speaker { get => _speaker; set { _speaker = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speaker))); } }
    public string Listener { get => _listener; set { _listener = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Listener))); } }
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
    public ObservableCollection<NodeViewModel> Nodes { get; set; } = new ObservableCollection<NodeViewModel>();

    public EditorViewModel()
    {
        Nodes.Add(new NodeViewModel
        {
            Input = new ObservableCollection<ConnectorViewModel>
            {
                new ConnectorViewModel
                {
                    Title = "In"
                }
            },
            Output = new ObservableCollection<ConnectorViewModel>
            {
                new ConnectorViewModel
                {
                    Title = "Out"
                }
            }
        });
    }
}