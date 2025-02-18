using Cloud.Caching;
using Cloud.Extensions;
using Cloud.LogisticsFuQing.Models;
using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Snowflake;
using Cloud.Utilities;
using Cloud.Utilities.Json;
using Domain.Entity;
using Domain.Entity.Dictionary;
using Domain.Entity.Identity;
using Domain.Entity.Order;
using Domain.Entity.Product;
using Domain.Interface;
using Domain.IService.Identity;
using Domain.IService.Order;
using Domain.IService.Product;
using Domain.Service.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Input;
using Identity.Shared.Dto.H5.Input;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.BankMallH5;
using Xg.Cloud.BankMallH5.Cloud.Model;
using Xg.Cloud.BankMallH5.Cloud.QueryModel;
using Xg.Cloud.Core;
using static FreeRedis.RedisClient;
namespace Domain.Service.Order
{
    /// <summary>
    /// 
    /// </summary>
    public class MallOrderService(IRepository<MallOrder> repository, ICloudUnitOfWork unitWork, ICache cache, IAuthenticationPrincipalService authenticationPrincipalService, IBankMallH5Service bankMallH5Service, IConfiguration configuration, ILogger<MallOrderService> logger, IMallProductAttrKeyService mallProductAttrKeyService, ICacheRepository<User> userCacheRepository, IListCacheRepository<GlobalDataDetail> listCacheGlobalDataDetailRepository, IMallProductSkuStockHistoryService mallProductSkuStockHistoryService, IMallProductSkuService mallProductSkuService, ILogisticsQuery logisticsQuery, ISnowflakeIdWorker snowflakeIdWorker) : BaseService<MallOrder, MallOrderDto>(repository), IMallOrderService
    {

        private readonly IRepository<MallOrder> _repository = repository;
        private readonly ICloudUnitOfWork _unitWork = unitWork;
        private readonly ICache _cache = cache;
        private readonly IAuthenticationPrincipalService _authenticationPrincipalService = authenticationPrincipalService;
        private readonly IBankMallH5Service _bankMallH5Service = bankMallH5Service;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<MallOrderService> _logger = logger;
        private readonly IMallProductAttrKeyService _mallProductAttrKeyService = mallProductAttrKeyService;
        private readonly ICacheRepository<User> _userCacheRepository = userCacheRepository;
        private readonly IListCacheRepository<GlobalDataDetail> _listCacheGlobalDataDetailRepository = listCacheGlobalDataDetailRepository;
        private readonly IMallProductSkuStockHistoryService _mallProductSkuStockHistoryService = mallProductSkuStockHistoryService;
        private readonly IMallProductSkuService _mallProductSkuService = mallProductSkuService;
        private readonly ILogisticsQuery _logisticsQuery = logisticsQuery;
        private readonly ISnowflakeIdWorker _snowflakeIdWorker=snowflakeIdWorker;

        public async Task<PagingData<List<MallOrderDto>>> GetPageListAsync(AdminMallOrderInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<MallOrder>();
            if (input.Status == CommonConst.MallOrderStatus_0)
            {
                express = express.CloudAnd(x => x.Status == CommonConst.MallOrderStatus_0);
            }
            else if (input.Status == CommonConst.MallOrderStatus_1)
            {
                express = express.CloudAnd(x => x.Status == CommonConst.MallOrderStatus_1 && x.MallOrderDetails.Any(y => y.Status == CommonConst.MallOrderDetailStatus_1));
            }
            else if (input.Status == CommonConst.MallOrderStatus_2)
            {
                express = express.CloudAnd(x => x.Status == CommonConst.MallOrderStatus_2 && x.MallOrderDetails.Any(y => y.Status == CommonConst.MallOrderDetailStatus_2));
            }
            else if (input.Status == CommonConst.MallOrderStatus_3)
            {
                express = express.CloudAnd(x => x.Status == CommonConst.MallOrderStatus_3 && x.MallOrderDetails.Any(y => y.Status == CommonConst.MallOrderDetailStatus_3));
            }
            else if (input.Status == CommonConst.MallOrderStatus_4)
            {
                List<int> statusList = new List<int>() { CommonConst.MallOrderDetailStatus_4, CommonConst.MallOrderDetailStatus_5, CommonConst.MallOrderDetailStatus_6, CommonConst.MallOrderDetailStatus_7, CommonConst.MallOrderDetailStatus_8, CommonConst.MallOrderDetailStatus_9 };
                express = express.CloudAnd(x => x.MallOrderDetails.Any(y => statusList.Contains(y.Status)));
            }
            var list = _repository.Paginate(input.PageNumber, input.PageSize, express, null, x => x.Id, u => u.MallOrderDetails).ProjectToType<MallOrderDto>().ToList();
            GetListDto(list);
            var totalCount = await _repository.CountAsync(express);
            return new PagingData<List<MallOrderDto>> { PageIndex = input.PageNumber, TotalCount = totalCount, List = list };
        }

