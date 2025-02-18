using Cloud.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Identity
{
    public class UserAddress:BaseEntity<long>
    {
        
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 收货人地址-省 
        ///</summary>
        [MaxLength(200)]
        public string ReceiverProvinceName { get; set; }
        /// <summary>
        /// 收货人地址-市 
        ///</summary>
        [MaxLength(200)]
        public string ReceiverCityName { get; set; }
        /// <summary>
        /// 收货人地址-区 
        ///</summary>
        [MaxLength(200)]
        public string ReceiverCountyName { get; set; }
        /// <summary>
        /// 收货人详细地址 
        ///</summary>
        [MaxLength(500)]
        public string ReceiverDetailInfo { get; set; }
        /// <summary>
        /// 收货人邮编 
        ///</summary>
        [MaxLength(200)]
        public string ReceiverPostCode { get; set; }
        /// <summary>
        /// 收货人电话 
        ///</summary>
        [MaxLength(200)]
        public string ReceiverMobile { get; set; }
        /// <summary>
        /// 收货人姓名 
        ///</summary>
        [MaxLength(100)]
        public string ReceiverName { get; set; }

        /// <summary>
        /// 是否默认（1默认）
        /// </summary>
        public int IsDefault { get; set; }
    }
}
