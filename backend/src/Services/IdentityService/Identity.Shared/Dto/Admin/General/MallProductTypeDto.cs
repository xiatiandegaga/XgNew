using Cloud.Domain.Entities;
using System;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// 产品类型表Dto
    ///</summary>
    public class MallProductTypeDto : BaseEntity<long>
    {
        /// <summary>
        /// 类型名称 
        ///</summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 类型编号 
        ///</summary>
        public string TypeCode { get; set; }
    }
}
