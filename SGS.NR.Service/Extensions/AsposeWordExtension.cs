using Aspose.Words;

namespace SGS.NR.Service.Extensions;

public static class AsposeWordExtension
{
    public static void RemoveRowsByBookmark(this Document doc, string bookmarkName)
    {
        // 獲取指定的書籤
        Bookmark bookmark = doc.Range.Bookmarks[bookmarkName];

        if (bookmark != null)
        {
            // 獲取書籤所在的儲存格
            Aspose.Words.Tables.Cell cell = (Aspose.Words.Tables.Cell)bookmark.BookmarkStart.GetAncestor(NodeType.Cell);

            if (cell != null)
            {
                // 獲取儲存格所在的行
                Aspose.Words.Tables.Row currentRow = cell.ParentRow;

                if (currentRow != null)
                {
                    // 獲取下一行
                    Aspose.Words.Tables.Row nextRow = (Aspose.Words.Tables.Row)currentRow.NextSibling;

                    // 移除當前行
                    currentRow.Remove();

                    // 如果存在下一行，也將其移除
                    nextRow?.Remove();
                }
            }
        }
    }
}
