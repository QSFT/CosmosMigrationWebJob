using Newtonsoft.Json;
using Spotlight.Common.NoSqlDatabaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosMigrationWebJob
{
    public class AuthTokenCosmosModel : NoSqlObject
    {
        public DateTime CreationTime { get; set; }
        [JsonIgnore]
        public override string TableName { get; set; }
        [JsonProperty("OrganizationId")]
        public override string Partition { get; set; }
    }
}
