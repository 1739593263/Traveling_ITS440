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
    class CosmosHotelTransService
    {
        static DocumentClient docClient = null;

        static readonly string databaseName = "Travelingdata";
        static readonly string collectionName = "HotelTransaction";

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

        public async static Task<List<HotelTrans>> GetHotel()
        {
            var hotelList = new List<HotelTrans>();

            if (!await Initialize()) return hotelList;

            var itemQuery = docClient.CreateDocumentQuery<HotelTrans>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<HotelTrans>();
                hotelList.AddRange(queryResult);
            }
            return hotelList;
        }

        public async static Task<List<HotelTrans>> searchPHotel(string uid)
        {
            var hotelList = new List<HotelTrans>();

            if (!await Initialize()) return hotelList;

            var itemQuery = docClient.CreateDocumentQuery<HotelTrans>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(hotel => hotel.isPaid != 0)
                .Where(hotel => hotel.userId == uid)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<HotelTrans>();
                hotelList.AddRange(queryResult);
            }
            return hotelList;
        }

        public async static Task<List<HotelTrans>> searchUnpHotel(string uid)
        {
            var hotelList = new List<HotelTrans>();

            if (!await Initialize()) return hotelList;

            var itemQuery = docClient.CreateDocumentQuery<HotelTrans>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(hotel => hotel.isPaid == 0)
                .Where(hotel => hotel.userId == uid)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<HotelTrans>();
                hotelList.AddRange(queryResult);
            }
            return hotelList;
        }

        public async static Task<HotelTrans> SearchHotelByLastUid(string uid)
        {
            var hotelList = new List<HotelTrans>();
            HotelTrans hotelTrans = new HotelTrans();

            if (!await Initialize()) return hotelTrans;

            var itemQuery = docClient.CreateDocumentQuery<HotelTrans>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(hotelt => hotelt.userId == uid)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<HotelTrans>();
                hotelList.AddRange(queryResult);


                hotelTrans = hotelList[hotelList.Count - 1];
            }
            return hotelTrans;
        }

        public async static Task<HotelTrans> searchHotelById(string id)
        {
            var hotelList = new List<HotelTrans>();
            HotelTrans hotelTrans = new HotelTrans();

            if (!await Initialize()) return hotelTrans;

            var itemQuery = docClient.CreateDocumentQuery<HotelTrans>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(hotel => hotel.Id == id)
                .AsDocumentQuery();

            while (itemQuery.HasMoreResults)
            {
                var queryResult = await itemQuery.ExecuteNextAsync<HotelTrans>();
                hotelList.AddRange(queryResult);
                hotelTrans = hotelList[0];
            }
            return hotelTrans;
        }

        public async static Task InsertHotelTrans(HotelTrans hotel)
        {
            if (!await Initialize()) return;

            await docClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                hotel);
        }

        public async static Task UpdateHotelTrans(HotelTrans hotel)
        {
            if (!await Initialize()) return;

            var docUri = UriFactory.CreateDocumentUri(databaseName, collectionName, hotel.Id);
            await docClient.ReplaceDocumentAsync(docUri, hotel);
        }
    }
}
