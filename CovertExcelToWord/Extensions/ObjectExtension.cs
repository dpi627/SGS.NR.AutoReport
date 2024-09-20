using System.Data;
using System.Reflection;

namespace CovertExcelToWord.Extensions;

public static class ObjectExtension
{
    public static Dictionary<string, object> ToDictionary(this object obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        var dictionary = new Dictionary<string, object>();

        foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = property.GetValue(obj);
            dictionary[property.Name] = value;
        }

        return dictionary;
    }

    //public static DataTable ToDataTable<T>(this T obj) where T : class
    //{
    //    if (obj == null)
    //        throw new ArgumentNullException(nameof(obj));

    //    var dataTable = new DataTable(typeof(T).Name);

    //    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //    foreach (var property in properties)
    //    {
    //        dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
    //    }

    //    var values = new object[properties.Length];
    //    for (int i = 0; i < properties.Length; i++)
    //    {
    //        values[i] = properties[i].GetValue(obj);
    //    }

    //    dataTable.Rows.Add(values);

    //    return dataTable;
    //}

    public static DataTable ToDataTable<T>(this IEnumerable<T> list) where T : class
    {
        if (list == null)
            throw new ArgumentNullException(nameof(list));

        var dataTable = new DataTable(typeof(T).Name);

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
        }

        foreach (var item in list)
        {
            var values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(item);
            }
            dataTable.Rows.Add(values);
        }

        return dataTable;
    }
}
