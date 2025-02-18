using System;
using Xg.Cloud.Core;

namespace Cloud.Domain.Entities
{
    public abstract class BaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 实体唯一标识
        /// </summary>
        public TPrimaryKey Id { get; set; }

        /// <summary>
        /// 删除状态  1: 正常   2:已删除
        /// </summary>
        public int DeletedStatus { get; set; } = CommonConst.DeletedStatus_Normal;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public TPrimaryKey CreatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新人
        /// </summary>
        public TPrimaryKey UpdatedBy { get; set; }

    }
}
