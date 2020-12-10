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
    class CosmosTransService
    {
        static DocumentClient docClient = null;

        static readonly string databaseName = "Travelingdata";
        static readonly string collectionName = "transaction";

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

        public async static Task<List<Transaction>> GetTransaction()
        {
            var transactionList = new List<Transaction>();

            if (!await Initialize()) return transactionList;

            var itemQuery = docClient.CreateDocumentQuery<Transaction>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Transaction>();
                transactionList.AddRange(queryResult);
            }
            return transactionList;
        }

        public async static Task<List<Transaction>> GetUPTransaction()
        {
            var transactionList = new List<Transaction>();

            if (!await Initialize()) return transactionList;

            var itemQuery = docClient.CreateDocumentQuery<Transaction>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(trans => trans.isPaid == 0)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Transaction>();
                transactionList.AddRange(queryResult);
            }
            return transactionList;
        }

        public async static Task<List<Transaction>> GetPTransaction()
        {
            var transactionList = new List<Transaction>();

            if (!await Initialize()) return transactionList;

            var itemQuery = docClient.CreateDocumentQuery<Transaction>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(trans => trans.isPaid != 0)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Transaction>();
                transactionList.AddRange(queryResult);
            }
            return transactionList;
        }

        public async static Task<List<Transaction>> GetTransactionBySort(string sort)
        {
            var transactionList = new List<Transaction>();

            if (!await Initialize()) return transactionList;

            var itemQuery = docClient.CreateDocumentQuery<Transaction>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(trans => trans.vehicleSort == sort)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Transaction>();
                transactionList.AddRange(queryResult);
            }
            return transactionList;
        }

        public async static Task InsertTransaction(Transaction transaction)
        {
            if (!await Initialize()) return;

            await docClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                transaction);
        }
    }
}
