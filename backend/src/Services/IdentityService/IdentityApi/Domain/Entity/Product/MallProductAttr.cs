using Cloud.Domain.Entities;

namespace Domain.Entity.Product
{
    public class MallProductAttr : BaseEntity<long>
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public long MallProductId { get; set; } 
        /// <summary>
        /// 属性id
        /// </summary>
        public long MallProductAttrKeyId { get; set; }
        /// <summary>
        /// 属性值id
        /// </summary>
        public long MallProductAttrValueId { get; set; }
    }
}
