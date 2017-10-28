using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Carable.Lazy;
namespace Soonish.Forms.Data
{
    public class SoonishTableStorage:ISoonishStorage
    {
        private readonly AzureTableOptions _options;
        private readonly IConfiguration _config;
        private readonly LazyAsync<CloudTable> _cloudTable;

        public SoonishTableStorage(IOptions<AzureTableOptions> options, IConfiguration config)
        {
            _options = options.Value ?? throw new NullReferenceException(nameof(options.Value));
            _config = config;
            _cloudTable = new LazyAsync<CloudTable>(CreateTableAsync);
        }

        private async Task<CloudTable> CreateTableAsync()
        {
            // Retrieve storage account information from connection string.
            var storageAccount = CloudStorageAccount.Parse(_config.GetConnectionString("AzureTable"));

            // Create a table client for interacting with the table service
            var tableClient = storageAccount.CreateCloudTableClient();

            // Create a table client for interacting with the table service 
            var table = tableClient.GetTableReference(_options.TableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }
        class ResponseEntity : TableEntity
        {
            // Your entity type must expose a parameter-less constructor
            public ResponseEntity() { }

            // Define the PK and RK
            public ResponseEntity(DateTime time)
            {
                this.PartitionKey = time.ToString("yyyy-M");
                this.RowKey = time.ToString("O");
            }

            //For any property that should be stored in the table service, the property must be a public property of a supported type that exposes both get and set.        
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        public async Task Insert(Response entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            TableOperation insert = TableOperation.Insert(new ResponseEntity(DateTime.UtcNow)
            {
                Email = entity.Email,
                Name = entity.Name,
                Phone = entity.Phone
            });
            var table =await _cloudTable.GetValue();
            // Execute the operation.
            await table.ExecuteAsync(insert);
        }

        public async Task<ReadOnlyCollection<Response>> GetResponsesForMonth(int year, int month)
        {
            var partitionKey = new DateTime(year, month, 1).ToString("yyyy-M");
            var table = await _cloudTable.GetValue();

            var partitionScanQuery = new TableQuery<ResponseEntity>().Where
                (TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            return await GetResponses(table, partitionScanQuery);
        }

        public async Task<ReadOnlyCollection<Response>> GetResponsesForYear(int year)
        {
            var partitionStartKey = new DateTime(year, 1, 1).ToString("yyyy-M");
            var partitionEndKey = new DateTime(year, 12, 1).ToString("yyyy-M");
            var table =await _cloudTable.GetValue();
            var rangeQuery = new TableQuery<ResponseEntity>().Where(
                        TableQuery.CombineFilters(
                            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThanOrEqual, partitionStartKey),
                            TableOperators.And,
                            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.LessThanOrEqual, partitionEndKey)));

            return await GetResponses(table, rangeQuery);
        }

        private async Task<ReadOnlyCollection<Response>> GetResponses(CloudTable table, TableQuery<ResponseEntity> query)
        {
            TableContinuationToken token = null;
            // Page through the results
            var responses = new List<Response>(100);
            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, token);
                responses.AddRange(segment.Select(Map));
                token = segment.ContinuationToken;
            }
            while (token != null);
            return new ReadOnlyCollection<Response>(responses);
        }


        private Response Map(ResponseEntity arg)
        {
            return new Response
            {
                Email = arg.Email,
                Name = arg.Name,
                Phone = arg.Phone
            };
        }
    }
}