        #region  数据查询
        /// <summary>
        /// 根据订单状态获取当前登录人的订单信息
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public async Task<PagingData<List<MallOrderDto>>> GetLoginUserPageListByStatusAsync(int pageNumber, int pageSize, int? status)
        {
            var express = LinqExtensions.True<MallOrder>();
            var loginUser =await  _authenticationPrincipalService.GetAuthenticatedUserAsync();

            express = express.CloudAnd(x => x.UserId == loginUser.Id);
            //待付款
            if (status == CommonConst.MallOrderStatus_0)
            {
                express = express.CloudAnd(x => x.Status == CommonConst.MallOrderStatus_0);
            }
            else if (status == CommonConst.MallOrderStatus_1)
            {
                express = express.CloudAnd(x => x.Status == CommonConst.MallOrderStatus_1 && x.MallOrderDetails.Any(y => y.Status == CommonConst.MallOrderDetailStatus_1));
            }
            else if (status == CommonConst.MallOrderStatus_2)
            {
                express = express.CloudAnd(x => x.Status == CommonConst.MallOrderStatus_2 && x.MallOrderDetails.Any(y => y.Status == CommonConst.MallOrderDetailStatus_2));
            }
            else if (status == CommonConst.MallOrderStatus_3)
            {
                express = express.CloudAnd(x => x.Status == CommonConst.MallOrderStatus_3 && x.MallOrderDetails.Any(y => y.Status == CommonConst.MallOrderDetailStatus_3));
            }
            else if (status == CommonConst.MallOrderStatus_4)
            {
                List<int> statusList = new List<int>() { CommonConst.MallOrderDetailStatus_4, CommonConst.MallOrderDetailStatus_5, CommonConst.MallOrderDetailStatus_6, CommonConst.MallOrderDetailStatus_7, CommonConst.MallOrderDetailStatus_8, CommonConst.MallOrderDetailStatus_9 };
                express = express.CloudAnd(x => x.MallOrderDetails.Any(y => statusList.Contains(y.Status)));
            }
            var list = _repository.Paginate(pageNumber, pageSize, express, null, x => x.Id, u => u.MallOrderDetails).ProjectToType<MallOrderDto>().ToList();
            GetListDto(list);
            var totalCount = await _repository.CountAsync(express);
            return new PagingData<List<MallOrderDto>> { PageIndex = pageNumber, TotalCount = totalCount, List = list };
        }

        public new async Task<MallOrderDto> GetSingleByIdAsync(IdQueryCommonInput input)
        {
            var dto = await base.GetSingleByIdAsync(input);
            var globalDataDetailList = _listCacheGlobalDataDetailRepository.Query(x => x.Code == CommonConst.MallOrderDetailStatus || x.Code == CommonConst.MallOrderStatus || x.Code == CommonConst.LogisticsType).ToList();
            var mallOrderDetailList = _unitWork.Query<MallOrderDetail>(x => x.MallOrderId == dto.Id).ProjectToType<MallOrderDetailDto>().ToList();
            dto.MallOrderDetails = mallOrderDetailList;
            GetDto(dto, globalDataDetailList);
            return dto;
        }
        private void GetListDto(List<MallOrderDto> listDto)
        {
            if (listDto != default)
            {
                listDto.ForEach(dto =>
                {
                    var globalDataDetailList = _listCacheGlobalDataDetailRepository.Query(x => x.Code == CommonConst.MallOrderDetailStatus || x.Code == CommonConst.MallOrderStatus || x.Code == CommonConst.LogisticsType).ToList();
                    GetDto(dto, globalDataDetailList);
                });
            }
        }

        private async void GetDto(MallOrderDto dto, List<GlobalDataDetail> globalDataDetailList)
        {
            if (dto != default)
            {
                dto.StatusName = globalDataDetailList.FirstOrDefault(x => x.Code == CommonConst.MallOrderStatus && x.ConstKey == dto.Status.ToString())?.Name;
                dto.PayAmount = ((dto.PayPrice * 1.00) / 100).ToDecimal();
                dto.TotalAmount = ((dto.TotalPrice * 1.00) / 100).ToDecimal();
                dto.AdminDiscountAmount = ((dto.AdminDiscountPrice * 1.00) / 100).ToDecimal() / 100;
                if (!string.IsNullOrWhiteSpace(dto.ReceiveInfo))
                {
                    dto.ReceiveInfoModel = JsonUtility.Deserialize<ReceiveInfoInput>(dto.ReceiveInfo);
                    if (dto.ReceiveInfoModel != default)
                    {
                        dto.ReceiveInfoModel.OrderId = dto.Id;
                    }
                }
                var user =await  _userCacheRepository.GetSingleByIdAsync(dto.UserId);
                if (user != default)
                {
                    dto.UserName = user.RealName;
                    dto.UserMobile = user.Mobile;
                }
                if (dto.MallOrderDetails != default)
                {
                    dto.MallOrderDetails.ForEach(detailDto =>
                    {
                        detailDto.StatusName = globalDataDetailList.FirstOrDefault(x => x.Code == CommonConst.MallOrderDetailStatus && x.ConstKey == detailDto.Status.ToString())?.Name;

                        detailDto.ProductPriceAmount = ((detailDto.ProductPrice * 1.00) / 100).ToDecimal();

                    });

                }

                if (!string.IsNullOrWhiteSpace(dto.LogisticsCompany))
                {
                    dto.LogisticsCompanyName = globalDataDetailList.FirstOrDefault(x => x.Code == CommonConst.LogisticsType && x.ConstKey == dto.LogisticsCompany)?.Name;
                }
            }
        }
        #endregion

