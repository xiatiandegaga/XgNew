using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloud.Utilities
{
    public static class AssemblyUtility
    {
        public static Assembly[] GetCallingAssembly(Func<AssemblyName, bool> assemblyPredicate)
        {
            //获取调用当前函数的函数的程序集的文件
            AssemblyName[] assemblyNames = Assembly.GetCallingAssembly().GetReferencedAssemblies().Where(assemblyPredicate).ToArray();
            List<Assembly> lstAssembly = assemblyNames.Select(n => Assembly.Load(n)).ToList();
            Assembly[] assemblies = lstAssembly.ToArray();
            return assemblies;
        }

        public static Assembly[] GetExecutingAssembly(Func<AssemblyName, bool> assemblyPredicate)
        {
            //获取的是当前执行的方法所在的程序文件-如当前类在xg.cloud.core里面
            AssemblyName[] assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(assemblyPredicate).ToArray();
            List<Assembly> lstAssembly = assemblyNames.Select(n => Assembly.Load(n)).ToList();
            Assembly[] assemblies = lstAssembly.ToArray();
            return assemblies;
        }

        public static Assembly[] GetEntryAssembly(Func<AssemblyName, bool> assemblyPredicate)
        {
            //获取当前应用程序第一个启动的程序（从 identity启动就是identity）
            AssemblyName[] assemblyNames = Assembly.GetEntryAssembly().GetReferencedAssemblies().Where(assemblyPredicate).ToArray();
            List<Assembly> lstAssembly = assemblyNames.Select(n => Assembly.Load(n)).ToList();
            Assembly[] assemblies = lstAssembly.ToArray();
            return assemblies;
        }
    }
}
