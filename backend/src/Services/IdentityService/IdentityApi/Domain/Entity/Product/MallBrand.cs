using Cloud.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Product
{
    /// <summary>
    /// 
    ///</summary>
    public class MallBrand : BaseEntity<long>
    {
        /// <summary>
        /// 品牌名称 
        ///</summary>
        [MaxLength(100)]
        public string BrandName { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 品牌编号 
        ///</summary>
        [MaxLength(100)]
        public string BrandCode { get; set; }
        /// <summary>
        /// 品牌图片 
        ///</summary>
        [MaxLength(500)]
        public string BrandImg { get; set; }
        /// <summary>
        /// 品牌logo 
        ///</summary>
        [MaxLength(100)]
        public string BrandLogo { get; set; }
    }
}
