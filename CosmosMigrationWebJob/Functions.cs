using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Spotlight.Common.DatabaseManagement;
using Spotlight.Common.NoSqlDatabaseManagement;

namespace CosmosMigrationWebJob
{
    public class info
    {
        public string endpoint { get; set; }
        public string authKey { get; set; }
        public string sqlConnectionString { get; set; }
        public string tableName { get; set; }
    }
    public class Functions
    {
        private static CosmosDatabase cosmosDatabaseManagement;
        private static SqlAzureManagement sqlAzureManagement;
        private const string CONNECTION_TABLE_NAME = "connections";
        private const string AUTH_TABLE_NAME = "auth_tokens";
        private const string DS_TABLE_NAME = "diagnostic_servers";
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("noy")] string message, TextWriter log)
        {
            info config = JsonConvert.DeserializeObject<info>(message);

            cosmosDatabaseManagement = new CosmosDatabase(new System.Uri(config.endpoint), config.authKey, "dsmetadata", 250);
            sqlAzureManagement = new SqlAzureManagement(config.sqlConnectionString);
            int itemsMigrated = 0;
            log.WriteLine("Migration Started");
            switch (config.tableName)
            {
                case CONNECTION_TABLE_NAME:
                    itemsMigrated = MigrateConnectionsTable();
                    break;
                case AUTH_TABLE_NAME:
                    itemsMigrated = MigrateAuthTokenTable();
                    break;
                case DS_TABLE_NAME:
                    itemsMigrated = MigrateDsTable();
                    break;
            }
                    log.WriteLine($"Migration finished successfully with {itemsMigrated} item migrated");
        }

        private static  void InsertDBContent<T>(List<T> cosmosContent) where T : NoSqlObject
        {
            List<Task<HttpStatusCode>> cosmosTasks = new List<Task<HttpStatusCode>>();

            foreach (var cosmosItem in cosmosContent)
            {
                cosmosTasks.Add(cosmosDatabaseManagement.InsertAsync(cosmosItem));
                if (cosmosTasks.Count >= 250)
                {
                    Task.WaitAll(cosmosTasks.ToArray());
                    cosmosTasks.Clear();
                }
            }

            Task.WaitAll(cosmosTasks.ToArray());
        }

        private static  int MigrateConnectionsTable()
        {

            List<ApiConnectionsModel> response = sqlAzureManagement.ExecuteReader("SELECT * FROM dsmetadata_connections", ApiConnectionsModel.Fields, () => new ApiConnectionsModel());

            var cosmosResponse = ConnectionsConvertToCosmosObjectList(response);

            InsertDBContent(cosmosResponse);

            return cosmosResponse.Count();
        }

        private static  int MigrateDsTable()
        {

            List<ApiDiagnosticServerModel> response = sqlAzureManagement.ExecuteReader("SELECT * FROM dsmetadata_diagnostic_servers", ApiDiagnosticServerModel.Fields, () => new ApiDiagnosticServerModel());

            var cosmosResponse = DsConvertToCosmosObjectList(response);

            InsertDBContent(cosmosResponse);

            return cosmosResponse.Count();
        }
        private static int MigrateAuthTokenTable()
        {

            List<AuthenticationTokenModel> response = sqlAzureManagement.ExecuteReader("SELECT * FROM dsmetadata_auth_tokens", AuthenticationTokenModel.Fields, () => new AuthenticationTokenModel());

            var cosmosResponse = AuthConvertToCosmosObjectList(response);

            InsertDBContent(cosmosResponse);

            return cosmosResponse.Count();
        }

        private static List<ConnectionsCosmosModel> ConnectionsConvertToCosmosObjectList(List<ApiConnectionsModel> cosmosObjectList)
        {
            List<ConnectionsCosmosModel> convertedList = cosmosObjectList.Select(connObject => new ConnectionsCosmosModel()
            {
                CreateDateUtc = connObject.CreateDateUtc,
                DiagnosticServerId = connObject.DiagnosticServerID,
                DiagnosticServerName = connObject.DiagnosticServerName,
                Partition = connObject.OrganizationId.ToString(),
                UpdateDateUtc = connObject.UpdateDateUtc,
                ConnectionName = connObject.ConnectionName,
                DisplayName = connObject.DisplayName,
                Id = connObject.Id.ToString(),
                IsEnabled = connObject.IsEnabled,
                IsOnPrem = connObject.IsOnPrem,
                MonitoringConfig = connObject.MonitoringConfig,
                Properties = connObject.Properties,
                Relationships = connObject.Relationships,
                Tags = connObject.Tags,
                TechnologyName = connObject.TechnologyName,
                TableName = CONNECTION_TABLE_NAME
            }).ToList();
            return convertedList;
        }
        private static List<AuthTokenCosmosModel> AuthConvertToCosmosObjectList(List<AuthenticationTokenModel> cosmosObjectList)
        {
            List<AuthTokenCosmosModel> convertedList = cosmosObjectList.Select(sqlObject => new AuthTokenCosmosModel()
            {
                Id = ($"{sqlObject.DiagnosticServerToken.ToString()}.{sqlObject.DiagnosticServerId}") ,
                Partition = sqlObject.OrganizationId.ToString(),
                CreationTime = sqlObject.CreationTime,
                TableName = AUTH_TABLE_NAME
            }).ToList();

            return convertedList;
        }
        private static List<DiagnosticServerCosmosModel> DsConvertToCosmosObjectList(List<ApiDiagnosticServerModel> dsObjectList)
        {
            List<DiagnosticServerCosmosModel> convertedList = dsObjectList.Select(dsObject => new DiagnosticServerCosmosModel()
            {
                CreateDateUtc = dsObject.CreateDateUtc,
                Id = dsObject.DiagnosticServerID.ToString(),
                DiagnosticServerName = dsObject.DiagnosticServerName,
                HostName = dsObject.HostName,
                IsCloudDs = dsObject.IsCloudDS,
                LicenseKeyData = dsObject.LicenseKeyData,
                Partition = dsObject.OrganizationId.ToString(),
                UpdateDateUtc = dsObject.UpdateDateUtc,
                UserAgent = dsObject.UserAgent,
                Version = dsObject.Version,
                TableName = DS_TABLE_NAME
            }).ToList();

            return convertedList;
        }

    }
}
