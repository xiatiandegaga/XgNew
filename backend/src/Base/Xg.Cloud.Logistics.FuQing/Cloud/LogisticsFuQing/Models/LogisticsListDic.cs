using System.Collections.Generic;

namespace Cloud.LogisticsFuQing.Models
{
    public class LogisticsListDic
    {
        public static Dictionary<string, string> DicFuQingNoQueryDeliveryStatus = new Dictionary<string, string>
        {
            {"0","快递揽件"},
            {"1","在途中"},
            {"2","正在派件"},
            {"3","已签收"},
            {"4","派送失败"},
            {"5","疑难件"},
            {"6","退件签收"},
        };
    }
}
