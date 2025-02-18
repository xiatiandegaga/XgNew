namespace Cloud.Models
{
    /// <summary>
    /// 分页查询数据集
    /// </summary>
    public class PagingData<T>
    {
        /// <summary>
        /// 返回分页数据里的当前页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 返回分页数据里的总数量
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 返回分页数据里的集合
        /// </summary>
        public T List { get; set; }
    }
}
