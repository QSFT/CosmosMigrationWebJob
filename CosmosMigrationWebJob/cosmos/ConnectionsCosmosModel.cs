using Spotlight.Common.NoSqlDatabaseManagement;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace CosmosMigrationWebJob
{
    public class ConnectionsCosmosModel: NoSqlObject
    {
        [JsonIgnore]
        public override string TableName { get; set; }
        [JsonProperty("OrganizationId")]
        public override string Partition { get; set; }
        public string DisplayName { get; set; }
        public string TechnologyName { get; set; }
        public bool IsOnPrem { get; set; }
        public bool IsEnabled { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public List<string> Tags { get; set; }
        public List<ConnectionRelationshipModel> Relationships { get; set; }
        public string MonitoringConfig { get; set; }
        public DateTime CreateDateUtc { get; set; }
        public DateTime UpdateDateUtc { get; set; }
        public string ConnectionName { get; set; }
        public string DiagnosticServerName { get; set; }
        public Guid DiagnosticServerId { get; set; }
    }
}
