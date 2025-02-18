using Cloud.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Product
{
    /// <summary>
    /// 产品目录表
    ///</summary>
    public class MallProductCategory : BaseEntity<long>
    {
        /// <summary>
        /// 目录名称
        ///</summary>
        [MaxLength(100)]
        public string CategoryName { get; set; }
        /// <summary>
        /// 父id 
        ///</summary>
        public long Pid { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 目录图片 
        ///</summary>
        [MaxLength(1000)]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 目录编号 
        ///</summary>
        [MaxLength(100)]
        public string CategoryCode { get; set; }
        /// <summary>
        /// 商品类型id
        /// </summary>
        public long MallProductTypeId { get; set; }
    }
}
