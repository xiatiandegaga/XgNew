namespace Xg.Cloud.BankMallH5.Cloud.Model
{
    public class BaseReturnModel
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public string? ErrCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string? ErrMsg { get; set; }

        public string? ErrInfo { get; set; }
    }
}
