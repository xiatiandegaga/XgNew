using Cloud.Extensions;
using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Dictionary;
using Domain.Entity.Identity;
using Domain.Entity.Order;
using Domain.Interface;
using Domain.IService.Identity;
using Domain.IService.Order;
using Domain.Service.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Input;
using Identity.Shared.Dto.H5.Input;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xg.Cloud.BankMallH5;
using Xg.Cloud.BankMallH5.Cloud.QueryModel;
using Xg.Cloud.Core;

namespace Domain.Service.Order
{
    public class MallOrderReturnService : BaseService<MallOrderReturn, MallOrderReturnDto>, IMallOrderReturnService
    {
        private readonly ICloudUnitOfWork _unitWork;
        private readonly IRepository<MallOrderReturn> _mallOrderReturnRepository;
        private readonly IListCacheRepository<GlobalDataDetail> _listCacheGlobalDataDetailRepository;
        private readonly ICacheRepository<User> _cacheUserRepository;
        private readonly IAuthenticationPrincipalService _authenticationPrincipalService;
        private readonly IBankMallH5Service _bankMallH5Service;
        private readonly IMallProductSkuStockHistoryService _mallProductSkuStockHistoryService;
        public MallOrderReturnService(ICloudUnitOfWork unitWork, IRepository<MallOrderReturn> mallOrderReturnRepository, IListCacheRepository<GlobalDataDetail> listCacheGlobalDataDetailRepository, ICacheRepository<User> cacheUserRepository , IAuthenticationPrincipalService authenticationPrincipalService, IBankMallH5Service bankMallH5Service, IMallProductSkuStockHistoryService mallProductSkuStockHistoryService) : base(mallOrderReturnRepository)
        {
            _unitWork = unitWork;
            _mallOrderReturnRepository = mallOrderReturnRepository;
            _listCacheGlobalDataDetailRepository = listCacheGlobalDataDetailRepository;
            _cacheUserRepository= cacheUserRepository;
            _authenticationPrincipalService = authenticationPrincipalService;
            _bankMallH5Service= bankMallH5Service;
            _mallProductSkuStockHistoryService= mallProductSkuStockHistoryService;
        }



        #region  数据查询
        public new async Task<PagingData<List<MallOrderReturnDto>>> GetPageListAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<MallOrderReturn>();
            var list = _mallOrderReturnRepository.Paginate(input.PageNumber, input.PageSize, express, null, x => x.Id,x=>x.MallOrderDetail).ProjectToType<MallOrderReturnDto>().ToList();
            await GetListDtoAsync(list);
            var totalCount = await _mallOrderReturnRepository.CountAsync(express);
            return new PagingData<List<MallOrderReturnDto>> { PageIndex = input.PageNumber, TotalCount = totalCount, List = list };
        }

