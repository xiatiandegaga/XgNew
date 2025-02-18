using Cloud.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Product
{
    /// <summary>
    /// 
    ///</summary>
    public class MallProduct : BaseEntity<long>
    {
        /// <summary>
        /// 产品名称 
        ///</summary>
        [MaxLength(100)]
        public string ProductName { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 产品编号 
        ///</summary>
        [MaxLength(100)]
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品简称 
        ///</summary>
        [MaxLength(100)]
        public string ProductShortName { get; set; }
        /// <summary>
        /// 品牌id 
        ///</summary>
        public long BrandId { get; set; }
        /// <summary>
        /// 产品目录ID 
        ///</summary>
        public long MallProductCategoryId { get; set; }
        /// <summary>
        /// 产品类型ID 
        ///</summary>
        public long MallProductTypeId { get; set; }
        /// <summary>
        /// 产品主图 
        ///</summary>
        [MaxLength(1000)]
        public string ProductMainImg { get; set; }
        /// <summary>
        /// 产品详情图 
        ///</summary>
        [MaxLength(1000)]
        public string ProductDetailImg { get; set; }
        /// <summary>
        /// 推荐状态1：推荐 0：不推荐 
        ///</summary>
        public short RecommendStatus { get; set; }
        /// <summary>
        /// 单位名称 
        ///</summary>
        [MaxLength(100)]
        public string UnitName { get; set; }

        /// <summary>
        /// 上架状态 1：上架 0：下架 
        ///</summary>
        public short Status { get; set; }
        /// <summary>
        /// 新品状态 1：新品 0：非新品 
        ///</summary>
        public short NewStatus { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        [MaxLength(500)]
        public string Remark { get; set; }
        /// <summary>
        /// 卖点描述
        ///</summary>
        [MaxLength(500)]
        public string Desc { get;set; }
    }
}
