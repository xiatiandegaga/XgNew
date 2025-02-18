using Cloud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Snowflake
{
    public class SeataSnowflakeIdWorker : ISnowflakeIdWorker
    {
        /// <summary>
        /// 开始时间截 (new DateTime(2020, 1, 1).ToUniversalTime() - Jan1st1970).TotalMilliseconds
        /// </summary>
        public const long Twepoch = 1577808000000L;

        /// <summary>
        /// The number of bits occupied by workerId
        /// </summary>
        private const int WorkerIdBits = 10;

        /// <summary>
        /// The number of bits occupied by timestamp
        /// </summary>
        private const int TimestampBits = 41;

        /// <summary>
        /// The number of bits occupied by sequence
        /// </summary>
        private const int SequenceBits = 12;

        /// <summary>
        /// Maximum supported machine id, the result is 1023
        /// </summary>
        private const int MaxWorkerId = ~(-1 << WorkerIdBits);

        /// <summary>
        /// mask that help to extract timestamp and sequence from a long
        /// </summary>
        private const long TimestampAndSequenceMask = ~(-1L << (TimestampBits + SequenceBits));

        /// <summary>
        /// business meaning: machine ID (0 ~ 1023)
        /// actual layout in memory:
        /// highest 1 bit: 0
        /// middle 10 bit: workerId
        /// lowest 53 bit: all 0
        /// </summary>
        private long _workerId { get; set; } = 0L;

        /// <summary>
        /// timestamp and sequence mix in one Long
        /// highest 11 bit: not used
        /// middle  41 bit: timestamp
        /// lowest  12 bit: sequence
        /// </summary>
        private long _timestampAndSequence;

        private static SeataSnowflakeIdWorker? _snowflakeId;

        private static readonly object SLock = new object();

        private readonly object _lock = new object();

        public SeataSnowflakeIdWorker(long workerId)
        {
            InitTimestampAndSequence();
            // sanity check for workerId
            if (workerId > MaxWorkerId || workerId < 0)
                throw new ArgumentException($"worker Id can't be greater than {MaxWorkerId} or less than 0");

            _workerId = workerId << (TimestampBits + SequenceBits);
        }

        public static SeataSnowflakeIdWorker Default()
        {
            if (_snowflakeId != null)
            {
                return _snowflakeId;
            }

            lock (SLock)
            {
                if (_snowflakeId != null)
                {
                    return _snowflakeId;
                }

                var workerId = Util.GenerateWorkerId(MaxWorkerId);

                return _snowflakeId = new SeataSnowflakeIdWorker(workerId);
            }
        }

        public virtual long NextId()
        {
            lock (_lock)
            {
                WaitIfNecessary();
                long timestampWithSequence = _timestampAndSequence & TimestampAndSequenceMask;
                return _workerId | timestampWithSequence;
            }
        }

        /// <summary>
        /// init first timestamp and sequence immediately
        /// </summary>
        private void InitTimestampAndSequence()
        {
            long timestamp = GetNewestTimestamp();
            long timestampWithSequence = timestamp << SequenceBits;
            _timestampAndSequence = timestampWithSequence;
        }

        /// <summary>
        /// block current thread if the QPS of acquiring UUID is too high
        /// that current sequence space is exhausted
        /// </summary>
        private void WaitIfNecessary()
        {
            long currentWithSequence = ++_timestampAndSequence;
            long current = currentWithSequence >> SequenceBits;
            long newest = GetNewestTimestamp();

            if (current >= newest)
            {
                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// get newest timestamp relative to twepoch
        /// </summary>
        /// <returns></returns>
        private long GetNewestTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - Twepoch;
        }
    }

    internal static class Util
    {
        /// <summary>
        /// auto generate workerId, try using mac first, if failed, then randomly generate one
        /// </summary>
        /// <returns>workerId</returns>
        public static long GenerateWorkerId(int maxWorkerId)
        {
            try
            {
                return GenerateWorkerIdBaseOnMac();
            }
            catch
            {
                return GenerateRandomWorkerId(maxWorkerId);
            }
        }

        /// <summary>
        /// use lowest 10 bit of available MAC as workerId
        /// </summary>
        /// <returns>workerId</returns>
        private static long GenerateWorkerIdBaseOnMac()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            var firstUpInterface = nics.OrderByDescending(x=> x.Speed).FirstOrDefault(x =>!x.Description.Contains("Virtual") && x.NetworkInterfaceType != NetworkInterfaceType.Loopback && x.OperationalStatus == OperationalStatus.Up);
            if (firstUpInterface == null)
            {
                throw new Exception("GenerateWorkerIdBaseOnMac----no available firstUpInterface found");
            }
            var props = firstUpInterface.GetIPProperties();
            // get first IPV4 address assigned to this interface
            var firstIpV4Address = props.UnicastAddresses
                .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(c => c.Address)
                .FirstOrDefault();
            Console.WriteLine($"firstIpV4Address---{firstIpV4Address}  descript---{firstUpInterface.Description}   nics---{nics[0].Description}");
            PhysicalAddress address = firstUpInterface.GetPhysicalAddress();
            byte[] mac = address.GetAddressBytes();
            return ((mac[4] & 0B11) << 8) | (mac[5] & 0xFF);
        }

        /// <summary>
        /// randomly generate one as workerId
        /// </summary>
        /// <returns></returns>
        private static long GenerateRandomWorkerId(int maxWorkerId)
        {
            return new Random().Next(maxWorkerId + 1);
        }
    }
}
