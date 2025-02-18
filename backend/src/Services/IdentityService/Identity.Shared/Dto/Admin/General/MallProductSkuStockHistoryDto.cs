using Cloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.General
{
    public class MallProductSkuStockHistoryDto : BaseEntity<long>
    {
        /// <summary>
        /// 出入库编号
        /// </summary>
        public string StockNo { get; set; }

        /// <summary>
        /// 出入库日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 操作用户id
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 出入库skuid
        /// </summary>
        public long SkuId { get; set; }
        /// <summary>
        /// 出入库productid
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 出入库数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 出入库类型（1入库 2出库）
        /// </summary>
        public int StockType { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public string GoodsAttrs { get; set; }

        /// <summary>
        /// 出入库细分类型
        /// </summary>
        public string StockDetailType { get; set; }

        /// <summary>
        /// 出入库细分类型名称
        /// </summary>
        public string StockDetailTypeName { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string MallProductName { get; set; }
    }
}
