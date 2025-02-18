using Identity.Shared.Dto.Admin.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.Output
{
    /// <summary>
    /// 产品属性表keyDto
    ///</summary>
    public class MallProductAttrKeyInfoOutput
    {
        public MallProductAttrKeyDto MallProductAttrKeyDto { get; set; }
        public ICollection<MallProductAttrValueDto> MallProductAttrValueDtos { get; set; }
    }
}
