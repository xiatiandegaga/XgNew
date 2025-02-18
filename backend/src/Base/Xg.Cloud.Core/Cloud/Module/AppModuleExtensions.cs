using System;
using System.Collections.Generic;
using System.Text;

namespace Cloud.Core.Module
{
    public static class AppModuleExtensions
    {
        /// <summary>
        /// 模块接口类型全名称
        /// </summary>
        public static string ModuleInterfaceTypeFullName { get; } = typeof(IAppModule).FullName;

        /// <summary>
        /// 是否实现了 IAppModule 接口
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public static bool IsModule(this Type moduleType)
        {

            // 过滤抽象类、接口、泛型类、非类
            if (moduleType.IsAbstract
                || moduleType.IsInterface
                || moduleType.IsGenericType
                || !moduleType.IsClass)
            {
                return false;
            }

            // 过滤没有实现IRModule接口的类
            var baseInterfaceType = moduleType.GetInterface(ModuleInterfaceTypeFullName, false);
            if (baseInterfaceType == null)
            {
                return false;
            }


            return true;
        }

        /// <summary>
        /// 检查模块类型是否实现了 IAppModule 接口
        /// </summary>
        /// <param name="moduleType"></param>
        public static void CheckModuleType(this Type moduleType)
        {
            if (!IsModule(moduleType))
            {
                throw new ArgumentException("Given type is not an IAppModule module: " + moduleType.AssemblyQualifiedName);
            }
        }
    }
}
