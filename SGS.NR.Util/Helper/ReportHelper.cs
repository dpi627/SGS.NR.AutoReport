using SGS.NR.Util.Extension;

namespace SGS.NR.Util.Helper
{
    public class ReportHelper
    {
        /// <summary>
        /// 組合一次性資料與集合資料，並轉換為字典
        /// </summary>
        /// <typeparam name="TSingleton">單一物件類型</typeparam>
        /// <typeparam name="TCollection">集合物件類型</typeparam>
        /// <param name="Singleton">單一物件，儲存一次性資料</param>
        /// <param name="Collection">集合物件，儲存集合資料，通常以表格呈現</param>
        /// <param name="CollectionName">集合名稱，預設 "Loop"</param>
        /// <returns>結合後的字典</returns>
        public static IDictionary<string, object> CombineData<TSingleton, TCollection>(
            TSingleton Singleton,
            IEnumerable<TCollection> Collection,
            string CollectionName = "Loop")
        {
            IDictionary<string, object>? result = Singleton.ToDict();
            result.Add(CollectionName, Collection.ToDict());
            return result;
        }
    }
}
