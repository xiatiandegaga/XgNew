using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.Core
{
    public class CloudLock:ICloudLock
    {
        private readonly ConcurrentDictionary<string, byte> lockObejcts = new();

        public Task<bool> TryAdd(string key)
        {
            if (lockObejcts.TryAdd(key, 1))
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task TryRemove(string key)
        {
            lockObejcts.TryRemove(key, out _);
            return Task.CompletedTask;
        }

        public Task Clear(string key)
        {
            lockObejcts.Clear();
            return Task.CompletedTask;
        }
    }
}
