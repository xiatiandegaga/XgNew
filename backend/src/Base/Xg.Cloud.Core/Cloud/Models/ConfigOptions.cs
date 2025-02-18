namespace Cloud.Models
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class ConfigOptions
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public PayWay PayWay { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public Project Project { get; set; }

    }
    /// <summary>
    /// 支付方式
    /// </summary>
    public class PayWay
    {
        /// <summary>
        /// 所有的模式
        /// </summary>
        public string All { get; set; }
        /// <summary>
        /// 咖啡订单支付模式（纯现金：Cash，纯积分：Point，现金加积分：CashAndPoint）
        /// </summary>
        public string CoffeePayWay { get; set; }
        /// <summary>
        /// 商城订单支付模式（纯现金：Cash，纯积分：Point，现金加积分：CashAndPoint）
        /// </summary>
        public string PointGoodsPayWay { get; set; }

        /// <summary>
        /// 绚集商城订单支付模式（纯现金：Cash，纯积分：Point，现金加积分：CashAndPoint）
        /// </summary>
        public string XuanJiPointGoodsPayWay { get; set; }
    }
    /// <summary>
    /// 项目
    /// </summary>
    public class Project
    {
        /// <summary>
        /// 所有的项目
        /// </summary>
        public string All { get; set; }
        /// <summary>
        /// 当前项目
        /// </summary>
        public string Current { get; set; }
    }
}
