namespace SGS.NR.Util.Extension
{
    /// <summary>
    /// 提供用於處理可列舉集合的擴展方法。
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 將可列舉集合轉換為字典列表。
        /// </summary>
        /// <typeparam name="T">可列舉集合中元素的類型。</typeparam>
        /// <param name="list">要轉換的可列舉集合。</param>
        /// <returns>表示可列舉集合中元素的字典列表。</returns>
        public static IEnumerable<IDictionary<string, object>> ToDict<T>(this IEnumerable<T> list)
        {
            List<IDictionary<string, object>>? result = [];
            foreach (T record in list)
                result.Add(record.ToDict());
            return result;
        }
    }
}
