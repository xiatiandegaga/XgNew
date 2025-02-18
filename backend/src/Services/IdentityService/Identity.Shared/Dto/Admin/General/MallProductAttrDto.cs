using Cloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.General
{
    public class MallProductAttrDto : BaseEntity<long>
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public long MallProductId { get; set; }
        /// <summary>
        /// 属性id
        /// </summary>
        public long MallProductAttrKeyId { get; set; }
        /// <summary>
        /// 属性值id
        /// </summary>
        public long MallProductAttrValueId { get; set; }
    }
}
