using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.Core
{
    public interface ICloudLock
    {
        Task<bool> TryAdd(string key);

        Task TryRemove(string key);

        Task Clear(string key);
    }
}
