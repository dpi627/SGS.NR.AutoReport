namespace SGS.NR.AutoReport.Wpf.Messages;

public class ProgressMessage
{
    public double ProgressValue { get; }

    public ProgressMessage(double progressValue)
    {
        ProgressValue = progressValue;
    }
}
