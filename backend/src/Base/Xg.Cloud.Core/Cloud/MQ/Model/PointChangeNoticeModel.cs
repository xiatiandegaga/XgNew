using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.MQ.Model
{
    public class PointChangeNoticeModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 变动积分
        /// </summary>
        public int Point{get;set;}
        /// <summary>
        /// 变动类目（积分/滤波币等）
        /// </summary>
        public string ChangeClass { get; set; }
        /// <summary>
        /// 变动类型（商城/门柜/滤波商城等）
        /// </summary>
        public string ChangeType { get; set; }
        /// <summary>
        /// 变动时间
        /// </summary>
        public DateTime ChangeDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
