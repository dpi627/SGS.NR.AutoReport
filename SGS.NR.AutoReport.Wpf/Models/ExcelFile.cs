#nullable disable

using CommunityToolkit.Mvvm.ComponentModel;

namespace SGS.NR.AutoReport.Wpf.Models;

public partial class ExcelFile : ObservableObject
{
    [ObservableProperty]
    private string fileName;

    [ObservableProperty]
    private string filePath;

    [ObservableProperty]
    private bool isChecked;
}
