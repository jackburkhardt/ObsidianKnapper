using System.Windows.Controls;
using OEIKnapper;

namespace OEIKnapperGUI;

public partial class DialogueEditor : UserControl
{
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
        var convo = Database.ConvoLookup[filename];
        
    }
    
}