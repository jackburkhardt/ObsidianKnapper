using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Newtonsoft.Json;
using OEIKnapper;
using PropertyTools.Wpf;

namespace OEIKnapperGUI;

public partial class GlobalVarEditor : UserControl
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
            variableList.ItemsSource = MainWindow.Database.GlobalVariables.Select(v => v.Tag);
        };
    }
    
    public void UpdateViewedVariable(string tag)
    {
        var originalVar = MainWindow.Database.GlobalVariables[tag];
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
        MessageBox.Show(JsonConvert.SerializeObject(MainWindow.Database.GlobalVariables[CurrentVar.Tag]), "Save Changes", MessageBoxButton.YesNo);
        propertyEditor.GetBindingExpression(PropertyGrid.SelectedObjectProperty)?.UpdateSource();
        MainWindow.Database.GlobalVariables[CurrentVar.Tag] = CurrentVar;
        MessageBox.Show(JsonConvert.SerializeObject(MainWindow.Database.GlobalVariables[CurrentVar.Tag]), "Save Changes", MessageBoxButton.YesNo);
    }
    
}
