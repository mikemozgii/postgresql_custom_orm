
using UnivIntel.PostgreSQL.ORM.Core.Enums;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;

namespace UnivIntel.PostgreSQL.ORM.Core.Uuid
{
    public class PKuuidSvc
    {
        private readonly int UuidGenerationLimit = 100;

        //todo thread-safe
        public ConcurrentQueue<Guid> TradingGuidGuidsCollection { get; set; } = new ConcurrentQueue<Guid>();

        public PKuuidSvc()
        {
            var ids = GenereateTradingGuid(1000);
            foreach (var id in ids)
            {
                TradingGuidGuidsCollection.Enqueue(id);
            }
        }

        public  PKuuidSvc CreateInstance() => new PKuuidSvc();

        public  Guid GenerateGuid() => new PKuuidSvc().GenereateTradingGuid();

        public void Generate_TradingGuidGuidsCollection()
        {
            var ids = GenereateTradingGuid(1000);
            foreach (var id in ids)
            {
                TradingGuidGuidsCollection.Enqueue(id);
            }
        }

        public string[] Genereate(PKuuidGenerationMdl parameters = null)
        {
            if (parameters != null)
            {
                var qnt = parameters.Quantity ?? 1;
                if (qnt > UuidGenerationLimit)
                {
                    qnt = UuidGenerationLimit;
                }

                switch (parameters.UuidType)
                {
                    case UuidType.Random:
                        return FormatUuid(parameters, RadomUuid(qnt));

                    case UuidType.Sequential:
                        return FormatUuid(parameters, SequentialUuid(parameters.SequentialUuidType, qnt));

                    case UuidType.Trading:
                        return GenereateTradingUuid(qnt);

                    default:
                        return FormatUuid(parameters, RadomUuid(qnt)); ;
                }
            }
            return RadomUuid(1);
        }

        private string[] FormatUuid(PKuuidGenerationMdl parameters, string[] uuids)
        {
            for (var i = 0; i < uuids.Count(); i++)
            {
                if (parameters.RemoveHyphens == true)
                {
                    uuids[i] = uuids[i].Replace("-", "");
                }

                if (parameters.Uppercase == true)
                {
                    uuids[i] = uuids[i].ToUpper();
                }

                if (parameters.IncludeBraces == true)
                {
                    uuids[i] = "{" + uuids[i] + "}";
                }
            }

            return uuids;
        }

        private readonly RNGCryptoServiceProvider GlobalRng = new RNGCryptoServiceProvider();

        private string[] RadomUuid(int qnt)
        {
            var uuids = new string[qnt];
            for (var i = 0; i < qnt; i++)
            {
                uuids[i] = Guid.NewGuid().ToString();
            }
            return uuids;
        }

        private string[] SequentialUuid(SequentialUuidType? uuidType, int qnt)
        {
            if (uuidType == null)
            {
                uuidType = SequentialUuidType.String;
            }

            var uuids = new string[qnt];
            for (var i = 0; i < qnt; i++)
            {
                var randomBytes = new byte[10];
                GlobalRng.GetBytes(randomBytes);
                var timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.AddYears(-1).AddDays(-i).Ticks / 10000L);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(timestampBytes);
                }

                var guidBytes = new byte[16];
                switch (uuidType)
                {
                    case SequentialUuidType.String:
                    case SequentialUuidType.Binary:
                        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                        Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                        if (uuidType == SequentialUuidType.String && BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(guidBytes, 0, 4);
                            Array.Reverse(guidBytes, 4, 2);
                        }
                        break;

                    case SequentialUuidType.AtEnd:
                        Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                        break;
                }

                uuids[i] = new Guid(guidBytes).ToString();
            }

            return uuids;
        }

        public string[] GenereateTradingUuid(int qnt)
        {
            var uuids = new string[qnt];
            var results = GenereateTradingGuid(qnt);
            for (var i = 0; i < qnt; i++)
            {
                uuids[i] = results[i].ToString();
            }
            return uuids;
        }

        public string GenereateTradingUuid()
        {
            return GenereateTradingUuid(1)[0];
        }

        public Guid GenereateTradingGuid()
        {
            if (TradingGuidGuidsCollection.TryDequeue(out var r))
            {
                return r;
            }

            Generate_TradingGuidGuidsCollection();
            return GenereateTradingGuid();
        }

        public Guid[] GenereateTradingGuid(int qnt)
        {
            var uuids = new Guid[qnt];
            for (var i = 0; i < qnt; i++)
            {
                var randomBytes = new byte[10];
                GlobalRng.GetBytes(randomBytes);
                var timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks / 10000L);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(timestampBytes);
                }

                var guidBytes = new byte[16];
                Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
                uuids[i] = new Guid(guidBytes);
            }

            return uuids.OrderBy(q => q).ToArray();
        }
    }
}
