namespace Cloud.LogisticsFuQing.Models
{
    public class LogisticsRequest
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求的物流类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 请求的物流单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 请求的物流权限
        /// </summary>
        public string Auth { get; set; }
    }
}
