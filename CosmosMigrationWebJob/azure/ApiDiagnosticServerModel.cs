/**
* Copyright 2018 Quest Software and/or its affiliates
* and other contributors as indicated by the @author tags.
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using Spotlight.Common.DatabaseManagement;

namespace CosmosMigrationWebJob
{
    public class ApiDSLicenseKeyDataModel
    {
        public long InstanceCount { get; set; }
        public string LicenseKey { get; set; }
        public string HostName { get; set; }
        public string SiteMessage { get; set; }
        public int ProductID { get; set; }
    }

    public class ApiDiagnosticServerModel : IIndexer
    {
        public const string DiagnosticServerNameColumn = "diagnostic_server_name";
        public const string DiagnosticServerIdColumn = "id";
        public const string IsCloudDsColumn = "is_cloud_ds";
        public const string CreateDateColumn = "create_date";
        public const string UpdateDateColumn = "update_date";
        public const string UserAgentColumn = "user_agent";
        public const string LicenseKeyDataColumn = "license_key_data";
        public const string OrganizationIdColumn = "organization_id";
        public const string VersionColumn = "version";
        public const string HostNameColumn = "host_name";

        public static readonly string[] Fields =
        {
            DiagnosticServerNameColumn, DiagnosticServerIdColumn, IsCloudDsColumn, CreateDateColumn, UpdateDateColumn, UserAgentColumn, LicenseKeyDataColumn,
            OrganizationIdColumn, VersionColumn, HostNameColumn
        };

        private readonly Dictionary<string, Action<dynamic>> _columnNamePropertyMapping = new Dictionary<string, Action<dynamic>>();

        public ApiDiagnosticServerModel()
        {
            _columnNamePropertyMapping.Add(DiagnosticServerNameColumn, value =>
            {
                if (value is string)
                {
                    DiagnosticServerName = value;
                }
            });
            _columnNamePropertyMapping.Add(DiagnosticServerIdColumn, value =>
            {
                if (value is Guid)
                {
                    DiagnosticServerID = value;
                }
            });
            _columnNamePropertyMapping.Add(OrganizationIdColumn, value =>
            {
                if (value is Guid)
                {
                    OrganizationId = value;
                }
            });
            _columnNamePropertyMapping.Add(IsCloudDsColumn, value =>
            {
                if (value is bool)
                {
                    IsCloudDS = value;
                }
            });
            _columnNamePropertyMapping.Add(CreateDateColumn, value =>
            {
                if (value is DateTime)
                {
                    CreateDateUtc = value;
                }
            });
            _columnNamePropertyMapping.Add(UpdateDateColumn, value =>
            {
                if (value is DateTime)
                {
                    UpdateDateUtc = value;
                }
            });
            _columnNamePropertyMapping.Add(UserAgentColumn, value =>
            {
                if (value is string)
                {
                    UserAgent = value;
                }
            });
            _columnNamePropertyMapping.Add(LicenseKeyDataColumn, value =>
            {
                if (value is string)
                {
                    LicenseKeyData = JsonConvert.DeserializeObject<List<ApiDSLicenseKeyDataModel>>(value);
                }
            });
            _columnNamePropertyMapping.Add(VersionColumn, value =>
            {
                if (value is string)
                {
                    Version = value; 
                }
            });
            _columnNamePropertyMapping.Add(HostNameColumn, value =>
            {
                if (value is string)
                {
                    HostName = value; 
                }
            });
        }
        
        public Guid OrganizationId { get; set; }
        public string DiagnosticServerName { get; set; }
        [JsonIgnore]
        public Guid DiagnosticServerID { get; set; }
        [JsonProperty("DiagnosticServerID")]
        Guid? NullableDiagnosticServerID { get { return DiagnosticServerID == Guid.Empty ? null : (Guid?)DiagnosticServerID; } set { DiagnosticServerID = value ?? Guid.Empty; } }
        public bool IsCloudDS { get; set; }
        public List<ApiDSLicenseKeyDataModel> LicenseKeyData { get; set; }
        public DateTime CreateDateUtc { get; set; }
        public DateTime UpdateDateUtc { get; set; }
        public string UserAgent { set; get; }
        public string Version { set; get; }
        public string HostName { set; get; }

        public object this[string t]
        {
            get => throw new NotImplementedException();
            set => _columnNamePropertyMapping[t](value);
        }
    }
}
