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
using System;
using System.Collections.Generic;
using Spotlight.Common.DatabaseManagement;

namespace CosmosMigrationWebJob
{
    public class AuthenticationTokenModel : IIndexer
    {
        public const string DsTokenColumn = "ds_token";
        public const string DiagnosticServerIdColumn = "diagnostic_server_id";
        public const string OrganizationIdColumn = "organization_id";
        public const string CreationTimeColumn = "creation_time";

        public static readonly string[] Fields = {DsTokenColumn, DiagnosticServerIdColumn, OrganizationIdColumn, CreationTimeColumn};

        private readonly Dictionary<string, Action<dynamic>> _columnNamePropertyMapping = new Dictionary<string, Action<dynamic>>();

        public AuthenticationTokenModel()
        {
            _columnNamePropertyMapping.Add(DsTokenColumn, value =>
            {
                if (value is Guid)
                {
                    DiagnosticServerToken = value;
                }
            });
            _columnNamePropertyMapping.Add(DiagnosticServerIdColumn, value =>
            {
                if (value is Guid)
                {
                    DiagnosticServerId = value;
                }
            });
            _columnNamePropertyMapping.Add(OrganizationIdColumn, value =>
            {
                if (value is Guid)
                {
                    OrganizationId = value;
                }
            });
            _columnNamePropertyMapping.Add(CreationTimeColumn, value =>
            {
                if (value is DateTime)
                {
                    CreationTime = value;
                }
            });
        }
        public Guid DiagnosticServerToken { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid DiagnosticServerId { get; set; }
        public DateTime CreationTime { get; set; }

        public object this[string t]
        {
            get => throw new NotImplementedException();
            set => _columnNamePropertyMapping[t](value);
        }
    }
}
