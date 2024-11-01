using System.Reflection;

namespace SGS.NR.Util.Helper;

public class VersionHelper
{
    private static readonly string? _isClickOnce = Environment.GetEnvironmentVariable("ClickOnce_IsNetworkDeployed");
    private static readonly string? _clickOnceVersion = Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion");
    private static readonly string _defaultVersion = "0.0.0.0";

    public static string CurrentVersion => GetApplicationVersion();

    private static string GetApplicationVersion()
    {
        try
        {
            if (!string.IsNullOrEmpty(_isClickOnce) && bool.Parse(_isClickOnce))
            {
                return _clickOnceVersion ?? _defaultVersion;
            }
            else
            {
                // 使用 GetEntryAssembly 來獲取主程式的組件
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly != null)
                {
                    var version = entryAssembly.GetName().Version;
                    return version != null ? version.ToString() : _defaultVersion;
                }
                else
                {
                    // 如果無法獲取主程式的組件，則回傳預設版本
                    return _defaultVersion;
                }
            }
        }
        catch (Exception)
        {
            return _defaultVersion;
        }
    }
}