        #region 修改信息
        /// <summary>
        /// 修改收货地址
        /// </summary>
        /// <param name="receiveInfo"></param>
        /// <exception cref="MyException"></exception>
        public async Task UpdateReceiveInfo(ReceiveInfoInput receiveInfo)
        {
            if (receiveInfo == default) throw new MyException("参数错误！");

            if (string.IsNullOrWhiteSpace(receiveInfo.ReceiverProvinceName)) throw new MyException("省不能为空！");
            if (string.IsNullOrWhiteSpace(receiveInfo.ReceiverCityName)) throw new MyException("市不能为空！");
            if (string.IsNullOrWhiteSpace(receiveInfo.ReceiverCountyName)) throw new MyException("区不能为空！");
            if (string.IsNullOrWhiteSpace(receiveInfo.ReceiverDetailInfo)) throw new MyException("详细地址不能为空！");
            if (string.IsNullOrWhiteSpace(receiveInfo.ReceiverName)) throw new MyException("收件人姓名不能为空！");
            if (string.IsNullOrWhiteSpace(receiveInfo.ReceiverMobile)) throw new MyException("收件人联系方式不能为空！");

            var order =await _unitWork.GetSingleAsync<MallOrder>(x => x.Id == receiveInfo.OrderId);
            if (order == default) throw new MyException("订单不存在！");
            if (order.Status != CommonConst.MallOrderStatus_0 && order.Status != CommonConst.MallOrderStatus_1) throw new MyException("订单已发货，无法修改地址！");
            order.ReceiveInfo = JsonUtility.Serialize(receiveInfo);
            _unitWork.Update(order);
            await _unitWork.CommitAsync();
        }
        #endregion
        #region 订单下单支付
        public async Task<QrcodeModel> OrderPayAsync(H5MallOrderPayInput model)
        {
            QrcodeModel result = default;
            #region 加锁部分
            var lockKey = ReflectionUtility.GetCurrentMethodFullName("lockKey");
            var timeoutSeconds = 20;
            LockController cacheLock = default;
            try
            {
                cacheLock = _cache.Lock(lockKey, timeoutSeconds);
                if (cacheLock != default)
                {
                    result = await SaveOrderPay(model);
                    cacheLock.Unlock();
                }
            }
            finally
            {
                cacheLock?.Unlock();
                cacheLock?.Dispose();
            }
            #endregion
            return result;
        }

        private async Task<QrcodeModel> SaveOrderPay(H5MallOrderPayInput model)
        {
            var loginUser = await _authenticationPrincipalService.GetAuthenticatedUserAsync();
            //var loginUser = new User() {  Mobile="13918110536",Id=1};
            if (string.IsNullOrWhiteSpace(loginUser.Mobile))
            {
                throw new MyException("用户信息错误！");
            }
            if (model == default)
            {
                throw new MyException("参数错误！");
            }
            if (model.PayAmount <= 0)
            {
                throw new MyException("支付金额错误！");
            }
            if (model.OrderItems == default || model.OrderItems.Count == 0)
            {
                throw new MyException("订单明细错误！");
            }
            var skuList = _unitWork.Query<MallProductSku>(x => model.OrderItems.Select(y => y.ProductSkuId).Contains(x.Id)).ToList();
            var productList = _unitWork.Query<MallProduct>(x => skuList.Select(y => y.MallProductId).Contains(x.Id)).ToList();

            int sumGoodsPrice = GetSumGoodsPrice(model.OrderItems, skuList);
            if (Math.Abs(sumGoodsPrice - model.PayAmount * 100) > 2m)
            {
                throw new MyException($"订单金额错误，请重新下单！", CommonConst.Yes);
            }
            var orderId = _snowflakeIdWorker.NextId();
            string msgSrcId = _configuration["RemoteServices:CloudBankMallH5:MsgSrcId"];//银联提供的

            MallOrder order = new MallOrder
            {
                Id = orderId,
                OrderNo = $"{msgSrcId}{orderId}",
                CreatedAt = DateTime.Now,
                PayPrice = (model.PayAmount * 100).ToInt(),
                Status = CommonConst.MallOrderStatus_0,
                UserId = loginUser.Id,
                TotalPrice = (model.PayAmount * 100).ToInt(),
                ReceiveInfo = JsonUtility.Serialize(model.ReceiveInfo),
                NumberOfInstallments = model.NumberOfInstallments,
                Remark = model.Remark
            };
            List<AdminMallSkuStockChangeInfoInput> changeStockModelList = new List<AdminMallSkuStockChangeInfoInput>();
            model.OrderItems.ForEach(async item =>
            {
                var detailId = _snowflakeIdWorker.NextId();
                var sku = skuList.FirstOrDefault(x => x.Id == item.ProductSkuId);
                var product = productList.FirstOrDefault(x => x.Id == sku.MallProductId);
                var orderDetail = new MallOrderDetail
                {
                    Id = detailId,
                    MallOrderId = orderId,
                    OrderNo = $"{msgSrcId}{detailId}",
                    ProductPrice = sku.SkuPrice,
                    ProductImgs = sku.SkuImg,
                    ProductName = product.ProductName,
                    ProductQuantity = item.ProductQuantity,
                    ProductSkuAttrs = item.ProductSkuAttrs,
                    ProductSkuId = item.ProductSkuId,
                    Status = CommonConst.MallOrderDetailStatus_0

                };
                if (sku.SkuStock <= 0)
                {
                    throw new MyException($"商品【{product.ProductName}{await _mallProductAttrKeyService.GetAttrStringAsync(item.ProductSkuAttrs)}】已售罄！");
                }
                if (sku.SkuStock < item.ProductQuantity)
                {
                    throw new MyException($"商品【{product.ProductName}{await _mallProductAttrKeyService.GetAttrStringAsync(item.ProductSkuAttrs)}】库存不足！");
                }

                var changeStockModel = new AdminMallSkuStockChangeInfoInput
                {
                    ChangeFreezeStockCount = item.ProductQuantity,
                    ChangeSkuStockCount = item.ProductQuantity,
                    StockType = 0,
                    StockDetailType = CommonConst.ProductInvOrRelType_OrderSaleRel,
                    SkuId = orderDetail.ProductSkuId,
                };
                changeStockModelList.Add(changeStockModel);
                _unitWork.Add(orderDetail);
            });

            _mallProductSkuService.SaveStock(changeStockModelList, _unitWork, loginUser.Id);
            var thirdOrderNo = $"{order.OrderNo}{TimeExtension.CurrentTimeFFString()}";
            order.ThirdOrderNo= thirdOrderNo;
            //商城后台保存订单
            _unitWork.Add<MallOrder>(order);

            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProductSku>();

            QrcodeQueryModel qrcodeQueryModel = new QrcodeQueryModel()
            {
                merOrderId = thirdOrderNo,
                msgId = $"{order.Id}",
                totalAmount = order.PayPrice,
                limitInstalNumList = model.NumberOfInstallments.ToString(),
                mobile = EncryptionUtility.Base64_Encode(loginUser.Mobile)
            };
            try
            {
                var result = await _bankMallH5Service.GetQrcode(qrcodeQueryModel);

                if (result.ErrCode != "SUCCESS")
                {
                    _logger.LogError($"{result}");
                    throw new MyException($"支付请求错误:{result.ErrMsg}");
                }
                //银联支付
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"银联分期支付获取付款码请求异常，订单id：{order.Id},异常信息：{ex}");
                throw new MyException("支付请求异常！");
            }
        }

