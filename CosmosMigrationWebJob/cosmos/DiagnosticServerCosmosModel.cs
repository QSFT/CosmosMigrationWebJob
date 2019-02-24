using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Spotlight.Common.NoSqlDatabaseManagement;

namespace CosmosMigrationWebJob
{
    public class DiagnosticServerCosmosModel:NoSqlObject
    {
        public string DiagnosticServerName { get; set; }
        public bool IsCloudDs { get; set; }
        public List<ApiDSLicenseKeyDataModel> LicenseKeyData { get; set; }
        public DateTime CreateDateUtc { get; set; }
        public DateTime UpdateDateUtc { get; set; }
        public string UserAgent { get; set; }
        public string Version { get; set; }
        public string HostName { get; set; }
        [JsonIgnore]
        public override string TableName { get; set; }
        [JsonProperty("OrganizationId")]
        public override string Partition { get; set; }
    }
}
