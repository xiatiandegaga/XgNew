using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.Input
{
    public class AdminMallSkuStockChangeInput
    {
        public long SkuId { get; set; }

        public int Num { get; set; }
    }

    public class AdminMallSkuStockChangeInfoInput
    {
        public long SkuId { get; set; }

        /// <summary>
        /// 出入库类型（0冻结 1入库 2出库）
        /// </summary>
        public int StockType { get; set; }

        /// <summary>
        /// 出入库细分类型
        /// </summary>
        public string StockDetailType { get; set; }
        /// <summary>
        /// 变动sku库存数  
        ///</summary>
        public int ChangeSkuStockCount { get; set; }

        /// <summary>
        /// 变动的冻结库存数  
        ///</summary>
        public int ChangeFreezeStockCount { get; set; }


    }
}
