using Cloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.General
{
    public class UserAddressDto : BaseEntity<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 收货人地址-省 
        ///</summary>
        public string ReceiverProvinceName { get; set; }
        /// <summary>
        /// 收货人地址-市 
        ///</summary>
        public string ReceiverCityName { get; set; }
        /// <summary>
        /// 收货人地址-区 
        ///</summary>
        public string ReceiverCountyName { get; set; }
        /// <summary>
        /// 收货人详细地址 
        ///</summary>
        public string ReceiverDetailInfo { get; set; }
        /// <summary>
        /// 收货人邮编 
        ///</summary>
        public string ReceiverPostCode { get; set; }
        /// <summary>
        /// 收货人电话 
        ///</summary>
        public string ReceiverMobile { get; set; }
        /// <summary>
        /// 收货人姓名 
        ///</summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 是否默认（1默认）
        /// </summary>
        public int IsDefault { get; set; }
    }
}
