using Cloud.Utilities.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloud.Utilities
{
    public static class ApiSignUtility
    {
        public static string GetSortParam(string req,string blackList=null)
        {
            var dicReq = JsonUtility.Deserialize<Dictionary<string, object>>(req);
            dicReq = dicReq.OrderBy(m => m.Key, StringComparer.Ordinal).ToDictionary(pair => pair.Key, pair => pair.Value);//升序 把Key换成Value 就是对Value进行排序                                                                           //keyValues.OrderByDescending(m => m.Key);//降序
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, object> kv in dicReq)
            {
                if (kv.Value == null)
                    continue;
                if(blackList!=default && blackList.Split(',').Contains(kv.Key))
                    continue;
                string pkey = kv.Key;
                object pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }
            return str.ToString()[0..^1];
        }
    }
}
