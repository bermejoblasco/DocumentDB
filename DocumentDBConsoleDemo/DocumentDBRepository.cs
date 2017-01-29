using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDBConsoleDemo
{
    public static class DocumentDBRepository<T> where T : class
    {
        private static string DatabaseId;
        private static Uri CollectionLink;
        private static string CollectionId;
        private static DocumentClient client;

        public static void Initialize(string databaseId, string collectionId)
        {
            CollectionId = collectionId;
            DatabaseId = databaseId;
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]),ConfigurationManager.AppSettings["authKey"]);
            CollectionLink = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public static async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(CollectionLink);
                return (T) (dynamic) document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }

        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var query = client.CreateDocumentQuery<T>(
                CollectionLink,
                new FeedOptions {MaxItemCount = -1})
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<IEnumerable<T>> GetAllItemsAsync()
        {
            var query = client.CreateDocumentQuery<T>(
                CollectionLink,
                new FeedOptions {MaxItemCount = -1})
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<Document> CreateItemAsync(T item)
        {
            return
                await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }


        public static async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync(CollectionLink, item);
        }

        public static async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(CollectionLink);
        }

        public static async Task<IEnumerable<T>> SearhcItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var query = client.CreateDocumentQuery<T>(CollectionLink)
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task CreateUserDefinedFunctionAsync(UserDefinedFunction function)
        {
            await client.CreateUserDefinedFunctionAsync(CollectionLink, function);
        }

        public static async Task CreateStoreProcedureAsync(StoredProcedure storeProcedure)
        {
            await client.CreateStoredProcedureAsync(CollectionLink, storeProcedure);
        }

        public static async Task<StoredProcedureResponse<T>> ExecuteSpByName<T>(string spName)
        {
            try
            {
                var storedProcedure = client.CreateStoredProcedureQuery(CollectionLink).Where(c => c.Id == spName).AsEnumerable().FirstOrDefault();
                return await client.ExecuteStoredProcedureAsync<T>(storedProcedure.SelfLink);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
           
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection {Id = CollectionId},
                        new RequestOptions {OfferThroughput = 1000});
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database {Id = DatabaseId});
                }
                else
                {
                    throw;
                }
            }
        }

        public static IQueryable<T> ExecutFunctionByName<T>(string sqrt, int parameter)
        {
            try
            {
                var queryString = $"SELECT {sqrt}({parameter})";
                return client.CreateDocumentQuery<T>(CollectionLink, queryString);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }

        public static async Task<ResourceResponse<Trigger>>  CreateDocumentTrigger(Trigger trigger)
        {
            try
            {
                return await client.CreateTriggerAsync(CollectionLink, trigger);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }
    }
}