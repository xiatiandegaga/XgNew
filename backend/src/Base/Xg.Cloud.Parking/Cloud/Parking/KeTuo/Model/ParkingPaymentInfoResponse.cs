using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Parking.KeTuo.Model
{
    public class ParkingPaymentInfoResponse
    {
        /// <summary>
        /// 车场id
        /// </summary>
        public int parkId { get; set; }

        /// <summary>
        /// 账单号（订单号5分钟内有效）
        /// </summary>
        public string orderNo { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string plateNo { get; set; }

        /// <summary>
        ///卡号、票号
        /// </summary>
        public string cardNo { get; set; }

        /// <summary>
        /// 车场名称
        /// </summary>
        public string parkName { get; set; }

        /// <summary>
        /// 入场时间 (yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public string entryTime { get; set; }

        /// <summary>
        /// 付款(查询费用)时间
        /// </summary>
        public string payTime { get; set; }

        /// <summary>
        /// 停车时长（分钟）
        /// </summary>
        public int elapsedTime { get; set; }

        /// <summary>
        /// 总金额（单位分）
        /// </summary>
        public int totalAmount { get; set; }

        /// <summary>
        /// 本次应付金额（减去优惠金额）,单位为分
        /// </summary>
        public int payable { get; set; }

        /// <summary>
        /// 优惠总金额（单位:分）线上优惠总金额，免费金额+ 免费时长抵扣的金额（需计算即可，不需在停车费中减免）
        /// </summary>
        public int deductionAmount { get; set; }

        /// <summary>
        /// 已经支付过的金额（单位:分） 线上已经支付的金额+线下已经支付的金额+线下优惠金额
        /// </summary>
        public int paidAmount { get; set; }

        /// <summary>
        /// 收费后允许延时出场的时间限制（分钟），默认：20分钟
        /// </summary>
        public int delayTime { get; set; }

        /// <summary>
        /// 停车入场图片
        /// </summary>
        public string imgName { get; set; }

        /// <summary>
        /// 图片信息类型：2=图片URL（可公网访问地址）
        /// </summary>
        public int imgType { get; set; }

        /// <summary>
        /// 图片URL
        /// </summary>
        public string imgInfo { get; set; }

        /// <summary>
        /// 固定车类型  详见参数枚举
        /// </summary>
        public int carType { get; set; }

        /// <summary>
        /// 车辆类型 详见参数枚举
        /// </summary>
        public string carStyle { get; set; }

        /// <summary>
        /// 出入场唯一记录ID
        /// </summary>
        public string trafficId { get; set; }

        /// <summary>
        /// 查询费用扩展信息，返回json字符串
        /// </summary>
        public string extInfo { get; set; }
    }
}
