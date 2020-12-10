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
    class CosmosHotelService
    {
        static DocumentClient docClient = null;

        static readonly string databaseName = "Travelingdata";
        static readonly string collectionName = "Hotel";

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

        public async static Task<List<Hotel>> GetHotel()
        {
            var hotelList = new List<Hotel>();

            if (!await Initialize()) return hotelList;

            var itemQuery = docClient.CreateDocumentQuery<Hotel>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Hotel>();
                hotelList.AddRange(queryResult);
            }
            return hotelList;
        }

        public async static Task<List<Hotel>> SearchHotel(string c)
        {
            var hotelList = new List<Hotel>();

            if (!await Initialize()) return hotelList;

            var itemQuery = docClient.CreateDocumentQuery<Hotel>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(hotel => hotel.city == c)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Hotel>();
                hotelList.AddRange(queryResult);
            }
            return hotelList;
        }

        public async static Task<Hotel> GetHotelById(string id)
        {
            Hotel hotel = new Hotel();
            var hotelList = new List<Hotel>();

            if (!await Initialize()) return hotel;

            var itemQuery = docClient.CreateDocumentQuery<Hotel>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(hotel => hotel.Id == id)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Hotel>();
                hotelList.AddRange(queryResult);
            }
            hotel = hotelList[0];

            return hotel;
        }

        public async static Task UpdateHotelInQuantity(string id, string type, string ope)
        {

            if (!await Initialize()) return;

            Hotel hotel = new Hotel();
            var hotelList = new List<Hotel>();

            var itemQuery = docClient.CreateDocumentQuery<Hotel>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(hotel => hotel.Id == id)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<Hotel>();
                hotelList.AddRange(queryResult);
            }
            hotel = hotelList[0];
            if (type == "Double")
            {
                if (ope == "+")
                {
                    hotel.doubleroom++;
                }
                else
                {
                    hotel.doubleroom--;
                }
            }
            else if (type == "Quadruple")
            {
                if (ope == "+")
                {
                    hotel.Quaroom++;
                }
                else
                {
                    hotel.Quaroom--;
                }
            }
            else if (type == "Suite")
            {
                if (ope == "+")
                {
                    hotel.suit++;
                }
                else
                {
                    hotel.suit--;
                }
            }
            else return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, hotel.Id);
            await docClient.ReplaceDocumentAsync(docUri, hotel);
        }
    }
}
