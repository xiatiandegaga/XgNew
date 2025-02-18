using Identity.Shared.Dto.Admin.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.Output
{
    public class AdminMallProductAttrOutput
    {
        /// <summary>
        /// 商品属性id
        /// </summary>
        public long AttrKeyId { get; set; }
        /// <summary>
        /// 商品属性名
        /// </summary>
        public string AttrKeyName { get; set; }
        /// <summary>
        /// 商品属性值id
        /// </summary>
        public long AttrValueId { get; set; }
        /// <summary>
        /// 商品属性值名
        /// </summary>
        public string AttrValueName { get; set; }

    }

    public class AdminMallProductsGroupOutput
    {
        public MallProductCategoryDto Category { get; set; }

        public List<MallProductDto> ProductDtos { get; set; }
    }

    public class NumberOfInstallmentOutput
    {
        public int NumberOfInstallments { get; set; }

        public decimal Price { get; set; }
    }
}
