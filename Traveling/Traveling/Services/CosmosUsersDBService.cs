using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveling.Helper;
using Traveling.Models;

namespace Traveling.Services
{
    class CosmosUsersDBService
    {
        static DocumentClient docClient = null;

        static readonly string databaseName = "Travelingdata";
        static readonly string collectionName = "Users";

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

        public async static Task<List<Users>> GetAllUsers()
        {
            var userList = new List<Users>();

            if (!await Initialize()) return userList;

            var itemQuery = docClient.CreateDocumentQuery<Users>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Users>();

                userList.AddRange(queryResult);
            }
            return userList;
        }

        public async static Task<List<Users>> GetSearchedUsers(string firstn, string lastn)
        {
            var userList = new List<Users>();

            if (!await Initialize()) return userList;

            var itemQuery = docClient.CreateDocumentQuery<Users>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    "SELECT * FROM Users u WHERE u.FirstName == '"+firstn+"' OR u.LastName == '"+ lastn + "'")
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Users>();

                userList.AddRange(queryResult);
            }
            return userList;
        }

        public async static Task<List<Users>> CheckLogin(string usern, string pwd) {
            var userList = new List<Users>();

            if (!await Initialize()) return userList;

            var usersQuery = docClient.CreateDocumentQuery<Users>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(user => user.username == usern)
                .Where(user => user.password == pwd)
                .AsDocumentQuery();

            while (usersQuery.HasMoreResults)
            {
                var queryResult = await usersQuery.ExecuteNextAsync<Users>();
                userList.AddRange(queryResult);
            }
            return userList;
        }

        public async static Task DeleteUser(Users user) {
            if (!await Initialize()) return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, user.Id);
            await docClient.DeleteDocumentAsync(docUri, new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });

        }

        public async static Task UpdateUser(Users user) {
            if (!await Initialize()) return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, user.Id);
            await docClient.ReplaceDocumentAsync(docUri, user);
        }
    }
}
