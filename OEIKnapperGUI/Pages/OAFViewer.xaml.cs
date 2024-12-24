using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using OEIKnapper;
using OEIKnapper.Filesystem;
using OEIKnapperGUI.Windows;

namespace OEIKnapperGUI.Pages;

public partial class OAFViewer : TabContentControl, INotifyPropertyChanged
{
    public ObsidianArchiveFile SelectedFile { get; set; } 
    private bool _showPreview = true;
    public bool ShowPreview 
    {
        get => _showPreview;
        set
        {
            _showPreview = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowPreview)));
        }
    }
    private OAFEntry? _selectedEntry;
    public OAFEntry? SelectedEntry 
    {
        get => _selectedEntry;
        set
        {
            _selectedEntry = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedEntry)));
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public OAFViewer()
    {
        InitializeComponent();
        Loaded += OAFViewer_Loaded;       
    }
    
    private void OAFViewer_Loaded(object sender, RoutedEventArgs e)
    {
        var oafFiles = Database.CurrentProject.GameAssets.Where(x => x.AssetType == GameRummager.OAFExt);
        bundleList.ItemsSource = oafFiles.Select(x => Path.GetFileName(x.GamePath));
    }

    private async void BundleList_OnPathSelected(string filename)
    {
        var oafFile = Database.CurrentProject.GameAssets.First(x => Path.GetFileName(x.GamePath) == filename);
        var reader = new OAFReader(oafFile.GamePath);
        var oafData = await reader.Read();
        
        SelectedFile = oafData;
        SelectedEntry = null;
        containedFiles.ItemsSource = oafData.Items.Select(item => item.Name);
    }
    
    private async void ContainedFiles_OnPathSelected(string fullpath)
    {
        var item = SelectedFile.Items.Find(x => x.Name == fullpath);
        if (item == null)
        {
            MessageBox.Show("Item not found in OAF file.");
            return;
        }
        
        SelectedEntry = item;
        SelectedEntry.ReadEntry(SelectedFile.Filepath);
        if (ShowPreview && Utf8.IsValid(SelectedEntry.Payload))
        {
            var text = Encoding.UTF8.GetString(SelectedEntry.Payload);
            previewBox.Text = text;
        }
        else if (ShowPreview)
        {
            previewBox.Text = $"Preview not available for {item.Name}";
        }
        else
        {
            previewBox.Text = "Previews disabled.";
        }
    }

    private async void DumpFile_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedEntry == null)
        {
            MessageBox.Show("No file selected to dump!", "Dumping Error", MessageBoxButton.OK);
            return;
        }

        var sfd = new SaveFileDialog
        {
            FileName = Path.GetFileName(SelectedEntry.Name),
        };
        if (sfd.ShowDialog() == true)
        {
            await using var fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
            await fs.WriteAsync(SelectedEntry.Payload.AsMemory(0, SelectedEntry.Payload.Length));
            fs.Close();
        }
    }
}

public partial class FilesizeStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int size)
        {
            return size switch
            {
                < 1024 => $"{size} B",
                < 1024 * 1024 => $"{size / 1024} KB",
                < 1024 * 1024 * 1024 => $"{size / 1024 / 1024} MB",
                _ => $"{size / 1024 / 1024 / 1024} GB"
            };
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}