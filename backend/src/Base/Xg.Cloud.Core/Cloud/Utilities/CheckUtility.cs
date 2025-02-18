using Cloud.Extensions;
using Cloud.Models;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cloud.Utilities
{
    [DebuggerStepThrough]
    public static class CheckUtility
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            T value, 
            [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new MyException($"{parameterName}can not be null");
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            T value, 
            [InvokerParameterName] [NotNull] string parameterName, 
            string message)
        {
            if (value == null)
            {
                throw new MyException($"{parameterName}can not be null-message-{message}");
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotNull(
            string value,
            [InvokerParameterName] [NotNull] string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (value == null)
            {
                throw new MyException($"{parameterName} can not be null-{parameterName}");
            }

            if (value.Length > maxLength)
            {
                throw new MyException($"{parameterName} length must be equal to or lower than {maxLength}-{parameterName}");
            }

            if (minLength > 0 && value.Length < minLength)
            {
                throw new MyException($"{parameterName} length must be equal to or bigger than {minLength}-{parameterName}");
            }

            return value;
        }
    }
}
