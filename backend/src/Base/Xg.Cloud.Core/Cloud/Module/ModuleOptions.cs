using JetBrains.Annotations;
using Cloud.Core.Module.PlugIns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloud.Core.Module
{
    public class ModuleOptions
    {
        [NotNull]
        public PlugInSourceList PlugInSources { get; }

        public ModuleOptions()
        {
            PlugInSources = new PlugInSourceList();
        }
    }
}
