using System;

namespace Cloud.LogisticsFuQing.Models
{
    public class FuQingNoQueryResultResponseDetail
    {
        public DateTime Time { get; set; }

        /// <summary>
        /// 快递当前节点明细状态（【石家庄市】 快件离开 【石家庄】 发往 【长安三部】）
        /// </summary>
        public string Status { get; set; }
    }
}
