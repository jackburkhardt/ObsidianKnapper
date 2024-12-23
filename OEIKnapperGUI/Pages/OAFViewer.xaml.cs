using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using OEIKnapper;
using OEIKnapper.Filesystem;

namespace OEIKnapperGUI.Pages;

public partial class OAFViewer : TabContentControl
{
    public ObsidianArchiveFile SelectedFile { get; set; }
    
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
        containedFiles.ItemsSource = oafData.Items.Select(item => item.Name);
    }
    
    private void ContainedFiles_OnPathSelected(string fullpath)
    {
        fullpath = fullpath.Replace('/', '\\');
        var item = SelectedFile.Items.First(x => x.Name == fullpath);
        
    }

    private void DumpFile_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}