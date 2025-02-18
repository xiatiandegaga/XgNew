using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Parking.KeTuo.Model
{
    public class GetOrderStatusResponse
    {
        /// <summary>
        /// 订单状态0:未支付1:已支付
        /// </summary>
        public string status { get; set; }
    }
}
