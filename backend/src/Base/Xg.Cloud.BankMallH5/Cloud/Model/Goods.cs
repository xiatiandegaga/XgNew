using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.BankMallH5.Cloud.Model
{
    public class Goods
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string? goodsId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string? goodsName { get; set; }
 
        /// <summary>
        /// 商品数量
        /// </summary>
        public string? quantity { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public int price { get; set; }
 
        /// <summary>
        /// 商品分类
        /// </summary>
        public string? goodsCategory { get; set; }

        /// <summary>
        /// 商品说明
        /// </summary>
        public string? body { get; set; }

        /// <summary>
        /// 子商户号
        /// </summary>
        public string? subMerchantId { get; set; }

        /// <summary>
        /// 商户子订单号
        /// </summary>
        public string? merOrderId { get; set; }

        /// <summary>
        /// 品总额
        /// </summary>
        public int subOrderAmount { get; set; }
    }
}
