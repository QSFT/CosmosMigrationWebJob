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
using Spotlight.Common.DiagnosticServerBehavior;

namespace CosmosMigrationWebJob
{
    public enum ConnectionAvailabilityStatus    
    {
        Available,
        Unavailable,
        Unknown,
        NoStatusCalculated
    
    }
    public class ConnectionRelationshipModel
    {
        public string RelationshipType { get; set; }
        public string ConnectionName { get; set; }
        public string TechnologyName { get; set; }
        public Guid? ConnectionId { get; set; }
    }

    public class ApiConnectionsModel : IIndexer
    {
        public const string DiagnosticServerNameColumn = "diagnostic_server_name";
        public const string DiagnosticServerIdColumn = "diagnostic_server_id";
        public const string ConnectioNameColumn = "connection_name";
        public const string DisplayNameColumn = "display_name";
        public const string TechnologyNameColumn = "technology_name";
        public const string IsEnabledColumn = "is_enabled";
        public const string IsOnPremColumn = "is_on_prem";
        public const string PropertiesColumn = "properties";
        public const string TagsColumn = "tags";
        public const string RelationShipsColumn = "relationships";
        public const string MonitoringConfigColumn = "monitoring_config";
        public const string CreateTimeUtcColumn = "create_time_utc";
        public const string UpdateTimeUtcColumn = "update_time_utc";
        public const string OrganizationIdColumn = "organization_id";
        public const string ConnectionIdColumn = "id";

        public static readonly string[] Fields =
        {
            DiagnosticServerNameColumn, DiagnosticServerIdColumn, ConnectioNameColumn, TechnologyNameColumn, IsEnabledColumn, IsOnPremColumn,
            PropertiesColumn, TagsColumn, RelationShipsColumn, MonitoringConfigColumn, CreateTimeUtcColumn, UpdateTimeUtcColumn, OrganizationIdColumn, ConnectionIdColumn, DisplayNameColumn

        };

        private readonly Dictionary<string, Action<dynamic>> _columnNamePropertyMapping = new Dictionary<string, Action<dynamic>>();

        public ApiConnectionsModel()
        {
            _columnNamePropertyMapping.Add(DiagnosticServerNameColumn, value =>
            {
                if (!(value is string)) return;
                DiagnosticServerName = value;
            });
            _columnNamePropertyMapping.Add(DiagnosticServerIdColumn, value =>
            {
                if (!(value is Guid)) return;
                DiagnosticServerID = value;
            });
            _columnNamePropertyMapping.Add(OrganizationIdColumn, value =>
            {
                if (!(value is Guid)) return;
                OrganizationId = value;
            });
            _columnNamePropertyMapping.Add(ConnectionIdColumn, value =>
            {
                if (!(value is Guid)) return;
                Id = value;
            });
            _columnNamePropertyMapping.Add(ConnectioNameColumn, value =>
            {
                if (!(value is string)) return;
                ConnectionName = value;
            });
            _columnNamePropertyMapping.Add(DisplayNameColumn, value =>
            {
                if (!(value is string)) return;
                DisplayName = TechnologyName == TechnologyBehavior.APPLICATION_DS || TechnologyName == TechnologyBehavior.APPLICATION_DS_CLOUD ? DiagnosticServerName : value;
            });
            _columnNamePropertyMapping.Add(TechnologyNameColumn, value =>
            {
                if (!(value is string)) return;
                TechnologyName = value;
            });
            _columnNamePropertyMapping.Add(IsEnabledColumn, value =>
            {
                if (!(value is bool)) return;
                IsEnabled = value;
            });
            _columnNamePropertyMapping.Add(IsOnPremColumn, value =>
            {
                if (value is bool)
                    IsOnPrem = value;
            });
            _columnNamePropertyMapping.Add(PropertiesColumn, value =>
            {
                if (!(value is string)) return;
                Properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            });
            _columnNamePropertyMapping.Add(TagsColumn, value =>
            {
                if (!(value is string)) return;
                Tags = JsonConvert.DeserializeObject<List<string>>(value);
            });
            _columnNamePropertyMapping.Add(RelationShipsColumn, value =>
            {
                if (!(value is string)) return;
                Relationships = JsonConvert.DeserializeObject<List<ConnectionRelationshipModel>>(value);
            });
            _columnNamePropertyMapping.Add(MonitoringConfigColumn, value =>
            {
                if (!(value is string)) return;
                MonitoringConfig = value;
            });
            _columnNamePropertyMapping.Add(CreateTimeUtcColumn, value =>
            {
                if (!(value is DateTime)) return;
                CreateDateUtc = value;
            });
            _columnNamePropertyMapping.Add(UpdateTimeUtcColumn, value =>
            {
                if (!(value is DateTime)) return;
                UpdateDateUtc = value;
            });
        }

        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string DiagnosticServerName { get; set; }
        public Guid DiagnosticServerID { get; set; }
        public string ConnectionName { get; set; }
        public string DisplayName { get; set; }
        public string TechnologyName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsOnPrem { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public List<string> Tags { get; set; }
        public List<ConnectionRelationshipModel> Relationships { get; set; }
        public string MonitoringConfig { get; set; }
        public DateTime CreateDateUtc { get; set; }
        public DateTime UpdateDateUtc { get; set; }
        public long UpdateUser { get; set; }
        public ConnectionAvailabilityStatus AvailabilityStatus { get; set; } = ConnectionAvailabilityStatus.NoStatusCalculated;

        public object this[string t]
        {
            get => throw new NotImplementedException();
            set => _columnNamePropertyMapping[t](value);
        }
    }
}
