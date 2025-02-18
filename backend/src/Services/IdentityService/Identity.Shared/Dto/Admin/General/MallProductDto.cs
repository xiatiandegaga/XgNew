using Cloud.Domain.Entities;
using Identity.Shared.Dto.Admin.Output;
using System.Collections.Generic;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// Dto
    ///</summary>
    public class MallProductDto : BaseEntity<long>
    {
        /// <summary>
        /// 产品名称 
        ///</summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 产品编号 
        ///</summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品简称 
        ///</summary>
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
        public string ProductMainImg { get; set; }
        /// <summary>
        /// 产品详情图 
        ///</summary>
        public string ProductDetailImg { get; set; }
        /// <summary>
        /// 推荐状态1：推荐 0：不推荐 
        ///</summary>
        public short RecommendStatus { get; set; }
        /// <summary>
        /// 单位名称 
        ///</summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 最低价格-单位:分 
        ///</summary>
        public int MinPrice { get; set; }
        /// <summary>
        /// 最高价格-单位:分 
        ///</summary>
        public int MaxPrice { get; set; }

        /// <summary>
        /// 内部最低价格-单位:分 
        ///</summary>
        public int InnerMinPrice { get; set; }
        /// <summary>
        /// 内部最高价格-单位:分 
        ///</summary>
        public int InnerMaxPrice { get; set; }
        /// <summary>
        /// 最低价格-单位:元
        ///</summary>
        public decimal MinPriceAmount { get; set; }
        /// <summary>
        /// 最高价格-单位:元
        ///</summary>
        public decimal MaxPriceAmount { get; set; }
        /// <summary>
        /// 内部最低价格-单位:元
        ///</summary>
        public decimal InnerMinPriceAmount { get; set; }
        /// <summary>
        /// 内部最高价格-单位:元
        ///</summary>
        public decimal InnerMaxPriceAmount { get; set; }
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
        public string Remark { get; set; }

        /// <summary>
        /// 卖点描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 最低价格sku的最大分期
        /// </summary>
        public NumberOfInstallmentOutput NumberOfInstallmentModel { get; set; }

        /// <summary>
        /// 内部员工最低价格sku的最大分期
        /// </summary>
        public NumberOfInstallmentOutput InnerNumberOfInstallmentModel { get; set; }


        public List<MallProductSkuDto> MallProductSkuDtos { get; set; }

        public List<MallProductAttrDto> MallProductAttrDtos { get; set; }
    }


}
