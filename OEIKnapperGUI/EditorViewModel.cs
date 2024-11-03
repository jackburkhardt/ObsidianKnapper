using System.Collections.ObjectModel;

namespace OEIKnapperGUI;

public class ConnectorViewModel
{
    public string Title { get; set; }
}

public class NodeViewModel
{
    public string Title { get; set; }

    public ObservableCollection<ConnectorViewModel> Input { get; set; } = new ObservableCollection<ConnectorViewModel>();
    public ObservableCollection<ConnectorViewModel> Output { get; set; } = new ObservableCollection<ConnectorViewModel>();
}

public class EditorViewModel
{
    public ObservableCollection<NodeViewModel> Nodes { get; } = new ObservableCollection<NodeViewModel>();

    public EditorViewModel()
    {
        Nodes.Add(new NodeViewModel
        {
            Title = "Welcome",
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