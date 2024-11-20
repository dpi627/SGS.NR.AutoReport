using System.Windows.Controls;

namespace SGS.NR.AutoReport.Wpf.Controls;

public class CustomThreeStateCheckBox : CheckBox
{
    protected override void OnToggle()
    {
        if (IsChecked == null)
        {
            IsChecked = true;
        }
        else if (IsChecked == true)
        {
            IsChecked = false;
        }
        else
        {
            IsChecked = true;
        }
    }
}

