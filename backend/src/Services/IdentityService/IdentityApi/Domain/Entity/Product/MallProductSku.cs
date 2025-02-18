using Cloud.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Product
{
    /// <summary>
    /// 
    ///</summary>
    public class MallProductSku : BaseEntity<long>
    {
        /// <summary>
        /// Sku名称 
        ///</summary>
        [MaxLength(100)]
        public string SkuName { get; set; }
        /// <summary>
        /// 排序序号（数值越小越靠前）
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// sku编号 
        ///</summary>
        [MaxLength(100)]
        public string SkuCode { get; set; }
        /// <summary>
        /// 产品id 
        ///</summary>
        public long MallProductId { get; set; }
        /// <summary>
        /// sku价格，单位分 
        ///</summary>
        public int SkuPrice { get; set; }
        /// <summary>
        /// sku内部价格，单位分 
        ///</summary>
        public int SkuInnerPrice { get; set; }
        /// <summary>
        /// sku图片 
        ///</summary>
        [MaxLength(1000)]
        public string SkuImg { get; set; }
        /// <summary>
        /// sku库存 
        ///</summary>
        public int SkuStock { get; set; }
        /// <summary>
        /// 冻结库存 
        ///</summary>
        public int FreezeStock { get; set; }
        /// <summary>
        /// 属性字符串，json字符串格式 
        ///</summary>
        [MaxLength(3000)]
        public string AttrKeyValue { get; set; }
        /// <summary>
        /// 规格参数 json字符串格式
        ///</summary>
        [MaxLength(3000)]
        public string SpecParam { get; set; }

        /// <summary>
        /// 状态（1启用 0停用）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 分期付款期数，多个以英文逗号(,)隔开
        ///</summary>
        [MaxLength(100)]
        public string NumberOfInstallments { get; set; }

        /// <summary>
        /// 内部人员分期付款期数，多个以英文逗号(,)隔开
        ///</summary>
        [MaxLength(100)]
        public string InnerNumberOfInstallments { get; set; }
    }
}
