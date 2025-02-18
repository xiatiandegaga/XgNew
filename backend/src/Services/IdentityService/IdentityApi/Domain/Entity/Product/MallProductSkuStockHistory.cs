using Cloud.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class MallProductSkuStockHistory:BaseEntity<long>
    {
        /// <summary>
        /// 出入库编号
        ///</summary>
        [MaxLength(100)]
        public string StockNo { get; set; }

        /// <summary>
        /// 出入库productid
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 出入库skuid
        /// </summary>
        public long SkuId { get; set; }

        /// <summary>
        /// 出入库数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 备注
        ///</summary>
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 出入库类型（1入库 2出库）
        /// </summary>
        public int StockType { get; set; }

        /// <summary>
        /// 属性
        ///</summary>
        [MaxLength(1000)]
        public string GoodsAttrs { get; set; }

        /// <summary>
        /// 出入库细分类型
        ///</summary>
        [MaxLength(200)]
        public string StockDetailType { get; set; }

    }
}