        public new async Task<MallOrderReturnDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var dto =(await  _mallOrderReturnRepository.GetSingleAsync(x => x.Id == input.Id,true, x => x.MallOrderDetail)).MapTo<MallOrderReturnDto>();
            var globalDataDetailList = _listCacheGlobalDataDetailRepository.Query(x => x.Code == CommonConst.MallOrderDetailStatus).ToList();
            await GetDtoAsync(dto, globalDataDetailList);
            return dto;
        }
        /// <summary>
        /// 通过订单明细id获取关联的退单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MallOrderReturnDto> GetInfoByOrderDetailIdAsync([FromBody] IdQueryCommonInput input)
        {
            var dto = await _mallOrderReturnRepository.Query(x=>x.MallOrderDetailId==input.Id, true,x => x.MallOrderDetail).ProjectToType<MallOrderReturnDto>().FirstOrDefaultAsync();
            var globalDataDetailList = _listCacheGlobalDataDetailRepository.Query(x => x.Code == CommonConst.MallOrderDetailStatus).ToList();
            await GetDtoAsync(dto, globalDataDetailList);
            return dto;
        }
        private async Task GetListDtoAsync(List<MallOrderReturnDto> listDto)
        {
            if (listDto != default)
            {
                foreach (var dto in listDto)
                {
                    var globalDataDetailList = _listCacheGlobalDataDetailRepository.Query(x => x.Code == CommonConst.MallOrderDetailStatus).ToList();
                    await GetDtoAsync(dto, globalDataDetailList);
                }
            }
        }

        private async Task GetDtoAsync(MallOrderReturnDto dto, List<GlobalDataDetail> globalDataDetailList)
        {
            if (dto != default)
            {
             
                dto.DetailStatusName = globalDataDetailList.FirstOrDefault(x => x.Code == CommonConst.MallOrderDetailStatus && x.ConstKey == dto.DetailStatus.ToString())?.Name;

                var user =await _cacheUserRepository.GetSingleByIdAsync(dto.UserId);
                if (user != default)
                {
                    dto.UserName = user.RealName;
                    dto.UserMobile = user.Mobile;
                }
                var checkUser=await _cacheUserRepository.GetSingleByIdAsync(dto.CheckUserId);
                if (checkUser != default)
                {
                    dto.CheckUserName = checkUser.RealName;
                }
                dto.ApplicationReasonName= globalDataDetailList.FirstOrDefault(x => x.Code == CommonConst.OrderReturnApplicationReason && x.ConstKey == dto.ApplicationReason.ToString())?.Name;
                dto.ProductAmount=((dto.ProductPrice*1.00)/100).ToDecimal();
                dto.TotalProductAmount=dto.ProductAmount*dto.ProductQuantity;
            }
        }
        #endregion

        /// <summary>
        /// 退货申请审核
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        public async Task CheckOrder(AdminOrderCheckInput input)
        {
            var orderReturn =await _unitWork.GetSingleAsync<MallOrderReturn>(x=>x.Id==input.Id, false,x => x.MallOrderDetail);
            if (orderReturn==default)
            {
                throw new MyException("退单订单不存在！");
            }
            var orderDetail = orderReturn.MallOrderDetail;
            if (orderReturn == default)
            {
                throw new MyException("订单子单不存在！");
            }
            if (orderReturn.Status!=0)
            {
                throw new MyException("单据已审核，请勿重复操作！");
            }
            if (orderDetail.Status != CommonConst.MallOrderDetailStatus_4 && orderDetail.Status != CommonConst.MallOrderDetailStatus_7 )
            {
                throw new MyException("单据不是退款申请中或者退货申请中状态，无法审核！");
            }
            var loginUser=await _authenticationPrincipalService.GetAuthenticatedUserAsync();
            orderReturn.CheckUserId = loginUser.Id;
            orderReturn.CheckDate = DateTime.Now;

            var detailOrder =await _unitWork.GetSingleAsync<MallOrderDetail>(x=>x.Id==orderReturn.MallOrderDetailId);
            if (detailOrder == default)
            {
                throw new MyException("关联订单不存在！");
            }
            var order =await _unitWork.GetSingleAsync<MallOrder>(x=>x.Id==detailOrder.MallOrderId);
            if (order == default)
            {
                throw new MyException("关联订单不存在！");
            }
            //通过
            if (input.Status == 1)
            {
                orderReturn.Status = 1;

                //待发货时直接退款中
                if (detailOrder.Status == CommonConst.MallOrderDetailStatus_4)
                {
                    detailOrder.Status = CommonConst.MallOrderDetailStatus_13;
                    //orderReturn.DetailStatus = CommonConst.MallOrderDetailStatus_13;
                }
                // 退货申请中 直接改成退货中
                else if (detailOrder.Status == CommonConst.MallOrderDetailStatus_7)
                {
                    detailOrder.Status = CommonConst.MallOrderDetailStatus_8;
                    //orderReturn.DetailStatus = CommonConst.MallOrderDetailStatus_8;
                }


            }
            //拒绝
            else
            {
                if(string.IsNullOrWhiteSpace(input.Message))
                {
                    throw new MyException("请填写拒绝原因！");
                }
                orderReturn.Status = 2;
                orderReturn.ResponseResult=input.Message;
                //orderReturn.DetailStatus= CommonConst.MallOrderDetailStatus_12;
                detailOrder.Status= CommonConst.MallOrderDetailStatus_12;
            }
            _unitWork.Update(detailOrder);
            _unitWork.Update(order);
            _unitWork.Update(orderReturn);
            await _unitWork.CommitAsync();
        }


        /// <summary>
        /// 退货单退款 
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        public async Task OrderRefund(AdminOrderCheckInput input)
        {
            var orderReturn =await _unitWork.GetSingleAsync<MallOrderReturn>(x => x.Id == input.Id, false,x => x.MallOrderDetail);
            if (orderReturn == default)
            {
                throw new MyException("订单不存在！");
            }

            var detailOrder = orderReturn.MallOrderDetail;
            if (detailOrder == default)
            {
                throw new MyException("关联订单不存在！");
            }

            if (orderReturn.Status != 1)
            {
                throw new MyException("退货申请单未通过，无法退款！！");
            }
            if (detailOrder.Status != CommonConst.MallOrderDetailStatus_9 && detailOrder.Status != CommonConst.MallOrderDetailStatus_13)
            {
                throw new MyException("单据不是未发货退款申请通过或已退货状态，无法退款！");
            }
            var loginUser =await  _authenticationPrincipalService.GetAuthenticatedUserAsync();

            var order =await  _unitWork.GetSingleAsync<MallOrder>(x => x.Id == detailOrder.MallOrderId);
            if (order == default)
            {
                throw new MyException("关联订单不存在！");
            }


            detailOrder.Status = CommonConst.MallOrderDetailStatus_5;
            //orderReturn.DetailStatus = CommonConst.MallOrderDetailStatus_5;

            var thirdReturnOrderNo = $"{order.OrderNo}{TimeExtension.CurrentTimeFFString()}";
            detailOrder.ThirdReturnOrderNo=thirdReturnOrderNo;
            _unitWork.Update(detailOrder);
            _unitWork.Update(order);
            _unitWork.Update(orderReturn);
            await _unitWork.CommitAsync();
            OrderReturnQueryModel orderReturnQueryModel = new OrderReturnQueryModel()
            {
                merOrderId = order.ThirdOrderNo,
                msgId = order.Id.ToString(),
                refundAmount = detailOrder.ProductPrice*detailOrder.ProductQuantity,
                refundDesc = orderReturn.Remark,
                refundOrderId = thirdReturnOrderNo,
            };
            var res = await _bankMallH5Service.OrderReturn(orderReturnQueryModel);
            if (res.ErrCode != "SUCCESS")
            {
                throw new MyException($"退款失败：{res.ErrMsg}");
            }
            // _unitWork.RemoveListCache<MallProductSku>();
        }

        /// <summary>
        /// 通过明细单id更新物流运单号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ModifyReturnLogisticsNoAsync(H5MallOrderModifyReturnLogisticsNoInput input)
        {
            var entity = await _unitWork.GetSingleAsync<MallOrderReturn>(x => x.MallOrderDetailId == input.Id);
            entity.ReturnLogisticsNo=input.ReturnLogisticsNo;
            _unitWork.Update(entity);
            await _unitWork.CommitAsync();
        }

        /// <summary>
        /// 通过明细单id更新订单明细为已退货（仅退货中可以操作）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task OrderDetailReturnedAsync(IdQueryCommonInput input)
        {
            var entity = await _unitWork.GetSingleAsync<MallOrderDetail>(x => x.Id == input.Id);
            if (entity.Status != CommonConst.MallOrderDetailStatus_8)
            {
                throw new MyException("单据不是退货中状态，无法更改成已退货！");
            }
            entity.Status = CommonConst.MallOrderDetailStatus_9;
            _unitWork.Update(entity);
            await _unitWork.CommitAsync();
        }


    }
}
