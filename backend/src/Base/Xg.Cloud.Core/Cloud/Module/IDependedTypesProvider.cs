using JetBrains.Annotations;
using System;

namespace Cloud.Core.Module
{
    public interface IDependedTypesProvider
    {
        /// <summary>
        /// 获取依赖的类型
        /// </summary>
        /// <returns></returns>
        [NotNull]
        Type[] GetDependedTypes();
    }
}
