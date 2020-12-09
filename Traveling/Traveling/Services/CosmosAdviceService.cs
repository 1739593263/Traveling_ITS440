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
    class CosmosAdviceService
    {
        static DocumentClient docClient = null;

        static readonly string databaseName = "Travelingdata";
        static readonly string collectionName = "advise";

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

        public async static Task<List<Advise>> GetAllAdvises()
        {
            var adviseList = new List<Advise>();

            if (!await Initialize()) return adviseList;

            var itemQuery = docClient.CreateDocumentQuery<Advise>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Advise>();

                adviseList.AddRange(queryResult);
            }
            return adviseList;
        }

        public async static Task<List<Advise>> GetSolvedAdvises()
        {
            var adviseList = new List<Advise>();

            if (!await Initialize()) return adviseList;

            var itemQuery = docClient.CreateDocumentQuery<Advise>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(advise => advise.isSolved > 0 )
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Advise>();

                adviseList.AddRange(queryResult);
            }
            return adviseList;
        }

        public async static Task<List<Advise>> SearchSolvedAdvises(string name)
        {
            var adviseList = new List<Advise>();

            if (!await Initialize()) return adviseList;

            var itemQuery = docClient.CreateDocumentQuery<Advise>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(advise => advise.isSolved > 0)
                .Where(advise => advise.caseName.Contains(name))
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Advise>();

                adviseList.AddRange(queryResult);
            }
            return adviseList;
        }

        public async static Task<List<Advise>> GetunSolvedAdvises()
        {
            var adviseList = new List<Advise>();

            if (!await Initialize()) return adviseList;

            var itemQuery = docClient.CreateDocumentQuery<Advise>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(advise => advise.isSolved == 0)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Advise>();

                adviseList.AddRange(queryResult);
            }
            return adviseList;
        }

        public async static Task<List<Advise>> SearchunSolvedAdvises(string name)
        {
            var adviseList = new List<Advise>();

            if (!await Initialize()) return adviseList;

            var itemQuery = docClient.CreateDocumentQuery<Advise>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(advise => advise.isSolved == 0)
                .Where(advise => advise.caseName.Contains(name))
                .AsDocumentQuery();

            /*var itemQuery = docClient.CreateDocumentQuery<Users>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    "SELECT * FROM advise a WHERE a.case_name == '%" + name + "%'")
                .AsDocumentQuery();*/

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Advise>();

                adviseList.AddRange(queryResult);
            }
            return adviseList;
        }

        public async static Task InsertAdvise(Advise advise)
        {
            if (!await Initialize()) return;

            await docClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                advise);
        }

        public async static Task UpdateAdvise(Advise advise)
        {
            if (!await Initialize()) return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, advise.Id);
            await docClient.ReplaceDocumentAsync(docUri, advise);
        }
    }
}
