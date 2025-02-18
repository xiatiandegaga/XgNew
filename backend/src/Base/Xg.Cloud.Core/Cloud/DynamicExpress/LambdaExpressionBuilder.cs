using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cloud.DynamicExpress
{
    public class LambdaExpressionBuilder
    {
        private static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
        private static readonly MethodInfo lstLongContainsMethod = typeof(List<long>).GetMethod("Contains");
        private static readonly MethodInfo startsWithMethod =
                                typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static readonly MethodInfo endsWithMethod =
                                typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
        private static Expression GetExpression(ParameterExpression param, DynamicFilter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            Expression handledMember = member;
            ConstantExpression constant;
            var memberType = ((PropertyInfo)member.Member).PropertyType;
            if (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                handledMember = Expression.Property(member, memberType.GetProperty("Value"));
                constant = Expression.Constant(Convert.ChangeType(filter.Value, Nullable.GetUnderlyingType(memberType)));
            }
            else
            {
                if (filter.Value == default)
                    return default;

                if (memberType == typeof(List<long>))
                {
                    constant = Expression.Constant(Convert.ChangeType(filter.Value.ToString().Split(',').ToList().ConvertAll(i => Int64.Parse(i)), typeof(List<long>)));
                }
                else
                {
                    constant = Expression.Constant(Convert.ChangeType(filter.Value, memberType));
                }
            }

            return filter.Op switch
            {
                Operation.Equals => Expression.Equal(handledMember, constant),
                Operation.GreaterThan => Expression.GreaterThan(handledMember, constant),
                Operation.GreaterThanOrEqual => Expression.GreaterThanOrEqual(handledMember, constant),
                Operation.LessThan => Expression.LessThan(handledMember, constant),
                Operation.LessThanOrEqual => Expression.LessThanOrEqual(handledMember, constant),
                Operation.Contains => Expression.Call(handledMember, containsMethod, constant),
                Operation.StartsWith => Expression.Call(handledMember, startsWithMethod, constant),
                Operation.EndsWith => Expression.Call(handledMember, endsWithMethod, constant),
                Operation.RevertContains => Expression.Call(constant, containsMethod, handledMember),
                Operation.NotEqual => Expression.NotEqual(handledMember, constant),
                Operation.LongRevertContains => Expression.Call(constant, lstLongContainsMethod, handledMember),

                _ => null,
            };
        }
        private static BinaryExpression GetORExpression(ParameterExpression param, DynamicFilter filter1, DynamicFilter filter2)
        {
            Expression bin1 = GetExpression(param, filter1);
            Expression bin2 = GetExpression(param, filter2);

            return Expression.Or(bin1, bin2);
        }

        private static Expression GetExpression(ParameterExpression param, IList<DynamicFilter> orFilters)
        {
            if (orFilters.Count == 0)
                return null;

            Expression exp = null;

            if (orFilters.Count == 1)
            {
                exp = GetExpression(param, orFilters[0]);
            }
            else if (orFilters.Count == 2)
            {
                exp = GetORExpression(param, orFilters[0], orFilters[1]);
            }
            else
            {
                while (orFilters.Count > 0)
                {
                    var f1 = orFilters[0];
                    var f2 = orFilters[1];

                    if (exp == null)
                    {
                        exp = GetORExpression(param, orFilters[0], orFilters[1]);
                    }
                    else
                    {
                        exp = Expression.Or(exp, GetORExpression(param, orFilters[0], orFilters[1]));
                    }
                    orFilters.Remove(f1);
                    orFilters.Remove(f2);

                    if (orFilters.Count == 1)
                    {
                        exp = Expression.Or(exp, GetExpression(param, orFilters[0]));
                        orFilters.RemoveAt(0);
                    }
                }
            }

            return exp;
        }

        public static Expression<Func<T, bool>> GetExpression<T>(FilterCollection filters)
        {
            if (filters == null || filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression(param, filters[0]);
            }
            else if (filters.Count == 2)
            {
                exp = Expression.AndAlso(GetExpression(param, filters[0]), GetExpression(param, filters[1]));
            }
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];
                    var f1Andf2 = Expression.AndAlso(GetExpression(param, filters[0]), GetExpression(param, filters[1]));
                    if (exp == null)
                    {
                        exp = f1Andf2;
                    }
                    else
                    {
                        exp = Expression.AndAlso(exp, f1Andf2);
                    }

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }
            if (exp == default)
                return default;
            return Expression.Lambda<Func<T, bool>>(exp, param);
        }
    }
}
