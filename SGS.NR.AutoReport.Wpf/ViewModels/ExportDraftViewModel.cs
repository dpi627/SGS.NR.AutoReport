using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Diagnostics;

namespace SGS.NR.AutoReport.Wpf.ViewModels;

public partial class ExportDraftViewModel : ObservableObject
{
    [RelayCommand]
    public void ImportExcel()
    {
        Debug.WriteLine("ImportExcel command executed");

        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel Files|*.xlsx",
            Title = "Select an Excel file",
            Multiselect = true
        };

        bool? result = openFileDialog.ShowDialog();

        if (!result.HasValue || !result.Value)
            return;

        var files = openFileDialog.FileNames;
        foreach (var file in files)
        {
            Debug.WriteLine(file);
        }
    }
}
