using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.MQ.Model
{
    public class XjSeckillModel
    {
        /// <summary>
        /// 秒杀id
        /// </summary>
        public long SeckillId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// SkuId
        /// </summary>
        public long SkuId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? lng { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public decimal? lat { get; set; }
    }
}
