using System.Collections.ObjectModel;
using System.Windows;
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

            var nodes = new List<NodeViewModel>();

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
                nodes.Add(newNode);
            }

            var connections = ArrangeNodesForView(nodes);
            
            this.Dispatcher.Invoke(() =>
            {
                ((EditorViewModel)nodeEditor.DataContext).Nodes.Clear();
                ((EditorViewModel)nodeEditor.DataContext).Connections.Clear();
                foreach (var node in nodes)
                {
                    ((EditorViewModel)nodeEditor.DataContext).Nodes.Add(node);
                }

                foreach (var conn in connections)
                {
                    ((EditorViewModel)nodeEditor.DataContext).Connections.Add(conn);
                }
            });
        });

    }

    private ObservableCollection<ConnectionViewModel> ArrangeNodesForView(List<NodeViewModel> nodes)
    {
        var connections = new ObservableCollection<ConnectionViewModel>();
        var sortedNodes = nodes.OrderBy(n => n.ID).ToList();
        var nodeDict = sortedNodes.ToDictionary(n => n.ID, n => n); 
        var firstNode = sortedNodes.First(n => n.ID != -200);
        
        firstNode.Location = new Point(100, 100);
        PlaceNodesAndConnect(firstNode, firstNode.Location);

        return connections;

        void PlaceNodesAndConnect(NodeViewModel node, Point lastPos)
        {
            var linkCount = node.AffiliatedNode.Links.Count;
            for (int i = 1; i <= linkCount; i++)
            {
                var toNode = nodeDict[node.AffiliatedNode.Links[i].ToNodeID];
                var pos = new Point(lastPos.X + 200, lastPos.Y + 200 * i);
                node.Location = pos;
                connections.Add(new ConnectionViewModel(){ Source = node.OutConnector, Target = toNode.InConnector});
                PlaceNodesAndConnect(toNode, pos);
            }
        }
    }

}