        /// <summary>
        /// 获取订单商品总价格 (单位：分)
        /// </summary>
        /// <param name="orderItems"></param>
        /// <param name="skuList"></param>
        /// <returns></returns>
        private int GetSumGoodsPrice(ICollection<H5MallOrderSettleList> orderItems, List<MallProductSku> skuList)
        {
            int sumGoodsPrice = 0;
            MallProductSku sku = default;
            orderItems.ToList().ForEach(item =>
            {
                sku = skuList.FirstOrDefault(x => x.Id == item.ProductSkuId);
                if (sku != default)
                {
                    sumGoodsPrice += item.ProductQuantity * sku.SkuPrice;
                }
            });
            return sumGoodsPrice;
        }

        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <param name="data"></param>
        public async Task<string> PayNotityUpdateOrder(string data)
        {
            string result = default;
            #region 加锁部分
            var lockKey = ReflectionUtility.GetCurrentMethodFullName("lockKey");
            var timeoutSeconds = 20;
            LockController cacheLock = default;
            try
            {
                var flowDto = JsonUtility.Deserialize<MallOrderCashFlowDto>( data);
                var flowEntity = flowDto.MapTo<MallOrderCashFlow>();
                string md5Key = _configuration["RemoteServices:CloudBankMallH5:Md5Key"];
                var waitSign = $"{ApiSignUtility.GetSortParam(data, "sign")}{md5Key}";
                var sign = EncryptionUtility.GetSHA256hash(waitSign).ToUpper();
                _logger.LogError($"支付或退款的签名{DateTime.Now},银联的sign：{flowDto.Sign}---我们的sign：{sign}");
                if (sign != flowDto.Sign)
                {
                    _logger.LogError($"支付或退款结果通知失败：签名错误");
                    return "FAILED";
                }
                cacheLock = _cache.Lock(lockKey, timeoutSeconds);
                if (cacheLock != default)
                {
                    result = await SaveNotifyFlow(flowEntity);
                    cacheLock.Unlock();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"支付或退款结果通知失败：{ex.ToString()}");
                return "FAILED";
            }
            finally
            {
                cacheLock?.Unlock();
                cacheLock?.Dispose();
            }
            #endregion
            return result;

            //_logger.LogError($"支付或退款{DateTime.Now},参数：{formData}");
            //string result = default;
            //#region 加锁部分
            //var lockKey = ReflectionUtility.GetCurrentMethodFullName("lockKey");
            //var timeoutSeconds = 20;
            //LockController cacheLock = default;
            //try
            //{
            //    var dicReq = JsonUtility.Deserialize<Dictionary<string, string>>(formData);
            //    MallOrderCashFlow mallOrderCashFlow = JsonUtility.Deserialize<MallOrderCashFlow>(formData);
            //    //MallOrderCashFlow mallOrderCashFlow = data.MapTo<MallOrderCashFlow>();
            //    string md5Key = _configuration["RemoteServices:CloudBankMallH5:Md5Key"];
            //    var waitSign = $"{ApiSignUtility.GetSortParam(formData, "sign")}{md5Key}";
            //    var sign = EncryptionUtility.MD5(waitSign).ToUpper();
            //    if (sign != dicReq["sign"].ToString())
            //    {
            //        _logger.LogError($"支付或退款结果通知失败：签名错误");
            //        return "FAILED";
            //    }
            //    cacheLock = _cache.Lock(lockKey, timeoutSeconds);
            //    if (cacheLock != default)
            //    {
            //        result =  SaveNotifyFlow(mallOrderCashFlow);
            //        cacheLock.Unlock();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"支付或退款结果通知失败：{JsonUtility.Serialize(ex)}");
            //    return "FAILED";
            //}
            //finally
            //{
            //    cacheLock?.Unlock();
            //    cacheLock?.Dispose();
            //}
            //#endregion
            //return result;



        }
        private async Task<string> SaveNotifyFlow(MallOrderCashFlow data)
        {
            //支付成功
            if (data.Status == CommonConst.JFQStatus_TRADE_SUCCESS)
            {
              
                var order =await _unitWork.GetSingleAsync<MallOrder>(x => x.ThirdOrderNo == data.MerOrderId);
                if (order == default)
                {
                    _logger.LogError($"支付或退款结果通知失败：订单{data.MerOrderId}不存在！");
                    return "FAILED";
                }
                if (order.Status != CommonConst.MallOrderStatus_0)
                {
                    _logger.LogError($"支付或退款结果通知失败：订单{data.MerOrderId}状态不是待付款状态！");
                    return "FAILED";
                }

                var orderDetailList = _unitWork.Query<MallOrderDetail>(x => x.MallOrderId == order.Id && x.Status == CommonConst.MallOrderDetailStatus_0).ToList();
                if (orderDetailList == default || orderDetailList.Count == 0)
                {

                    _logger.LogError($"支付或退款结果通知失败：订单{data.MerOrderId}没有待付款状态的明细！");
                    return "FAILED";
                }

                order.Status = CommonConst.MallOrderStatus_1;
                order.PaymentTime = DateTime.Now;
                data.NotifyDate = DateTime.Now;

                orderDetailList.ForEach(item =>
                {
                    item.Status = CommonConst.MallOrderDetailStatus_1;
                    _unitWork.Update(item);
                });
                _unitWork.Update(order);

            }
            //退款成功
            else if (data.Status == CommonConst.JFQStatus_TRADE_REFUND)
            {
                var orderDetail =await  _unitWork.GetSingleAsync<MallOrderDetail>(x => x.ThirdReturnOrderNo == data.RefundOrderId);
                if (orderDetail == default)
                {
                    _logger.LogError($"支付或退款结果通知失败：明细单{data.RefundOrderId}不存在！");
                    return "FAILED";
                }
                if (orderDetail.Status != CommonConst.MallOrderDetailStatus_5)
                {
                    _logger.LogError($"支付或退款结果通知失败：明细单{data.RefundOrderId}状态不是退款中状态！");
                    return "FAILED";
                }
                var orderReturn =await  _unitWork.GetSingleAsync<MallOrderReturn>(x => x.MallOrderDetailId == orderDetail.Id && x.Status == 1);
                if (orderReturn != default)
                {
                    //orderReturn.DetailStatus = CommonConst.MallOrderDetailStatus_6;
                    _unitWork.Update(orderReturn);

                    List<AdminMallSkuStockChangeInfoInput> list = new List<AdminMallSkuStockChangeInfoInput>()
                    {
                        new AdminMallSkuStockChangeInfoInput()
                        {
                             ChangeFreezeStockCount=0,
                             ChangeSkuStockCount=orderReturn.ProductQuantity,
                             StockType=CommonConst.StockType_1,
                             StockDetailType=CommonConst.ProductInvOrRelType_OrderReturnInv,
                             SkuId=orderDetail.ProductSkuId,
                        }
                    };

                    _mallProductSkuService.SaveStock(list, _unitWork,0);
                }
                else
                {
                    List<AdminMallSkuStockChangeInfoInput> list = new List<AdminMallSkuStockChangeInfoInput>()
                    {
                        new()
                        {
                             ChangeFreezeStockCount=orderReturn.ProductQuantity,
                             ChangeSkuStockCount=orderReturn.ProductQuantity,
                             StockType=CommonConst.StockType_1,
                             StockDetailType=CommonConst.ProductInvOrRelType_OrderReturnInv,
                             SkuId=orderDetail.ProductSkuId,
                        }
                    };

                    _mallProductSkuService.SaveStock(list, _unitWork,0);
                }
                orderDetail.Status = CommonConst.MallOrderDetailStatus_6;

                _unitWork.Update(orderDetail);
            }
            else
            {
                _logger.LogError($"支付或退款结果通知失败：未知的交易状态{data.Status}");
                return "FAILED";
            }
            _unitWork.Add(data);
            await _unitWork.CommitAsync();

            await _unitWork.RemoveListCacheAsync<MallProductSku>();
            return "SUCCESS";
        }
        public async Task<QrcodeModel> PayById(long id)
        {
            var order =await _unitWork.GetSingleAsync<MallOrder>(x => x.Id == id);
            if (order == default)
            {
                throw new MyException("订单不存在！", CommonConst.No);
            }

            if (order.Status != CommonConst.MallOrderStatus_0)
            {
                throw new MyException("单据不是待付款订单，无法支付！");
            }
            var loginUser = await _authenticationPrincipalService.GetAuthenticatedUserAsync();
            var thirdOrderNo = $"{order.OrderNo}{TimeExtension.CurrentTimeFFString()}";
            order.ThirdOrderNo = thirdOrderNo;
            _unitWork.Update(order);
            await _unitWork.CommitAsync();
            QrcodeQueryModel qrcodeQueryModel = new QrcodeQueryModel()
            {
                merOrderId = thirdOrderNo,
                msgId = $"{order.Id}",
                totalAmount = order.PayPrice,
                isForceLimitInstalNum = order.NumberOfInstallments.ToString(),
                mobile = EncryptionUtility.Base64_Encode(loginUser.Mobile)
            };
            try
            {
                var result = await _bankMallH5Service.GetQrcode(qrcodeQueryModel);

                if (result.ErrCode != "SUCCESS")
                {
                    _logger.LogError($"{result}");
                    throw new MyException($"支付请求错误:{result.ErrMsg}");
                }
                //银联支付
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"银联分期支付获取付款码请求异常，订单id：{order.Id},异常信息：{ex}");
                throw new MyException("支付请求异常！");
            }

        }

        #endregion


        #region 退单&取消
        /// <summary>
        /// 退单（子单退）
        /// </summary>
        /// <param name="input"></param>
        public async Task ReturnDetailOrder(H5MallOrderReturnInput input)
        {

            #region 加锁部分
            var lockKey = ReflectionUtility.GetCurrentMethodFullName("lockKey");
            var timeoutSeconds = 20;
            LockController cacheLock = default;
            try
            {
                cacheLock = _cache.Lock(lockKey, timeoutSeconds);
                if (cacheLock != default)
                {
                    var detailOrder = await _unitWork.GetSingleAsync<MallOrderDetail>(x => x.Id == input.Id);
                    if (detailOrder == default)
                    {
                        throw new MyException("订单不存在！", CommonConst.No);
                    }
                    if (detailOrder.Status != CommonConst.MallOrderDetailStatus_1 && detailOrder.Status != CommonConst.MallOrderDetailStatus_2 && detailOrder.Status != CommonConst.MallOrderDetailStatus_3)
                    {
                        throw new MyException("该单据无法申请退单");
                    }

                    if (await _unitWork.ExistsAsync<MallOrderReturn>(x => (x.Status == 0 || x.Status == 1) && x.OrderNo == detailOrder.OrderNo))
                    {
                        throw new MyException("该单据已申请退单，请勿重复操作！");
                    }
                    var order = await _unitWork.GetSingleAsync<MallOrder>(x => x.Id == detailOrder.MallOrderId);


                    #region 待发货 发货中和已完成  要退款退货申请

                    //待发货同样走申请-xq-2024-09-20
                    //未发货
                    //if (detailOrder.Status == CommonConst.MallOrderDetailStatus_1)
                    //{
                    //    detailOrder.Status = CommonConst.MallOrderDetailStatus_5;//退款

                    //    List<int> statusIdList = new List<int>() { CommonConst.MallOrderDetailStatus_1, CommonConst.MallOrderDetailStatus_2, CommonConst.MallOrderDetailStatus_3, CommonConst.MallOrderDetailStatus_10 };
                    //    if (!_unitWork.ExistsAsync<MallOrderDetail>(x => x.MallOrderId == detailOrder.MallOrderId && !statusIdList.Contains(x.Status) && x.Id != detailOrder.Id))
                    //    {
                    //        order.Status = CommonConst.MallOrderStatus_4;
                    //    }

                    //    _unitWork.Update(detailOrder);
                    //    _unitWork.Update(order);
                    //    _unitWork.Commit();
                    //    OrderReturnQueryModel orderReturnQueryModel = new OrderReturnQueryModel()
                    //    {
                    //        merOrderId = order.OrderNo,
                    //        msgId = order.Id.ToString(),
                    //        refundAmount = detailOrder.ProductPrice,
                    //        refundDesc = input.Remark,
                    //        refundOrderId = detailOrder.OrderNo,
                    //    };
                    //    var res= await _bankMallH5Service.OrderReturn(orderReturnQueryModel);
                    //    if (res.ErrCode != "SUCCESS")
                    //    {
                    //        throw new MyException($"退款失败：{res.ErrMsg}");
                    //    }
                    //}
                    //else
                    //{

                    //}
                    if (detailOrder.Status == CommonConst.MallOrderDetailStatus_1)
                    {
                        detailOrder.Status = CommonConst.MallOrderDetailStatus_4;//退款申请中
                    }
                    else
                    {
                        detailOrder.Status = CommonConst.MallOrderDetailStatus_7;//退货申请中
                    }
                    order.Status = CommonConst.MallOrderStatus_4;
                    var id = _snowflakeIdWorker.NextId();
                    _unitWork.Add<MallOrderReturn>(new MallOrderReturn
                    {
                        RefOrderNo = detailOrder.OrderNo,
                        RefMallOrderNo = order.OrderNo,
                        CreateDate = DateTime.Now,
                        ProductName = detailOrder.ProductName,
                        MallOrderDetailId = detailOrder.Id,
                        //DetailStatus = CommonConst.MallOrderDetailStatus_7,
                        OrderNo = $"MOR{id}",
                        ProductImgs = detailOrder.ProductImgs,
                        ProductPrice = detailOrder.ProductPrice,
                        Remark = input.Remark,
                        ProductQuantity = detailOrder.ProductQuantity,
                        Status = 0,
                        ProductSkuAttrs = detailOrder.ProductSkuAttrs,
                        ProductSkuId = detailOrder.ProductSkuId,
                        UserId = order.UserId,
                        ApplicationReason = input.ApplicationReason,
                        ApplicationDescription = input.ApplicationDescription,
                        ApplicationImgs = input.ApplicationImgs

                    });
                    _unitWork.Update(detailOrder);
                    _unitWork.Update(order);
                    await _unitWork.CommitAsync();
                    #endregion
                    cacheLock.Unlock();
                }
            }
            finally
            {
                cacheLock?.Unlock();
                cacheLock?.Dispose();
            }
            #endregion

        }


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="input"></param>
        public async Task CancelOrder(H5MallOrderReturnInput input)
        {
            #region 加锁部分
            var lockKey = ReflectionUtility.GetCurrentMethodFullName("lockKey");
            var timeoutSeconds = 20;
            LockController cacheLock = default;
            try
            {
                cacheLock = _cache.Lock(lockKey, timeoutSeconds);
                if (cacheLock != default)
                {
                    var order = await _unitWork.GetSingleAsync<MallOrder>(x => x.Id == input.Id);
                    if (order == default)
                    {
                        throw new MyException("订单不存在！", CommonConst.No);
                    }
                    if (order.Status != CommonConst.MallOrderStatus_0)
                    {
                        throw new MyException("单据不是待付款，无法取消");
                    }

                    //已取消
                    order.Status = CommonConst.MallOrderStatus_5;

                    var orderDetailList = _unitWork.Query<MallOrderDetail>(x => x.MallOrderId == order.Id).ToList();

                    List<AdminMallSkuStockChangeInfoInput> changeStockModelList = new List<AdminMallSkuStockChangeInfoInput>();
                    orderDetailList.ForEach(item =>
                    {
                        item.Status = CommonConst.MallOrderDetailStatus_11;

                        var changeStockModel = new AdminMallSkuStockChangeInfoInput()
                        {
                            ChangeFreezeStockCount = item.ProductQuantity,
                            ChangeSkuStockCount = item.ProductQuantity,
                            StockType = CommonConst.StockType_1,
                            StockDetailType = CommonConst.ProductInvOrRelType_OrderSaleRel,
                            SkuId = item.ProductSkuId,
                        };
                        changeStockModelList.Add(changeStockModel);
                        _unitWork.Update(item);
                    });
                    var loginUser = await _authenticationPrincipalService.GetAuthenticatedUserAsync();
                    _mallProductSkuService.SaveStock(changeStockModelList, _unitWork, loginUser.Id);
                    _unitWork.Update(order);
                    await _unitWork.CommitAsync();
                    await _unitWork.RemoveListCacheAsync<MallProductSku>();
                    cacheLock.Unlock();
                }
            }
            finally
            {
                cacheLock?.Unlock();
                cacheLock?.Dispose();
            }
            #endregion

        }
        #endregion


        #region 发货&签收
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        public async Task SendOut(AdminOrderSendOutInput input)
        {
            var order =await _unitWork.GetSingleAsync<MallOrder>(x => x.Id == input.Id);
            if (order == default)
            { throw new MyException("订单不存在！", CommonConst.No); }

            if (order.Status != CommonConst.MallOrderStatus_1)
            {
                throw new MyException("订单状态不是待发货，无法发货！");
            }

            var detailOrderList = _unitWork.Query<MallOrderDetail>(x => x.MallOrderId == order.Id && x.Status == CommonConst.MallOrderDetailStatus_1).ToList();
            if (detailOrderList == default || detailOrderList.Count == 0)
            {
                throw new MyException("单据明细里不包含待发货的子单！");
            }

            List<AdminMallSkuStockChangeInfoInput> changeStockModelList = new List<AdminMallSkuStockChangeInfoInput>();

            detailOrderList.ForEach(item =>
            {
                item.Status = CommonConst.MallOrderDetailStatus_2;

                var changeStockModel = new AdminMallSkuStockChangeInfoInput()
                {
                    ChangeFreezeStockCount = item.ProductQuantity,
                    ChangeSkuStockCount = 0,
                    StockType = CommonConst.StockType_2,
                    StockDetailType = CommonConst.ProductInvOrRelType_OrderSaleRel,
                    SkuId = item.ProductSkuId,
                };
                changeStockModelList.Add(changeStockModel);
                _unitWork.Update(item);
            });
            var loginUser =await  _authenticationPrincipalService.GetAuthenticatedUserAsync();
            _mallProductSkuService.SaveStock(changeStockModelList, _unitWork, loginUser.Id);
            order.Status = CommonConst.MallOrderStatus_2;
            order.DeliveryTime = DateTime.Now;
            order.LogisticsCompany = input.LogisticsCompany;
            order.LogisticsNo = input.LogisticsNo;
            _unitWork.Update(order);
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProductSku>();
        }
        /// <summary>
        /// 签收
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        public async Task SignFor(IdQueryCommonInput input)
        {
            var order =await _unitWork.GetSingleAsync<MallOrder>(x => x.Id == input.Id);
            if (order == default)
            { throw new MyException("订单不存在！", CommonConst.No); }

            if (order.Status != CommonConst.MallOrderStatus_2)
            {
                throw new MyException("订单状态不是已发货，无法签收！", CommonConst.No);
            }

            var detailOrderList = _unitWork.Query<MallOrderDetail>(x => x.MallOrderId == order.Id && x.Status == CommonConst.MallOrderDetailStatus_2).ToList();
            if (detailOrderList == default || detailOrderList.Count == 0)
            {
                throw new MyException("单据明细里不包含已发货的子单！", CommonConst.No);
            }
            detailOrderList.ForEach(item =>
            {
                item.Status = CommonConst.MallOrderDetailStatus_3;

                _unitWork.Update(item);
            });
            order.Status = CommonConst.MallOrderStatus_3;
            order.ReceiveTime = DateTime.Now;
            _unitWork.Update(order);
            await _unitWork.CommitAsync();
        }


        /// <summary>
        /// 自动签收发货7天的单据
        /// </summary>
        /// <exception cref="MyException"></exception>
        public async Task AutoSignFor()
        {
            _logger.LogError($"自动签收开始......！");
            var orderList = _unitWork.Query<MallOrder>(x => x.DeliveryTime < DateTime.Today.AddDays(-7) && x.Status == CommonConst.MallOrderStatus_2).ToList();

            if (orderList != default && orderList.Count > 0)
            {
                orderList.ForEach(order =>
                {
                    var detailOrderList = _unitWork.Query<MallOrderDetail>(x => x.MallOrderId == order.Id && x.Status == CommonConst.MallOrderDetailStatus_2).ToList();
                    if (detailOrderList == default || detailOrderList.Count == 0)
                    {
                        _logger.LogError($"订单号【{order.OrderNo}】单据明细里不包含已发货的子单！");
                    }
                    else
                    {
                        detailOrderList.ForEach(item =>
                        {
                            item.Status = CommonConst.MallOrderDetailStatus_3;
                            _unitWork.Update(item);
                        });

                    }
                    order.Status = CommonConst.MallOrderStatus_3;
                    order.ReceiveTime = DateTime.Now;
                });
            }

            _unitWork.UpdateRange(orderList);
            await _unitWork.CommitAsync();
            _logger.LogError($"自动签收结束......！");
        }
        #endregion


        public async Task<FuQingNoQueryResultResponse> GetOrderLogisticsById(IdQueryCommonInput input)
        {
            var dto = await GetSingleByIdAsync(input);
            var logisticNo = dto.LogisticsNo;
            if (!string.IsNullOrWhiteSpace(dto.ReceiveInfo))
            {
                dto.ReceiveInfoModel = JsonUtility.Deserialize<ReceiveInfoInput>(dto.ReceiveInfo);
            }
            var phoneNo = dto.ReceiveInfoModel.ReceiverMobile;
            if (dto.LogisticsCompany == CommonConst.LogisticsType_SF)
            {
                if (phoneNo.Length < 4)
                {
                    throw new MyException("顺丰快递单号查询物流需要收件人手机号后四位，收件人手机号长度低于4位");
                }
                logisticNo = $"{logisticNo}:{phoneNo.Substring(phoneNo.Length - 4, 4)}";
            }
            var ret = await _logisticsQuery.NoQueryResult(new LogisticsRequest { No = logisticNo });


            var globalDataDetailList = _listCacheGlobalDataDetailRepository.Query(x => x.Code == CommonConst.MallOrderStatus || x.Code == CommonConst.LogisticsType).ToList();
            dto.StatusName = globalDataDetailList.FirstOrDefault(x => x.Code == CommonConst.MallOrderStatus && x.ConstKey == dto.Status.ToString())?.Name;
            var globalDataDetailLogistics = globalDataDetailList.FirstOrDefault(x => x.Code == CommonConst.LogisticsType && x.ConstKey == dto.LogisticsCompany);
            if (globalDataDetailLogistics != null)
            {
                dto.LogisticsCompanyName = globalDataDetailLogistics.Name;
                dto.LogisticsCompanyLogo = globalDataDetailLogistics.Remark;
            };

            ret.LogisticsCompany = dto.LogisticsCompany;
            ret.LogisticsNo = dto.LogisticsNo;
            ret.LogisticsCompanyLogo = dto.LogisticsCompanyLogo;
            ret.LogisticsCompanyName = dto.LogisticsCompanyName;
            ret.OrderStatus = dto.StatusName;
            return ret;
        }
    }
}
