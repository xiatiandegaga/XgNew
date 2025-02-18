namespace Cloud.Emailing
{
    public class MailModel
    {
        public static readonly string reportMould = @"
<body>
  <div style=""background:#fff;width:100%;height: 60%;padding: 28rpx;box-sizing: border-box;margin: 0 auto;"">
    <table style=""width: 100%;"">
      <tr>
        <td style=""text-align: left;color: #9F9DA2;"">今日报表</td>
        <td style=""width:80px;height:20px;background-repeat:no-repeat;background-size:100% 100%;"""" background=""https://cloud-common-1302109057.cos.ap-nanjing.myqcloud.com/logo.png""></td>
      </tr>
      <tr>
        <td colspan=""2"">
          <p style=""color:#686868;font-size: 15px;"">为您推送今日报表数据</p>
        </td>
      </tr>
      <tr> 
        <td colspan=""2"" style=""width:312px;height:116px;background-repeat:no-repeat;background-size:100% 100%;""  background=""https://cloud-common-1302109057.cos.ap-nanjing.myqcloud.com/bg.png""></td>
      </tr>
      <tr> 
        <td colspan=""2"">
          <a href=""https://mini.coffeehall.cn:7700?code=$code$"" style=""display: block;width: 100%;height:43px;line-height:43px;color:#fff;
          font-size: 14px;border-radius: 5px;background-color:#63C2FF;text-align: center;"">查看详情</a>
        </td>
      </tr>
    </table>
  </div>
</body>
";
    }
}
