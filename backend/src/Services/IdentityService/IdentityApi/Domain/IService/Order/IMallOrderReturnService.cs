using Cloud.Models;
using Domain.Entity.Order;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Input;
using Identity.Shared.Dto.H5.Input;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IService.Order
{
    public interface IMallOrderReturnService : IBaseService<MallOrderReturn, MallOrderReturnDto>
    {
        new Task<PagingData<List<MallOrderReturnDto>>> GetPageListAsync(PageQueryCommonInput input);
        /// <summary>
        /// 通过订单明细id获取关联的退单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MallOrderReturnDto> GetInfoByOrderDetailIdAsync([FromBody] IdQueryCommonInput input);
        Task CheckOrder(AdminOrderCheckInput input);

        /// <summary>
        /// 退货入库 
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        Task OrderRefund(AdminOrderCheckInput input);

        /// <summary>
        /// 明细单id更新物流运单号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ModifyReturnLogisticsNoAsync(H5MallOrderModifyReturnLogisticsNoInput input);

        /// <summary>
        /// 通过明细单id更新订单明细为已退货（仅退货中可以操作）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task OrderDetailReturnedAsync(IdQueryCommonInput input);
        

    }
}
