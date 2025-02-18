using Cloud.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Dictionary
{
    /// <summary>
    /// 
    ///</summary>
    public class ImgRule : BaseEntity<long>
    {
        /// <summary>
        /// 图片规则编号 
        ///</summary>
        [MaxLength(100)]
        public string ImgRuleCode { get; set; }
        /// <summary>
        /// 图片规则名称 
        ///</summary>
        [MaxLength(100)]
        public string ImgRuleName { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        [MaxLength(500)]
        public string Remark { get; set; }
        /// <summary>
        /// 排序-正序 
        ///</summary>
        public int SortNo { get; set; }
    }
}
