using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.Helper;
using Traveling.Models;

namespace Traveling.Services
{
    class CosmosScheduleDBService
    {
        static DocumentClient docClient = null;

        static readonly string databaseName = "Travelingdata";
        static readonly string collectionName = "schedule";

        static async Task<bool> Initialize() {
            if (docClient != null) return true;
            try
            {
                docClient = new DocumentClient(new Uri(APIKeys.CosmosEndpointUrl), APIKeys.CosmosAuthKey);

                await docClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
                await docClient.CreateDocumentCollectionIfNotExistsAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        new DocumentCollection { Id = collectionName },
                        new RequestOptions { OfferThroughput = 400 }
                    );
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                docClient = null;
                return false;
            }

            return true;
        }

        public async static Task<List<Schedule>> GetSchedule() {
            var scheduleList = new List<Schedule>();

            if (!await Initialize()) return scheduleList;

            var itemQuery = docClient.CreateDocumentQuery<Schedule>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(flight=>flight.isAvailable!=0)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults) {
                var queryResult = await itemQuery.ExecuteNextAsync<Schedule>();
                scheduleList.AddRange(queryResult);
            }
            return scheduleList;
        }

        public async static Task<List<Schedule>> SearchSchedule(string sourceplace, string desplace, string sdate) {
            var scheduleList = new List<Schedule>();

            if (!await Initialize()) return scheduleList;

            var itemQuery = docClient.CreateDocumentQuery<Schedule>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(flight => flight.date == sdate)
                .Where(flight => flight.source == sourceplace)
                .Where(flight => flight.destination == desplace)
                .Where(flight => flight.isAvailable!=0)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Schedule>();

                scheduleList.AddRange(queryResult);
            }
            return scheduleList;
        }

        public async static Task<List<Schedule>> GetAllSchedule()
        {
            var scheduleList = new List<Schedule>();

            if (!await Initialize()) return scheduleList;

            var itemQuery = docClient.CreateDocumentQuery<Schedule>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Schedule>();

                scheduleList.AddRange(queryResult);
            }
            return scheduleList;
        }

        public async static Task<List<Schedule>> SearchAllSchedule(string sourceplace, string desplace, string sdate)
        {
            var scheduleList = new List<Schedule>();

            if (!await Initialize()) return scheduleList;

            Console.WriteLine("LOOL: "+ sourceplace+" "+ desplace+" "+ sdate);

            var itemQuery = docClient.CreateDocumentQuery<Schedule>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(flight => flight.date == sdate)
                .Where(flight => flight.source == sourceplace)
                .Where(flight => flight.destination == desplace)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Schedule>();
                scheduleList.AddRange(queryResult);
            }
            return scheduleList;
        }

        public async static Task UpdateSchedule(Schedule schedule)
        {
            if (!await Initialize()) return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, schedule.Id);
            await docClient.ReplaceDocumentAsync(docUri, schedule);
        }

        public async static Task InsertSchedule(Schedule schedule)
        {
            if (!await Initialize()) return;

            await docClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), 
                schedule);
        }

        public async static Task DeleteSchdule(Schedule schedule)
        {
            if (!await Initialize()) return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, schedule.Id);
            await docClient.DeleteDocumentAsync(docUri, new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });
        }
    }
}
