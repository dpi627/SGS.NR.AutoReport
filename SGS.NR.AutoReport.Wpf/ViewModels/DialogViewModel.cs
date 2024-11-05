using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using System.Windows.Threading;

namespace SGS.NR.AutoReport.Wpf.ViewModels;

public partial class DialogViewModel : ObservableObject
{
    private readonly DispatcherTimer _timer;
    private double _remainingSeconds = 3;

    [ObservableProperty]
    private string _message;

    [ObservableProperty]
    private string _okButtonText;

    [ObservableProperty]
    private string _cancelButtonText;

    [ObservableProperty]
    private bool _isOkButtonVisible;

    [ObservableProperty]
    private bool _isCancelButtonVisible;

    [ObservableProperty]
    private bool _isAutoClose;

    [ObservableProperty]
    private int _countdownProgress;

    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public DialogViewModel(
        string message,
        string okButtonText = "OK",
        string cancelButtonText = "Cancel",
        bool showCancelButton = false,
        bool isAutoClose = false)
    {
        Message = message;
        OkButtonText = okButtonText;
        CancelButtonText = cancelButtonText;
        IsOkButtonVisible = true;
        IsCancelButtonVisible = showCancelButton;
        IsAutoClose = isAutoClose;

        OkCommand = new RelayCommand(() => CloseDialog(true));
        CancelCommand = new RelayCommand(() => CloseDialog(false));

        if (IsAutoClose)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        _remainingSeconds -= 0.016;
        CountdownProgress = (int)((3 - _remainingSeconds) / 3.0 * 100);
        if (_remainingSeconds <= 0)
        {
            _timer.Stop();
            CloseDialogOk();
        }
    }

    private void CloseDialogOk()
    {
        _timer?.Stop();
        CloseDialog(true);
    }

    private void CloseDialogCancel()
    {
        _timer?.Stop();
        // Implement dialog closing logic here
        CloseDialog(true);
    }

    private void CloseDialog(bool result)
    {
        if (DialogHost.IsDialogOpen("RootDialog"))
            DialogHost.Close("RootDialog", result);
    }
}
