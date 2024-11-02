using System.Windows;
using System.Windows.Controls;
using OEIKnapper;

namespace OEIKnapperGUI;

public partial class GlobalVarEditor : UserControl
{
    public Utility.ObservableProperty<GlobalVariable> currentVar { get; set; } = new ();
    public GlobalVarEditor()
    {
        InitializeComponent();
        
        
        Loaded += (sender, args) =>
        {
            variableList.OnPathSelected += UpdateViewedVariable; 
            variableList.ItemsSource = MainWindow.Database.GlobalVariables.Select(v => v.Tag);
        };
    }
    
    public void UpdateViewedVariable(string tag)
    {
        currentVar.Value = MainWindow.Database.GlobalVariables[tag];
    }
    
    public void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow.Database.GlobalVariables[currentVar.Value.Tag] = currentVar.Value;
    }
    
    
}
