using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Parking.KeTuo.Model
{
    public class BaseResponse<T> 
    {
        /// <summary>
        /// 响应代码 0=成功，其他失败，详见返回码枚举
        /// </summary>
        public string resCode { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string resMsg { get; set; }

        /// <summary>
        ///响应内容，json字符串
        /// </summary>
        public T data { get; set; }
    }
}
