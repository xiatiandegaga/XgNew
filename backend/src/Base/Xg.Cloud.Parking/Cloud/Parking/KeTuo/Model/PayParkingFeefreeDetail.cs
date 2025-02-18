using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Parking.KeTuo.Model
{
    public class PayParkingFeefreeDetail
    {
        /// <summary>
        /// 减免金额 单位：分
        /// </summary>
        public int money { get; set; }

        /// <summary>
        /// 减免时间 单位：秒
        /// </summary>
        public int time { get; set; }

        /// <summary>
        /// 会员id、抵扣券编号、购物小票号
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 抵扣来源：0:积分抵扣（对应支付方式1013） 1:抵扣券(对应支付方式1010) 2:购物小票(对应支付方式1014),3:会员减免(对应支付方式1012)等
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 减免项目名称（第三方自定义减免名称）
        /// </summary>
        public string freeName { get; set; }
    }
}
