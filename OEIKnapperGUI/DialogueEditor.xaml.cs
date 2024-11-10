using System.Collections.ObjectModel;
using System.Windows.Controls;
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

            var nodeList = new List<NodeViewModel>();

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

                nodeList.Add(newNode);
            }

            this.Dispatcher.Invoke(() =>
                ((EditorViewModel)nodeEditor.DataContext).Nodes = new ObservableCollection<NodeViewModel>(nodeList));

        });

    }

}