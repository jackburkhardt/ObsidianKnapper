using System.Collections.ObjectModel;
using System.Windows.Controls;
using Nodify;
using OEIKnapper;
using OEIKnapper.Conversations;

namespace OEIKnapperGUI;

public partial class DialogueEditor : UserControl
{

    private Conversation _currentConvo;
    
    public DialogueEditor()
    {
        InitializeComponent();
        Loaded += (sender, args) =>
        {
            convoBrowser.OnPathSelected += UpdateViewedConvo;
            convoBrowser.ItemsSource = Database.ConvoLookup.Select(c => c.Filename);
        };
    }

    public void UpdateViewedConvo(string filename)
    {
        Database.LoadConversation(filename, convo =>
        {
            _currentConvo = convo;

            var nodeMap = new Dictionary<int, NodeViewModel>();

            foreach (var node in _currentConvo.Nodes)
            {
                NodeViewModel newNode;
                switch (node)
                {
                    case TalkNode talkNode:
                        newNode = new TalkNodeViewModel
                        {
                            AffiliatedNode = talkNode,
                            Speaker = Database.Speakers[talkNode.SpeakerGuid].Tag,
                            Listener = Database.Speakers[talkNode.ListenerGuid].Tag
                        };
                        break;
                    case PlayerResponseNode playerResponseNode:
                        newNode = new PlayerResponseNodeViewModel
                        {
                            AffiliatedNode = playerResponseNode
                        };
                        break;
                    case ScriptNode scriptNode:
                        newNode = new ScriptNodeViewModel
                        {
                            AffiliatedNode = scriptNode
                        };
                        break;
                    case BankNode bankNode:
                        newNode = new BankNodeViewModel
                        {
                            AffiliatedNode = bankNode
                        };
                        break;
                    default:
                        throw new ArgumentException("No visual component for node type: " + node.GetType());
                }

                var nodeContext = convo.Tag.Replace(".conversation", "").ToLower();
                newNode.NodeContentContext = nodeContext;
                nodeMap.Add(newNode.ID, newNode);
            }

            var connections = ArrangeNodesForView(nodeMap);
            
            this.Dispatcher.Invoke(() =>
            {
                ((EditorViewModel)nodeEditor.DataContext).Nodes.Clear();
                foreach (var node in nodeMap)
                {
                    ((EditorViewModel)nodeEditor.DataContext).Nodes.Add(node.Value);
                }

                foreach (var conn in connections)
                {
                    ((EditorViewModel)nodeEditor.DataContext).Connections.Add(conn);
                }
            });
        });

    }

    private ObservableCollection<ConnectionViewModel> ArrangeNodesForView(Dictionary<int, NodeViewModel> nodes)
    {
        var connections = new ObservableCollection<ConnectionViewModel>();
        foreach (var node in nodes.Values)
        {
            foreach (var nodeLink in node.AffiliatedNode.Links)
            {
                var inConn = new ConnectorViewModel();
                var outConn = new ConnectorViewModel();
                
                node.OutgoingConnectors.Add(outConn);
                nodes[nodeLink.ToNodeID].IncomingConnectors.Add(inConn);
                
                connections.Add(new ConnectionViewModel(){ Source = outConn, Target = inConn});
            }
        }

        return connections;
    }

}