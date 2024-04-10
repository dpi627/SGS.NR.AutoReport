namespace SGS.NR.Util.Extension
{
    public static class IDictionaryExtension
    {
        /// <summary>
        /// 合併兩個字典。
        /// 如有重複 key，後者會覆蓋前者 value
        /// </summary>
        /// <param name="dict1">第一個字典。</param>
        /// <param name="dict2">第二個字典。</param>
        /// <returns>合併後的字典。</returns>
        public static IDictionary<string, object> Merge(
            this IDictionary<string, object> dict1,
            IDictionary<string, object> dict2
            )
        {
            return dict1.Concat(dict2)
                .GroupBy(pair => pair.Key)
                .ToDictionary(
                group => group.Key,
                group => group.Last().Value
                );
        }

        /// <summary>
        /// 將集合添加到字典中。
        /// 將 IEnumerable<T> 轉換為 IDictionary<string, object> 後，添加到字典中。
        /// </summary>
        /// <typeparam name="T">集合中的元素類型。</typeparam>
        /// <param name="source">要添加到的字典。</param>
        /// <param name="collection">要添加的集合。</param>
        /// <param name="collectionName">集合的名稱。</param>
        /// <returns>添加集合後的字典。</returns>
        public static IDictionary<string, object> AddCollection<T>(
            this IDictionary<string, object> source,
            IEnumerable<T> collection,
            string collectionName = "Loop"
        )
        {
            source.Add(collectionName, collection.ToDict());
            return source;
        }
    }
}
