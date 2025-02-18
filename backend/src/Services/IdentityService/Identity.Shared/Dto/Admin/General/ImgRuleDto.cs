using Cloud.Domain.Entities;
using System;
using System.Collections.Generic;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// ImgRuleDto
    ///</summary>
    public class ImgRuleDto : BaseEntity<long>
    {
        /// <summary>
        /// 图片规则编号 
        ///</summary>
        public string ImgRuleCode { get; set; }
        /// <summary>
        /// 图片规则名称 
        ///</summary>
        public string ImgRuleName { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        public string Remark { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }


    }
}
