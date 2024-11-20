using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SGS.NR.AutoReport.Wpf.Messages;
using SGS.NR.AutoReport.Wpf.Models;
using SGS.NR.AutoReport.Wpf.Services;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.Interface;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

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

    private bool _isUpdatingAllSelected = false;

    [ObservableProperty]
    private bool? _allSelected = false;

    partial void OnAllSelectedChanged(bool? value)
    {
        if (_isUpdatingAllSelected)
            return;

        _isUpdatingAllSelected = true;

        try
        {
            if (value == true || value == false)
            {
                foreach (var item in ExcelFiles)
                {
                    item.IsChecked = value.Value;
                }
            }
        }
        finally
        {
            _isUpdatingAllSelected = false;
        }
    }

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

        // 註冊接收器
        _messenger.Register<FilesProcessedMessage>(this, (r, m) =>
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RemoveProcessedFiles(m.ProcessedFiles);
            });
        });

        _messenger.Register<ProgressMessage>(this, (r, m) =>
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressValue = m.ProgressValue;
            });
        });

        ExcelFiles.CollectionChanged += ExcelFiles_CollectionChanged;
    }

    private void ExcelFiles_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (ExcelFile item in e.NewItems)
            {
                item.PropertyChanged += ExcelFile_PropertyChanged;
            }
        }

        if (e.OldItems != null)
        {
            foreach (ExcelFile item in e.OldItems)
            {
                item.PropertyChanged -= ExcelFile_PropertyChanged;
            }
        }

        UpdateAllSelected();
    }

    private void RemoveProcessedFiles(List<ExcelFile> processedFiles)
    {
        foreach (var file in processedFiles)
        {
            ExcelFiles.Remove(file);
        }
        UpdateAllSelected();
    }
    private void UpdateAllSelected()
    {
        if (!ExcelFiles.Any())
        {
            AllSelected = false;
            return;
        }

        if (ExcelFiles.All(f => f.IsChecked))
        {
            AllSelected = true;
        }
        else if (ExcelFiles.All(f => !f.IsChecked))
        {
            AllSelected = false;
        }
        else
        {
            AllSelected = null;
        }
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
            var excelFile = new ExcelFile
            {
                FileName = Path.GetFileName(file),
                FilePath = file,
                IsChecked = true
            };

            // 订阅 PropertyChanged 事件
            excelFile.PropertyChanged += ExcelFile_PropertyChanged;

            ExcelFiles.Add(excelFile);
        }
        UpdateAllSelected();
        _logger.LogInformation("Import Excel: {@Excels}", ExcelFiles);
    }

    private void ExcelFile_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ExcelFile.IsChecked))
        {
            UpdateAllSelected();
        }
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
                // 收集已處理的檔案
                var processedFiles = new List<ExcelFile>();
                // 複製一份要處理的檔案清單，避免操作過程遭到修改
                var files = ExcelFiles.ToList();
                foreach (var excelFile in files)
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

                    // 加入已處理的檔案清單
                    processedFiles.Add(excelFile);

                    // 更新進度
                    currentFile++;
                    ProgressValue = (double)currentFile / filesToProcess * 100;
                    // 傳遞進度更新消息
                    _messenger.Send(new ProgressMessage(ProgressValue));
                }

                // 傳遞已處理的檔案列表
                _messenger.Send(new FilesProcessedMessage(processedFiles));
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "匯出異常：{Message}", ex.Message);
            _dialog.ShowMessage("匯出異常：" + ex.Message);
        }
        finally
        {
            ProgressValue = 100;
            _messenger.Send(new LoadingMessage(false));
            _dialog.ShowMessage("報告草稿製作完成");
            //await Task.Delay(1000);
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
        UpdateAllSelected();
        ExcelFiles.Clear();
    }
}
