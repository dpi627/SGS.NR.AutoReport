using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapsterMapper;
using Microsoft.Win32;
using SGS.NR.AutoReport.Wpf.Models;
using SGS.NR.AutoReport.Wpf.Services;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.Interface;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace SGS.NR.AutoReport.Wpf.ViewModels;

public partial class ExportDraftViewModel : ObservableObject
{
    private readonly IDialogService _dialog;
    private readonly IMapper _mapper;
    private IContainerLoadingService _serviceCL;
    private IVesselLoadingService _serviceVL;

    [ObservableProperty]
    private ObservableCollection<ExcelFile> _excelFiles = [];

    [ObservableProperty]
    private ObservableCollection<DraftTempFile> _draftTempFiles = [];

    [ObservableProperty]
    private DraftTempFile? _selectedDraftTempFile;

    public ExportDraftViewModel(
        IContainerLoadingService serviceCL,
        IVesselLoadingService serviceVL,
        IMapper mapper,
        IDialogService dialog)
    {
        _dialog = dialog;
        _mapper = mapper;
        _serviceCL = serviceCL;
        _serviceVL = serviceVL;

        GetDraftTemplate();

        if (DraftTempFiles.Any())
            SelectedDraftTempFile = DraftTempFiles.First();
    }

    [RelayCommand]
    public void ImportExcel()
    {
        Debug.WriteLine("ImportExcel command executed");

        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel Files|*.xlsm",
            Title = "Select Excel Files (*.xlsm)",
            Multiselect = true
        };

        bool? result = openFileDialog.ShowDialog();

        if (!result.HasValue || !result.Value)
            return;

        var files = openFileDialog.FileNames;
        foreach (var file in files)
        {
            //Debug.WriteLine(file);

            ExcelFiles.Add(new ExcelFile
            {
                FileName = Path.GetFileName(file),
                FilePath = file,
                IsChecked = true
            });
        }
    }

    [RelayCommand]
    public void ExportWord()
    {
        foreach (var excelFile in ExcelFiles)
        {
            if (!excelFile.IsChecked)
                continue;

            //Debug.WriteLine(excelFile.FilePath);
            try {
                var DraftInfo = new DraftInfo
                {
                    SourcePath = excelFile.FilePath,
                    TemplatePath = SelectedDraftTempFile?.FilePath,
                    TargetPath = $@"C:\dev\_tmp\{DateTime.Now:yyyyMMddHHmmssfff}.docx"
                };
                ExportDraft(DraftInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        _dialog.ShowMessage("報告草稿(Word)製作完成");
    }

    private void ExportDraft(DraftInfo draftInfo)
    {
        if(draftInfo.TemplatePath.Contains("Draft.Container.Load.docx"))
        {
            var clInfo = _mapper.Map<ContainerLoadingInfo>(draftInfo);
            _ = _serviceCL.GetDraft(clInfo);
        }
        else if (draftInfo.TemplatePath.Contains("Draft.Vessel.Load.docx"))
        {
            var vlInfo = _mapper.Map<VesselLoadingInfo>(draftInfo);
            _ = _serviceVL.GetDraft(vlInfo);
        }
    }

    public void GetDraftTemplate()
    {
        Debug.WriteLine("GetDraftTemplate command executed");

        var templateDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
        if (!Directory.Exists(templateDirectory))
        {
            Debug.WriteLine("Templates directory does not exist.");
            return;
        }

        var wordFiles = Directory.GetFiles(templateDirectory, "*.docx");
        foreach (var file in wordFiles)
        {
            Debug.WriteLine(file);

            DraftTempFiles.Add(new DraftTempFile
            {
                FileName = Path.GetFileName(file),
                FilePath = file
            });
        }
    }
}
