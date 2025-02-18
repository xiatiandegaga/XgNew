using Cloud.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Product
{
    /// <summary>
    /// 产品属性表key
    ///</summary>
    public class MallProductAttrKey : BaseEntity<long>
    {
        /// <summary>
        /// 属性名 
        ///</summary>
        [MaxLength(100)]
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

    }
}
