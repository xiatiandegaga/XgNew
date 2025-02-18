using Cloud.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Dictionary
{
    /// <summary>
    /// 
    ///</summary>
    public class ImgRuleDetail : BaseEntity<long>
    {
        /// <summary>
        /// 图片规则明细编号 
        ///</summary>
        public string ImgRuleDetailCode { get; set; }
        /// <summary>
        /// 图片规则明细名称 
        ///</summary>
        public string ImgRuleDetailName { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        public string Remark { get; set; }
        /// <summary>
        /// 排序序号（数值越小越靠前）
        ///</summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 图片规则主表id 
        ///</summary>
        public long? ImgRuleId { get; set; }
        /// <summary>
        /// 图片规则明细-主图 
        ///</summary>
        public string MainImg { get; set; }
        /// <summary>
        /// 图片规则明细-明细图 
        ///</summary>
        public string DetailImage { get; set; }
        /// <summary>
        /// 链接类型（详见字典ImgLinkType） 
        ///</summary>
        public string LinkType { get; set; }
        /// <summary>
        /// 链接主键值 
        ///</summary>
        public string LinkKey { get; set; }
        /// <summary>
        /// 链接地址 
        ///</summary>
        [MaxLength(1000)]
        public string LinkAddress { get; set; }
        /// <summary>
        /// 有效期起始时间 
        ///</summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 有效期截止时间 
        ///</summary>
        public DateTime EndTime { get; set; }
    }
}
