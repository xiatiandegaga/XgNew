using Cloud.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Product
{
    /// <summary>
    /// 产品属性表值value
    ///</summary>
    public class MallProductAttrValue : BaseEntity<long>
    {
        /// <summary>
        /// 属性值名 
        ///</summary>
        [MaxLength(100)]
        public string AttrValueName { get; set; }
        /// <summary>
        /// 属性名id 
        ///</summary>
        public long MallProductAttrKeyId { get; set; }
        /// <summary>
        /// 排序 
        ///</summary>
        public int SortNo { get; set; }

    }
}
