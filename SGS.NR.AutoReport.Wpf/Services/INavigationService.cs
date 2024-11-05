using System.Windows.Controls;

namespace SGS.NR.AutoReport.Wpf.Services;

public interface INavigationService
{
    void NavigateTo(string pageKey);
    void GoBack();
    void SetFrame(Frame frame);
    void Configure(string key, Type pageType);
}
