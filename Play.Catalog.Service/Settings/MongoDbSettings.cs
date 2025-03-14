using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; } = null!;
        public string Port { get; set; }
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}