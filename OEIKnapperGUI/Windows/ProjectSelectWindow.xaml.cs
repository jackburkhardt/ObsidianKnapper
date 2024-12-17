using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using OEIKnapper;

namespace OEIKnapperGUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public struct GameExeData()
{
    public BitmapSource Icon { get; private set; }
    public string Name { get; private set; }
    public string Path { get; private set; }
    
    public GameExeData(BitmapSource icon, string name, string path) : this()
    {
        Icon = icon;
        Name = name;
        Path = path;
    }
};
public partial class ProjectSelectWindow : Window
{
    public ObservableCollection<GameExeData> Projects { get; set; } = [];
    
    public ProjectSelectWindow()
    {
        InitializeComponent();
        this.Loaded += ProjectSelectWindow_Loaded;
    }
    
    private void ProjectSelectWindow_Loaded(object sender, RoutedEventArgs e)
    {
        string[] searchDirs = [@"C:\Program Files (x86)\Steam\steamapps\common", @"C:\Program Files\Epic Games"];
        string[] searchPaths = [@"\Pentiment\Pentiment.exe", @"\TheOuterWorlds\TheOuterWorlds.exe"];
        
        foreach (var dir in searchDirs)
        {
            foreach (var path in searchPaths)
            {
                var fullPath = dir + path;
                if (File.Exists(fullPath))
                {
                    LoadProjectData(fullPath);
                }
            }
        }
    }


    private void FileSelect_OnClick(object sender, RoutedEventArgs e)
    {
        // open file dialog for .exe
        var dialog = new OpenFileDialog
        {
            Filter = "Game executables (*.exe)|*.exe",
            InitialDirectory = @"C:\"
        };
        
        if (dialog.ShowDialog() == true)
        {
            LoadProjectData(dialog.FileName);
        }
    }

    void LoadProjectData(string path)
    {
        // pull icon from executable
        var icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
        var iconBitmapImg = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        var fname = path.Split('\\').Last();
        
        Projects.Add(new GameExeData(iconBitmapImg, fname, path));
    }

    private async void OpenProject_OnClick(object sender, RoutedEventArgs e)
    {
        if (projectList.SelectedItem == null) return;
        var project = (GameExeData) projectList.SelectedItem;
        
        var progress = new Progress<Database.ProgressReport>();
        var progressWindow = new TaskProgress(progress);
        progressWindow.Show();
        await Database.LoadProjectAsync(project.Path, progress);
        
        var mainWindow = new MainWindow
        {
            Icon = project.Icon
        };
        mainWindow.Show();
        this.Close();
    }
}