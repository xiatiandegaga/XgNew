using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloud.Core.Module.PlugIns
{
    public class PlugInSourceList : List<IPlugInSource>
    {
        [NotNull]
        public virtual Type[] GetAllModules()
        {
            return this
                .SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies())
                .Distinct()
                .ToArray();
        }
    }
}
