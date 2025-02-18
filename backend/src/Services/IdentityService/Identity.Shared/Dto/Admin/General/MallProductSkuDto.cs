using Cloud.Domain.Entities;
using Identity.Shared.Dto.Admin.Output;
using System;
using System.Collections.Generic;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// Dto
    ///</summary>
    public class MallProductSkuDto : BaseEntity<long>
    {
        /// <summary>
        /// Sku名称 
        ///</summary>
        public string SkuName { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// sku编号 
        ///</summary>
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
        /// sku价格，单位元
        ///</summary>
        public decimal SkuPriceAmount { get; set; }
        /// <summary>
        /// sku内部价格，单位元
        ///</summary>
        public decimal SkuInnerPriceAmount { get; set; }
        /// <summary>
        /// sku图片 
        ///</summary>
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
        public string AttrKeyValue { get; set; }
        /// <summary>
        /// 规格参数 json字符串格式
        ///</summary>
        public string SpecParam { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人 
        ///</summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 状态（1启用 0停用）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 分期付款期数，多个以英文逗号(,)隔开
        /// </summary>
        public string NumberOfInstallments { get; set; }

        /// <summary>
        /// 内部人员分期付款期数，多个以英文逗号(,)隔开
        /// </summary>
        public string InnerNumberOfInstallments { get; set; }

        public List<NumberOfInstallmentOutput> NumberOfInstallmentModelList { get; set; }

        public List<NumberOfInstallmentOutput> InnerNumberOfInstallmentModelList { get; set; }
    }
}
