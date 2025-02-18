using Cloud.Domain.Entities;
using System;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// 产品属性表值valueDto
    ///</summary>
    public class MallProductAttrValueDto : BaseEntity<long>
    {
        /// <summary>
        /// 属性值名 
        ///</summary>
        public string AttrValueName { get; set; }
        /// <summary>
        /// 属性名id 
        ///</summary>
        public long MallProductAttrKeyId { get; set; }
        /// <summary>
        /// 排序 
        ///</summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 状态（1启用 0禁用）
        /// </summary>
        public int Status { get; set; }
    }
}
