using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using OEIKnapper.Conversations;
using OEIKnapperGUI.Windows;

namespace OEIKnapperGUI.Pages;

public partial class DialogueEditor : TabContentControl
{
    private Conversation _currentConvo;
    
    public DialogueEditor()
    {
        InitializeComponent();
        Loaded += (sender, args) =>
        {
            nodeInspector.RelatedEditor = this;
            convoBrowser.OnPathSelected += UpdateViewedConvo;
            convoBrowser.ItemsSource = Database.ConvoLookup.Select(c => c.Filename);
        };
    }

    public async void UpdateViewedConvo(string path)
    {
        var convo = await Database.LoadConversationAsync(path);
        _currentConvo = convo;
        SetTabHeader($"Dialogue Editor ({Path.GetFileName(path)})");

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
                        Speaker = talkNode.SpeakerGuid == Guid.Empty ? "None" : Database.Speakers[talkNode.SpeakerGuid].Tag,
                        Listener = talkNode.ListenerGuid == Guid.Empty ? "None" : Database.Speakers[talkNode.ListenerGuid].Tag
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
            

        ((DialogueEditorViewModel)nodeEditor.DataContext).Nodes.Clear();
        ((DialogueEditorViewModel)nodeEditor.DataContext).Connections.Clear();
        foreach (var node in nodes)
        {
            ((DialogueEditorViewModel)nodeEditor.DataContext).Nodes.Add(node);
        }

        foreach (var conn in connections)
        {
            ((DialogueEditorViewModel)nodeEditor.DataContext).Connections.Add(conn);
        }
            

    }

    private ObservableCollection<ConnectionViewModel> ArrangeNodesForView(List<NodeViewModel> nodes)
    {
        var connections = new ObservableCollection<ConnectionViewModel>();
        var sortedNodes = nodes.OrderBy(n => n.ID).ToList();
        var nodeDict = sortedNodes.ToDictionary(n => n.ID, n => n); 
        var firstNode = sortedNodes.First(n => n.ID != -200);

        var placedNodes = new Stack<int>();
        
        firstNode.Location = new Point(100, 100);
        PlaceNodesAndConnect(firstNode, firstNode.Location);

        return connections;

        void PlaceNodesAndConnect(NodeViewModel node, Point lastPos)
        {
            var linkCount = node.AffiliatedNode.Links.Count;
            for (int i = 0; i < linkCount; i++)
            {
                var toNode = nodeDict[node.AffiliatedNode.Links[i].ToNodeID];
                var pos = new Point(lastPos.X + 300, lastPos.Y + (i / ((float)linkCount * -1)) * 400);
                node.Location = pos;
                var connection = new ConnectionViewModel()
                    { 
                        SourceNode = node, 
                        TargetNode = toNode,
                        IsConditional = toNode.Conditionals.Conditions.Count > 0,
                        Conditional = toNode.Conditionals,
                        HasExtendedProperties = node.AffiliatedNode.Links[i].ExtendedProperties.Count > 0,
                        ExtendedProperties = node.AffiliatedNode.Links[i].ExtendedProperties
                    };
                
                connections.Add(connection);
                node.Connections.Add(connection);
                
                
                if (!placedNodes.Contains(toNode.ID))
                {
                    placedNodes.Push(toNode.ID);
                    PlaceNodesAndConnect(toNode, pos);
                }
            }
        }
    }

    public void SetFocusOnNode(int nodeID)
    {
        var node = ((DialogueEditorViewModel)nodeEditor.DataContext).Nodes.FirstOrDefault(n => n.ID == nodeID);
        if (node != null)
        {
            SetFocusOnNode(node);
        }
    }
    
    public void SetFocusOnNode(NodeViewModel node)
    {
        nodeEditor.Focus();
        nodeEditor.BringIntoView(node.Location, true, () =>
        {
            ((DialogueEditorViewModel)nodeEditor.DataContext).SelectedNode = node;
        });
    }

    private void GoToBox_Changed(object sender, RoutedEventArgs e)
    {
        if (!Equals(sender, goToBox)) return;
        if (int.TryParse(goToBox.Text, out var id))
        {
            var node = ((DialogueEditorViewModel)nodeEditor.DataContext).Nodes.FirstOrDefault(n => n.ID == id);
            if (node != null)
            {
                SetFocusOnNode(node);
            }
        }
    }
}