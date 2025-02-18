namespace Cloud.Models.MiniProgramModel
{
    public class PhoneNumberData
    {
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        public string phoneNumber { get; set; }
        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        public string purePhoneNumber { get; set; }
        /// <summary>
        /// 区号
        /// </summary>
        public string countryCode { get; set; }
        public watermark watermark { get; set; }

    }

    public class watermark
    {
        public string appid { get; set; }
        public int timestamp { get; set; }
    }
}
