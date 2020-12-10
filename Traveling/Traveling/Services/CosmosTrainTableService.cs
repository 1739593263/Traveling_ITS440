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
    class CosmosTrainTableService
    {
        static DocumentClient docClient = null;

        static readonly string databaseName = "Travelingdata";
        static readonly string collectionName = "trainTable";

        static async Task<bool> Initialize()
        {
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                docClient = null;
                return false;
            }

            return true;
        }

        public async static Task<List<Train>> GetTrain()
        {
            var scheduleList = new List<Train>();

            if (!await Initialize()) return scheduleList;

            var itemQuery = docClient.CreateDocumentQuery<Train>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(train => train.isAvailable != 0)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Train>();
                scheduleList.AddRange(queryResult);
            }
            return scheduleList;
        }

        public async static Task<List<Train>> SearchTrain(string sourceplace, string desplace, string sdate)
        {
            var trainList = new List<Train>();

            if (!await Initialize()) return trainList;

            var itemQuery = docClient.CreateDocumentQuery<Train>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(train => train.date == sdate)
                .Where(train => train.source == sourceplace)
                .Where(train => train.destination == desplace)
                .Where(train => train.isAvailable != 0)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Train>();

                trainList.AddRange(queryResult);
            }
            return trainList;
        }

        public async static Task<List<Train>> GetAllTrain()
        {
            var trainList = new List<Train>();

            if (!await Initialize()) return trainList;

            var itemQuery = docClient.CreateDocumentQuery<Train>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Train>();

                trainList.AddRange(queryResult);
            }
            return trainList;
        }

        public async static Task<List<Train>> SearchAllTrain(string sourceplace, string desplace, string sdate)
        {
            var trainList = new List<Train>();

            if (!await Initialize()) return trainList;

            var itemQuery = docClient.CreateDocumentQuery<Train>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(flight => flight.date == sdate)
                .Where(flight => flight.source == sourceplace)
                .Where(flight => flight.destination == desplace)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Train>();
                trainList.AddRange(queryResult);
            }
            return trainList;
        }

        public async static Task UpdateTrain(Train train)
        {
            if (!await Initialize()) return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, train.Id);
            await docClient.ReplaceDocumentAsync(docUri, train);
        }

        public async static Task InsertTrain(Train train)
        {
            if (!await Initialize()) return;

            await docClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                train);
        }

        public async static Task DeleteTrain(Train train)
        {
            if (!await Initialize()) return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, train.Id);
            await docClient.DeleteDocumentAsync(docUri, new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });
        }

        public async static Task<Train> GetTrainById(string id)
        {
            Train train = new Train();
            var trainList = new List<Train>();

            if (!await Initialize()) return train;

            var itemQuery = docClient.CreateDocumentQuery<Train>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(train => train.Id == id)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Train>();
                trainList.AddRange(queryResult);
            }
            train = trainList[0];

            return train;
        }
    }
}
