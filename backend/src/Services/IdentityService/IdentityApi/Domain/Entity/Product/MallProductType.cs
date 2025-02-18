using Cloud.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Product
{
    /// <summary>
    /// 产品类型表
    ///</summary>
    public class MallProductType : BaseEntity<long>
    {
        /// <summary>
        /// 类型名称 
        ///</summary>
        [MaxLength(100)]
        public string TypeName { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 类型编号 
        ///</summary>
        [MaxLength(100)]
        public string TypeCode { get; set; }
    }
}
