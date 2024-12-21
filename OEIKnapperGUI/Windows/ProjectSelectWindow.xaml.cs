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
        Projects = new ObservableCollection<GameExeData>(GameRummager.RummageForGames());
        projectList.ItemsSource = Projects;
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
            Projects.Add(GameRummager.LoadProjectData(dialog.FileName));
        }
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