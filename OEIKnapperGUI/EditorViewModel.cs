using System.Collections.ObjectModel;
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

public class NodeViewModel
{
    public Node AffiliatedNode { get; set; }
    public int ID { get => AffiliatedNode.NodeID;  set => AffiliatedNode.NodeID = value; }

    public ObservableCollection<ConnectorViewModel> Input { get; set; } = new ObservableCollection<ConnectorViewModel>();
    public ObservableCollection<ConnectorViewModel> Output { get; set; } = new ObservableCollection<ConnectorViewModel>();
}

public class SpeechNodeViewModel : NodeViewModel
{
    Spe
}

public class EditorViewModel
{
    public ObservableCollection<NodeViewModel> Nodes { get; } = new ObservableCollection<NodeViewModel>();

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