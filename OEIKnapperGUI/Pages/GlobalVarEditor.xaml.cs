using System.Windows;
using Newtonsoft.Json;
using OEIKnapper;
using PropertyTools.Wpf;

namespace OEIKnapperGUI;

public partial class GlobalVarEditor : TabContentControl
{
    public static readonly DependencyProperty CurrentVarProperty = DependencyProperty.Register(
        nameof(CurrentVar), typeof(GlobalVariable), typeof(GlobalVarEditor), new PropertyMetadata(default(GlobalVariable)));

    public GlobalVariable CurrentVar
    {
        get => (GlobalVariable) GetValue(CurrentVarProperty);
        set => SetValue(CurrentVarProperty, value);
    }
    public GlobalVarEditor()
    {
        InitializeComponent();
        DataContext = this;
        
        Loaded += (sender, args) =>
        {
            variableList.OnPathSelected += UpdateViewedVariable; 
            variableList.ItemsSource = Database.GlobalVariables.Select(v => v.Tag);
        };
    }
    
    public void UpdateViewedVariable(string tag)
    {
        var originalVar = Database.GlobalVariables[tag];
        var copy = new GlobalVariable()
        {
            ID = originalVar.ID,
            Tag = originalVar.Tag,
            Type = originalVar.Type,
            InitialValue = originalVar.InitialValue
        };

        CurrentVar = copy;
    }
    
    public void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(JsonConvert.SerializeObject(Database.GlobalVariables[CurrentVar.Tag]), "Save Changes", MessageBoxButton.YesNo);
        propertyEditor.GetBindingExpression(PropertyGrid.SelectedObjectProperty)?.UpdateSource();
        Database.GlobalVariables[CurrentVar.Tag] = CurrentVar;
        MessageBox.Show(JsonConvert.SerializeObject(Database.GlobalVariables[CurrentVar.Tag]), "Save Changes", MessageBoxButton.YesNo);
    }
    
}
