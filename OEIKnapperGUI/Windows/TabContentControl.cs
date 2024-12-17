using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace OEIKnapperGUI;

public abstract class TabContentControl : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty TabHeaderProperty = DependencyProperty.Register(nameof(TabHeader), typeof(string), typeof(TabContentControl));

    public string TabHeader
    {
        get => (string)GetValue(TabHeaderProperty);
        set => SetValue(TabHeaderProperty, value);
    }

    public ObservableCollection<RibbonGroup> MenuGroups { get; set; } = new();
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public TabViewModel GetViewModel()
    {
        return new TabViewModel
        {
            Content = this,
            Header = TabHeader,
            MenuItems = MenuGroups
        };
    }

}