namespace Cloud.Models
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 每页行数(默认10)
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 当前页（默认1）
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 排序列[字段 Descending表示倒序排列，字段表示正序排列]
        /// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// 记录数量[不用传值]
        /// </summary>
        public long Count { get; set; }

    }
}
