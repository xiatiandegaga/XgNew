using Cloud.DynamicExpress;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Cloud.Extensions
{
    public static class CoreRequestFormExtension
    {
        public static FilterCollection GetCloudDynamicExpressFilter(this HttpRequest request)
        {
            FilterCollection collectFilter = new FilterCollection();
            var keys = request.Form.Keys.Where(k => k.StartsWith("STD$"));
            string[] arrkey;
            string value;
            foreach (string key in keys)
            {
                value = request.Form[key][0];
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                arrkey = key.Split('$');
                collectFilter.Add(new List<DynamicFilter>() { new DynamicFilter { PropertyName = arrkey[2], Op = (Operation)Convert.ToInt32(arrkey[1]), Value = value } });
            }
            if (collectFilter.Count < 1)
            {
                return null;
            }

            return collectFilter;
        }

        public static Expression<Func<T, bool>> GetCloudDynamicExpress<T>(this List<DynamicFilter> lst_filter)
        {
            if (lst_filter == null || lst_filter.Count == 0)
            {
                return null;
            }

            FilterCollection collectFilter = new ();
            foreach (var item in lst_filter)
            {
                collectFilter.Add(new List<DynamicFilter> { item });
            }

            if (collectFilter.Count < 1)
            {
                return null;
            }

            return LambdaExpressionBuilder.GetExpression<T>(collectFilter);
        }
    }
}
