using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Parking.KeTuo.Model
{
    public class PayParkingFeeResponse
    {
        /// <summary>
        /// 支付后获得总停车时间(分钟)，用来计算出厂时间+实际停车时间的，ElapsedTime是查询账单时的停车时间，一般客户付完单后会有几十分钟的延迟出场时间
        /// </summary>
        public int parkingTime { get; set; }
    }
}
