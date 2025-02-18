using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloud.Core.Module
{
    public class ApplicationShutdownContext
    {
        public IServiceProvider ServiceProvider { get; }

        public ApplicationShutdownContext([NotNull] IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
