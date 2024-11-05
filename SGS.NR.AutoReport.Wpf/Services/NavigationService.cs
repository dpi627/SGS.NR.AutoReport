using System.Windows.Controls;

namespace SGS.NR.AutoReport.Wpf.Services;

public class NavigationService() : INavigationService
{
    private readonly Dictionary<string, Type> _pagesByKey = [];
    private Frame _mainFrame;

    public void SetFrame(Frame frame)
    {
        _mainFrame = frame;
    }

    public void Configure(string key, Type pageType)
    {
        if (_pagesByKey.ContainsKey(key))
            throw new ArgumentException($"The key {key} is already configured in NavigationService");

        _pagesByKey.Add(key, pageType);
    }

    public void NavigateTo(string pageKey)
    {
        if (!_pagesByKey.ContainsKey(pageKey))
            throw new ArgumentException($"No such page: {pageKey}. Did you forget to call NavigationService.Configure?");

        var type = _pagesByKey[pageKey];
        _mainFrame.Navigate(Activator.CreateInstance(type));
    }

    public void GoBack()
    {
        if (_mainFrame.CanGoBack)
            _mainFrame.GoBack();
    }
}
