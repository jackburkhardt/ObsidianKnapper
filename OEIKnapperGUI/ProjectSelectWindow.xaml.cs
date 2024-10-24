using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Drawing;
using System.Windows.Interop;
using OEIKnapper;

namespace OEIKnapperGUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public struct ProjectData()
{
    public BitmapSource Icon { get; private set; }
    public string Name { get; private set; }
    public string Path { get; private set; }
    
    public ProjectData(BitmapSource icon, string name, string path) : this()
    {
        Icon = icon;
        Name = name;
        Path = path;
    }
};
public partial class ProjectSelectWindow : Window
{
    public ObservableCollection<ProjectData> Projects { get; set; } = [];
    
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
        
        Projects.Add(new ProjectData(iconBitmapImg, fname, path));
    }

    private void OpenProject_OnClick(object sender, RoutedEventArgs e)
    {
        if (projectList.SelectedItem == null) return;
        
        var project = (ProjectData) projectList.SelectedItem;
        var db = new Database();
        db.LoadProject(project.Path);
        var mainWindow = new MainWindow();
        mainWindow.Show();
        //this.Close();
    }
}