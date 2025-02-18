using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;
using Xg.Cloud.BankMallH5.Cloud.Model;
using Xg.Cloud.BankMallH5.Cloud.QueryModel;

namespace Xg.Cloud.BankMallH5
{
    public interface IBankMallH5Service
    {
       
        Task<QrcodeModel> GetQrcode(QrcodeQueryModel qrcodeQueryModel);
        Task<OrderReturnModel> OrderReturn(OrderReturnQueryModel orderReturnQueryModel);

        /// <summary>
        /// 订单关闭
        /// </summary>
        /// <param name="merOrderId">商户订单号 原交易单号</param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        Task<OrderCloseModel> OrderColse(string merOrderId);
        Task ReturnQuery(string orderNo);
        Task<string?> GetAccessToken();

    }
}
