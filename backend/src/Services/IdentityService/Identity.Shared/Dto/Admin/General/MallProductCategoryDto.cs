using Cloud.Domain.Entities;
using System;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// 产品目录表Dto
    ///</summary>
    public class MallProductCategoryDto : BaseEntity<long>
    {
        /// <summary>
        /// 目录名称
        ///</summary>
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
        public string ImageUrl { get; set; }
        /// <summary>
        /// 目录编号 
        ///</summary>
        public string CategoryCode { get; set; }
        /// <summary>
        /// 商品类型id
        /// </summary>
        public long MallProductTypeId { get; set; }
    }
}
