using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.Core
{
    public static class CommonConst
    {
        public const string Tips_DataNull = "不可为空";

        #region 通用状态-AjaxCode回执
        /// <summary>
        /// 成功
        /// </summary>
        public const int Ajax_Success = 1;
        /// <summary>
        /// 失败
        /// </summary>
        public const int Ajax_Fail = 2;
        #endregion

        #region 是否删除-正常已删除
        /// <summary>
        /// 正常
        /// </summary>
        public const int DeletedStatus_Normal = 1;
        /// <summary>
        /// 已删除
        /// </summary>
        public const int DeletedStatus_Deleted = 2;
        #endregion
        #region 菜单表类型（1：菜单 2：按钮）
        /// <summary>
        /// 菜单
        /// </summary>
        public const int MenuCategory_Menu = 1;
        /// <summary>
        /// 按钮
        /// </summary>
        public const int MenuCategory_Button = 2;
        #endregion
        #region 是否
        /// <summary>
        /// 是
        /// </summary>
        public const int No = 0;
        /// <summary>
        /// 否
        /// </summary>
        public const int Yes = 1;
        #endregion

        #region 银联聚分期
        #region 银联聚分期结果通知状态
        /// <summary>
        /// 支付成功状态
        /// </summary>
        public const string JFQStatus_TRADE_SUCCESS = "TRADE_SUCCESS";
        /// <summary>
        /// 退款成功状态
        /// </summary>
        public const string JFQStatus_TRADE_REFUND = "TRADE_REFUND";
        #endregion
        #endregion
        /// <summary>
        /// 超小      
        /// </summary>
        public const int TaskCount_ExtraSmall = 2;

        /// <summary>
        /// 小      
        /// </summary>
        public const int TaskCount_Small = 4;

        /// <summary>
        /// 超中      
        /// </summary>
        public const int TaskCount_ExtraMedium = 8;

        /// <summary>
        /// 中
        /// </summary>
        public const int TaskCount_Medium = 16;

        /// <summary>
        /// 大
        /// </summary>

        public const int TaskCount_Large = 24;

        /// <summary>
        /// 大
        /// </summary>

        public const int TaskCount_ExtraLarge = 32;
        #region 出入库类型
        /// <summary>
        /// 入库
        /// </summary>
        public const int StockType_1 = 1;

        /// <summary>
        /// 出库
        /// </summary>
        public const int StockType_2 = 2;
        #endregion

      
        #region 数据字典主表code常量
        /// <summary>
        /// H5商城订单状态
        /// </summary>

        public const string MallOrderStatus = "MallOrderStatus";
        /// <summary>
        /// H5商城订单详细状态
        /// </summary>

        public const string MallOrderDetailStatus = "MallOrderDetailStatus";


        /// <summary>
        /// 商品入库类型
        /// </summary>

        public const string ProductInvOrRelType = "ProductInvOrRelType";



        /// <summary>
        /// 物流类型
        /// </summary>

        public const string LogisticsType = "LogisticsType";

        /// <summary>
        /// 退单申请原因
        /// </summary>

        public const string OrderReturnApplicationReason = "OrderReturnApplicationReason";

        
        #endregion

        #region 数据字典子表 Code 常量

        #region H5商城订单状态
        /// <summary>
        /// 待付款
        /// </summary>
        public const int MallOrderStatus_0 = 0;
        /// <summary>
        /// 已付款待发货
        /// </summary>
        public const int MallOrderStatus_1 = 1;
        /// <summary>
        /// 已发货
        /// </summary>
        public const int MallOrderStatus_2 = 2;
        /// <summary>
        /// 已完成
        /// </summary>
        public const int MallOrderStatus_3 = 3;
        /// <summary>
        /// 售后
        /// </summary>
        public const int MallOrderStatus_4 = 4;
        /// <summary>
        /// 已取消
        /// </summary>
        public const int MallOrderStatus_5 = 5;
        #endregion

        #region H5商城订单详细状态
        /// <summary>
        /// 待付款
        /// </summary>
        public const int MallOrderDetailStatus_0 = 0;
        /// <summary>
        /// 已付款待发货
        /// </summary>
        public const int MallOrderDetailStatus_1 = 1;
        /// <summary>
        /// 已发货
        /// </summary>
        public const int MallOrderDetailStatus_2 = 2;
        /// <summary>
        /// 已完成
        /// </summary>
        public const int MallOrderDetailStatus_3 = 3;
        /// <summary>
        /// 退款申请中
        /// </summary>
        public const int MallOrderDetailStatus_4 = 4;
        /// <summary>
        /// 退款中
        /// </summary>
        public const int MallOrderDetailStatus_5 = 5;
        /// <summary>
        /// 已退款
        /// </summary>
        public const int MallOrderDetailStatus_6 = 6;
        /// <summary>
        /// 退货申请中
        /// </summary>
        public const int MallOrderDetailStatus_7 = 7;
        /// <summary>
        /// 退货中
        /// </summary>
        public const int MallOrderDetailStatus_8 = 8;
        /// <summary>
        /// 已退货
        /// </summary>
        public const int MallOrderDetailStatus_9 = 9;
        /// <summary>
        /// 已评价
        /// </summary>
        public const int MallOrderDetailStatus_10 = 10;
        /// <summary>
        /// 已取消
        /// </summary>
        public const int MallOrderDetailStatus_11 = 11;

        /// <summary>
        /// 已拒绝
        /// </summary>
        public const int MallOrderDetailStatus_12 = 12;

        /// <summary>
        ///未发货退款申请通过
        /// </summary>
        public const int MallOrderDetailStatus_13 = 13;
        #endregion

        #region 出入库类型明细
        /// <summary>
        /// 库存入库
        /// </summary>
        public const string ProductInvOrRelType_StockInv = "StockInv";

        /// <summary>
        /// 订单退单入库
        /// </summary>
        public const string ProductInvOrRelType_OrderReturnInv = "OrderReturnInv";

        #endregion

        #region 出入库类型明细

        /// <summary>
        /// 库存出库
        /// </summary>
        public const string ProductInvOrRelType_StockRel = "StockRel";

        /// <summary>
        /// 订单销售出库
        /// </summary>
        public const string ProductInvOrRelType_OrderSaleRel = "OrderSaleRel";

        #endregion

        /// <summary>
        /// 顺丰物流
        /// </summary>
        public const string LogisticsType_SF = "SF";

        /// <summary>
        /// 京东物流
        /// </summary>
        public const string LogisticsType_JD = "JD";
        #endregion



    }
}
