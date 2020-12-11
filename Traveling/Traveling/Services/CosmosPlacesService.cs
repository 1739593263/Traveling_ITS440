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
    class CosmosPlacesService
    {
        static DocumentClient docClient = null;

        static readonly string databaseName = "Travelingdata";
        static readonly string collectionName = "Places";

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

        public async static Task<String[]> GetPlaces()
        {
            List<Places> PlacesList = new List<Places>();

            if (!await Initialize()) return null;

            var itemQuery = docClient.CreateDocumentQuery<Places>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(place => place.isflight != 0)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Places>();
                PlacesList.AddRange(queryResult);
            }

            var places = new String[PlacesList.Count];
            for (int a = 0; a < PlacesList.Count; a++) {
                places[a] = PlacesList[a].place;
            }
            return places;
        }

        public async static Task<List<Places>> GetPlacesList()
        {
            List<Places> PlacesList = new List<Places>();

            if (!await Initialize()) return null;

            var itemQuery = docClient.CreateDocumentQuery<Places>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Places>();
                PlacesList.AddRange(queryResult);
            }

            return PlacesList;
        }

        public async static Task<List<Places>> SearchPlaces(string p)
        {
            List<Places> PlacesList = new List<Places>();

            if (!await Initialize()) return null;

            var itemQuery = docClient.CreateDocumentQuery<Places>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(place => place.place.Contains(p))
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Places>();
                PlacesList.AddRange(queryResult);
            }

            return PlacesList;
        }

        public async static Task<Places> GetPlaceById(string id)
        {
            var placesList = new List<Places>();
            Places place = new Places();

            if (!await Initialize()) return place;

            var itemQuery = docClient.CreateDocumentQuery<Places>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(places => places.Id == id)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Places>();
                placesList.AddRange(queryResult);

                place = placesList[0];
            }
            return place;
        }

        public async static Task InsertPlace(Places place)
        {
            if (!await Initialize()) return;

            await docClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                place);
        }

        public async static Task DeletePlace(Places place)
        {
            if (!await Initialize()) return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, place.Id);
            await docClient.DeleteDocumentAsync(docUri, new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });

        }
    }
}
