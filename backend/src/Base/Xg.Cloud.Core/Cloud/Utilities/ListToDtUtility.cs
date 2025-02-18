using Cloud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Cloud.Utilities
{
    public static class ListToDtUtility
    {

        /// <summary>
        /// 泛型集合转DataTable  注意:DataTable不支持可空值类型如int? datetime?等 必须判断转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> array)
        {
            var ret = new DataTable();
            Type colType;
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
            {
                colType = pd.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                ret.Columns.Add(pd.Name, colType);
            }
            foreach (T item in array)
            {
                var Row = ret.NewRow();
                foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
                    Row[pd.Name] = pd.GetValue(item) ?? DBNull.Value;
                ret.Rows.Add(Row);
            }
            return ret;
        }

        public static DataTable DataTableColumnListCopyToDataTable(this List<List<DataTableColumnModel>> list)
        {
            if (list == default || list.Count == 0 || list[0] == default || list[0].Count == 0)
                throw new MyException("结果无数据！",0);
            var ret = new DataTable();
            foreach (var item in list[0])
            {
                ret.Columns.Add(item.FieldName);
            }
            foreach (List<DataTableColumnModel> item in list)
            {
                var Row = ret.NewRow();
                foreach (var pd in item)
                {
                    if (ret.Columns.Contains(pd.FieldName))
                        Row[pd.FieldName] = pd.FieldValue ?? DBNull.Value;
                }
                ret.Rows.Add(Row);
            }
            return ret;
        }


    }
}
