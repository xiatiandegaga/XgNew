using System.Diagnostics;
using System.Linq;

namespace Cloud.Utilities
{
    public class ProcessUtility
    {
        public static long IpToLong(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
        }

        public static int GetPid() => Process.GetCurrentProcess().Id;

        public static string GetIp => System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                  .Select(p => p.GetIPProperties())
                  .SelectMany(p => p.UnicastAddresses)
                  .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
                  .FirstOrDefault()?.Address.ToString();
        public static long GetLongIp() => IpToLong(GetIp);

    }
}
