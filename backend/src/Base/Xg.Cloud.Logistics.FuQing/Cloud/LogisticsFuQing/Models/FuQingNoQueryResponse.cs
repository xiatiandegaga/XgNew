namespace Cloud.LogisticsFuQing.Models
{ 
    public class FuQingNoQueryResponse
    {
        /// <summary>
        ///  status 0:正常查询 201:快递单号错误 203:快递公司不存在 204:快递公司识别失败 205:没有信息 207:该单号被限制，错误单号 */
        /// </summary>
        public string Status { get; set; }

        public string Msg { get; set; }

        public FuQingNoQueryResultResponse Result { get; set; }
    }
}
