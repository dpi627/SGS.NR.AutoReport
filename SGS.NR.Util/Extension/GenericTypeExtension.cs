using System.Reflection;

namespace SGS.NR.Util.Extension
{
    /// <summary>
    /// 提供用於處理泛型的擴展方法。
    /// </summary>
    public static class GenericTypeExtension
    {
        /// <summary>
        /// 將指定的模型物件轉換為字典。
        /// </summary>
        /// <typeparam name="T">模型物件的類型。</typeparam>
        /// <param name="model">要轉換的模型物件。</param>
        /// <returns>表示模型物件的字典。</returns>
        public static IDictionary<string, object> ToDict<T>(this T model)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            Dictionary<string, object>? dict = [];
            foreach (PropertyInfo prop in props)
                dict.Add(prop.Name, prop.GetValue(model));
            return dict;
        }
    }
}