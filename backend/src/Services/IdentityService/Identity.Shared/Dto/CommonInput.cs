using Cloud.DynamicExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto
{
    /// <summary>
    /// 分页查询model
    /// </summary>
    public class PageQueryCommonInput
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }
        ///// <summary>
        ///// 排序-非必传，除非有特殊排序需求
        ///// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// 条件格式如：[{PropertyName:'字段名',Op:操作符,Value:输入的值}] 其中操作符（0-等于 1-大于 2-小于 3-大于等于 4-小于等于 5-包含 6-反包含 7-不等于 8-时间startWith 9-时间endWith 10-long类型反包含）
        /// </summary>
        public List<DynamicFilter> Filter { get; set; } = null;
    }

    /// <summary>
    /// 全部查询model
    /// </summary>
    public class AllQueryCommonInput
    {
        /// <summary>
        /// 排序-非必传，除非有特殊排序需求
        /// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// 条件格式如：[{PropertyName:'字段名',Op:操作符,Value:输入的值}] 其中操作符（0-等于 1-大于 2-小于 3-大于等于 4-小于等于 5-包含 6-反包含 7-不等于 8-时间startWith 9-时间endWith 10-long类型反包含）
        /// </summary>
        public List<DynamicFilter> Filter { get; set; }
    }

    /// <summary>
    /// CodeQueryModel
    /// </summary>
    public class CodeQueryCommonInput
    {
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// CodeQueryModel
    /// </summary>
    public class CodesQueryCommonInput
    {
        /// <summary>
        /// Code
        /// </summary>
        public string Codes { get; set; }
    }

    /// <summary>
    /// IdQueryModel
    /// </summary>
    public class IdQueryCommonInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
    }

    public class ListIdQueryCommonInput
    {
        public List<long> ListId { get; set; }
    }
}
