using System;

namespace Cloud.SMS
{
    public class SmsOptions
    {
        public string NationCode { get; set; } = "86";

        public string PhoneNumber { get; set; }
        /// <summary>
        /// 下发手机号码，采用 e.164 标准，+[国家或地区码][手机号]例如+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号，最多不要超过200个手机号*/
        /// </summary>
        public String[] PhoneNumberSet { get; set; }

        public string TemplateId { get; set; }

        public string[] Parameters { get; set; }

        public string Sign { get; set; } = string.Empty;

        public string Extend { get; set; } = string.Empty;

        public string Ext { get; set; } = string.Empty;
    }
}
