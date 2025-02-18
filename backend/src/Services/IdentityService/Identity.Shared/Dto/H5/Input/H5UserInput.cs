using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.H5.Input
{

    public class H5UserSignMobileInput
    {
        /// <summary>
        /// sm2加密用户手机号
        /// </summary>
        public string SignMobile { get; set; }
    }

    public class H5UserJsBankUserInput
    {
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string MobileNo { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CoreCustNo { get; set; }
    }

}
