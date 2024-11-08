using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SGS.NR.AutoReport.Wpf.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace SGS.NR.AutoReport.Wpf.ViewModels;

public partial class ExportDraftViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ExcelFile> _excelFiles = [];

    [RelayCommand]
    public void ImportExcel()
    {
        Debug.WriteLine("ImportExcel command executed");

        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel Files|*.xlsx",
            Title = "Select Excel Files",
            Multiselect = true
        };

        bool? result = openFileDialog.ShowDialog();

        if (!result.HasValue || !result.Value)
            return;

        var files = openFileDialog.FileNames;
        foreach (var file in files)
        {
            Debug.WriteLine(file);

            ExcelFiles.Add(new ExcelFile
            {
                FileName = Path.GetFileName(file),
                FilePath = file
            });
        }
    }
}
