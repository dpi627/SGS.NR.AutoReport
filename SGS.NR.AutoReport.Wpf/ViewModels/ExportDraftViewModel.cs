using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SGS.NR.AutoReport.Wpf.Models;
using SGS.NR.AutoReport.Wpf.Services;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.Interface;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace SGS.NR.AutoReport.Wpf.ViewModels;

public partial class ExportDraftViewModel : ObservableObject
{
    private readonly IDialogService _dialog;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private IContainerLoadingService _serviceCL;
    private IVesselLoadingService _serviceVL;
    private readonly IMessenger _messenger;
    private string _targetFileDirectory = @"C:\SGSLIMS\NR";

    [ObservableProperty]
    private ObservableCollection<ExcelFile> _excelFiles = [];

    [ObservableProperty]
    private ObservableCollection<DraftTempFile> _draftTempFiles = [];

    [ObservableProperty]
    private DraftTempFile? _selectedDraftTempFile;

    [ObservableProperty]
    private double _progressValue;

    [ObservableProperty]
    private bool _isExporting;

    public ExportDraftViewModel(
        IContainerLoadingService serviceCL,
        IVesselLoadingService serviceVL,
        IMapper mapper,
        IDialogService dialog,
        IMessenger messenger,
        ILogger<ExportDraftViewModel> logger)
    {
        _dialog = dialog;
        _mapper = mapper;
        _logger = logger;
        _serviceCL = serviceCL;
        _serviceVL = serviceVL;
        _messenger = messenger;

        GetDraftTemplate();

        if (DraftTempFiles.Any())
            SelectedDraftTempFile = DraftTempFiles.First();
    }

    [RelayCommand]
    private void ImportExcel()
    {
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
            ExcelFiles.Add(new ExcelFile
            {
                FileName = Path.GetFileName(file),
                FilePath = file,
                IsChecked = true
            });
        }
        _logger.LogInformation("Import Excel: {@Excels}", ExcelFiles);
    }

    [RelayCommand]
    private async Task ExportWordAsync()
    {
        // 計算要處理的檔案總數
        var filesToProcess = ExcelFiles.Count(x => x.IsChecked);
        
        if (!ExcelFiles.Any())
        {
            _dialog.ShowMessage("請先匯入檔案");
            return;
        }

        if (filesToProcess == 0)
        {
            _dialog.ShowMessage("請勾選至少一筆資料");
            return;
        }

        IsExporting = true;
        ProgressValue = 0;
        _messenger.Send(new LoadingMessage(true));

        try
        {
            await Task.Run(() =>
            {
                CheckDirectoryExist(_targetFileDirectory);
                string targetFileName;

                var currentFile = 0;
                foreach (var excelFile in ExcelFiles)
                {
                    if (!excelFile.IsChecked)
                        continue;

                    targetFileName = Path.GetFileNameWithoutExtension(excelFile.FileName);
                    var draftInfo = new DraftInfo
                    {
                        SourcePath = excelFile.FilePath,
                        TemplatePath = SelectedDraftTempFile?.FilePath,
                        TargetPath = Path.Combine(_targetFileDirectory, $"{targetFileName}.docx")
                    };

                    _logger.LogInformation("#{Seq} From {Excel} to {Word}", currentFile, draftInfo.SourcePath, draftInfo.TargetPath);
                    ExportDraft(draftInfo);

                    // 更新進度
                    currentFile++;
                    ProgressValue = (double)currentFile / filesToProcess * 100;
                }
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            ProgressValue = 100;
            _messenger.Send(new LoadingMessage(false));
            _dialog.ShowMessage("報告草稿製作完成");
            await Task.Delay(1000);
            IsExporting = false;
        }
    }

    private static void CheckDirectoryExist(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private void ExportDraft(DraftInfo draftInfo)
    {
        _logger.LogInformation("Source Excel: {Excel}", draftInfo.SourcePath);

        if (draftInfo.TemplatePath.Contains("Draft.Container.Load.docx"))
        {
            var clInfo = _mapper.Map<ContainerLoadingInfo>(draftInfo);
            _ = _serviceCL.GetDraft(clInfo);
        }
        else if (draftInfo.TemplatePath.Contains("Draft.Vessel.Load.docx"))
        {
            var vlInfo = _mapper.Map<VesselLoadingInfo>(draftInfo);
            _ = _serviceVL.GetDraft(vlInfo);
        }

        _logger.LogInformation("Target Word: {Word}", draftInfo.TargetPath);
    }

    public void GetDraftTemplate()
    {
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
        _logger.LogInformation("Draft Temp Files: {@DraftTempFiles}", DraftTempFiles);
    }

    [RelayCommand]
    private void OpenExportDirectory()
    {
        try
        {
            if (Directory.Exists(_targetFileDirectory))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _targetFileDirectory,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                _dialog.ShowMessage("The directory does not exist.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to open the export directory.");
            _dialog.ShowMessage("Failed to open the export directory.");
        }
    }

    // add command ClearDataGridCommand to clean griddata
    [RelayCommand]
    private void ClearDataGrid()
    {
        ExcelFiles.Clear();
    }
}
