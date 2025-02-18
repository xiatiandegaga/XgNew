using Cloud.Domain.Entities;
using System;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// 产品属性表keyDto
    ///</summary>
    public class MallProductAttrKeyDto : BaseEntity<long>
    {
        /// <summary>
        /// 属性名 
        ///</summary>
        public string AttrKeyName { get; set; }
        /// <summary>
        /// 商品类型id 
        ///</summary>
        public long MallProductTypeId { get; set; }
        /// <summary>
        /// 商品目录id 
        ///</summary>
        public long MallProductCategoryId { get; set; }
        /// <summary>
        /// 排序 
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 创建日期 
        ///</summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 创建人 
        ///</summary>
        public long CreateId { get; set; }

        /// <summary>
        /// 状态（1启用 0禁用）
        /// </summary>
        public int Status { get; set; }
    }
}
