using Cloud.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Dictionary
{
    /// <summary>
    /// 数据字典主表
    /// </summary>
    public class GlobalData : BaseEntity<long>
    {
        /// <summary>
        /// 编号
        ///</summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNo { get; set; }
    }
}
