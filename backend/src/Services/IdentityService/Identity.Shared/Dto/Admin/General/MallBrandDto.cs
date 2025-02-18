using Cloud.Domain.Entities;
using System;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// Dto
    ///</summary>
    public class MallBrandDto : BaseEntity<long>
    {
        /// <summary>
        /// 品牌名称 
        ///</summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 品牌编号 
        ///</summary>
        public string BrandCode { get; set; }
        /// <summary>
        /// 品牌图片 
        ///</summary>
        public string BrandImg { get; set; }
        /// <summary>
        /// 品牌logo 
        ///</summary>
        public string BrandLogo { get; set; }
    }
}
