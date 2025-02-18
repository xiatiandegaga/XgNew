using JetBrains.Annotations;
using System;
using System.Text;

namespace Cloud.Core.Module.PlugIns
{
    public interface IPlugInSource
    {
        [NotNull]
        Type[] GetModules();
    }
}
