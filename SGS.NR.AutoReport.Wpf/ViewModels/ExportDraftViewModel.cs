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
        _messenger.Send(new LoadingMessage(true));

        try
        {
            await Task.Run(() =>
            {
                CheckDirectoryExist(_targetFileDirectory);
                string targetFileName;
                foreach (var excelFile in ExcelFiles)
                {
                    if (!excelFile.IsChecked)
                        continue;
                    Task.Delay(1000).Wait();
                    targetFileName = Path.GetFileNameWithoutExtension(excelFile.FileName);
                    var draftInfo = new DraftInfo
                    {
                        SourcePath = excelFile.FilePath,
                        TemplatePath = SelectedDraftTempFile?.FilePath,
                        TargetPath = Path.Combine(_targetFileDirectory, $"{targetFileName}.docx")
                    };

                    ExportDraft(draftInfo);
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
            _messenger.Send(new LoadingMessage(false));
            _dialog.ShowMessage("報告草稿(Word)製作完成");
        }
    }

    private static void CheckDirectoryExist(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private void ExportDraft(DraftInfo draftInfo)
    {
        _logger.LogInformation("Export Draft: {@DraftInfo}", draftInfo);

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
}
