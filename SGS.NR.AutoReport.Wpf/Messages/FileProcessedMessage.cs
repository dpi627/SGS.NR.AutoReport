using SGS.NR.AutoReport.Wpf.Models;

namespace SGS.NR.AutoReport.Wpf.Messages;

public class FilesProcessedMessage
{
    public List<ExcelFile> ProcessedFiles { get; }

    public FilesProcessedMessage(List<ExcelFile> processedFiles)
    {
        ProcessedFiles = processedFiles;
    }
}

