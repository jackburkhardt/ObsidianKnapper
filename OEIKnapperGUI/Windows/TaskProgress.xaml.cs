using System.Windows;

namespace OEIKnapperGUI.Windows;

public partial class TaskProgress : Window
{
    private Progress<Database.ProgressReport> ProgressReporter;
    public TaskProgress(Progress<Database.ProgressReport> progressReporter)
    {
        InitializeComponent();
        progressReporter.ProgressChanged += ProgressReporter_ProgressChanged;
    }
    
    private void ProgressReporter_ProgressChanged(object? sender, Database.ProgressReport e)
    {
        Dispatcher.Invoke(() =>
        {
            ProgressBar.Maximum = e.Total;
            ProgressBar.Value = e.Completed;
            CurrentTask.Text = $"({e.Completed}/{e.Total}) {e.CurrentTask}";
        });
        
        if (e.Completed == e.Total)
        {
            Close();
        }
    }
}