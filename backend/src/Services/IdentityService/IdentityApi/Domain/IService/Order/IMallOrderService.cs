using Cloud.LogisticsFuQing.Models;
using Cloud.Models;
using Domain.Entity.Order;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Input;
using Identity.Shared.Dto.H5.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.BankMallH5.Cloud.Model;

namespace Domain.IService.Order
{
    public interface IMallOrderService : IBaseService<MallOrder, MallOrderDto>
    {
        Task<PagingData<List<MallOrderDto>>> GetPageListAsync(AdminMallOrderInput input);
        /// <summary>
        /// 修改收货地址
        /// </summary>
        /// <param name="receiveInfo"></param>
        /// <exception cref="MyException"></exception>
        Task UpdateReceiveInfo(ReceiveInfoInput receiveInfo);
        Task<QrcodeModel> OrderPayAsync(H5MallOrderPayInput model);
        Task<PagingData<List<MallOrderDto>>> GetLoginUserPageListByStatusAsync(int pageIndex, int pageSize, int? status);

        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <param name="data"></param>
        Task<string> PayNotityUpdateOrder(string data);

        Task<QrcodeModel> PayById(long id);

        /// <summary>
        /// 退单（子单退）
        /// </summary>
        /// <param name="input"></param>
        Task ReturnDetailOrder(H5MallOrderReturnInput input);

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="input"></param>
        Task CancelOrder(H5MallOrderReturnInput input);
        #region 发货&签收
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        Task SendOut(AdminOrderSendOutInput input);

        /// <summary>
        /// 签收
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        Task SignFor(IdQueryCommonInput input);

        /// <summary>
        /// 自动签收
        /// </summary>
        Task AutoSignFor();
        #endregion

        Task<FuQingNoQueryResultResponse> GetOrderLogisticsById(IdQueryCommonInput input);
    }
}
