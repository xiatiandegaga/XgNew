using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Models
{
    public class DataTableColumnModel
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public dynamic FieldValue { get; set; }
    }
}
