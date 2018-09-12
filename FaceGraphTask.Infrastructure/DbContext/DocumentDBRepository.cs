using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Azure.Documents;
using System.Linq.Expressions;
using Microsoft.Azure.Documents.Linq;

namespace FaceGraphTask.Infrastructure.DbContext
{
    public class DocumentDbRepository<T> where T : class
    {
        private readonly string _databaseId = ConfigurationManager.AppSettings["database"];
        private DocumentClient _client;
        private readonly string _collectionId;
        public DocumentDbRepository(string collectionId)
        {
            _collectionId = collectionId;
            Initialize();
        }


        public T GetItemAsync(Guid id, string category)
        {
            try
            {
                Document document =
                    _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id.ToString())).Result;
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public IEnumerable<T> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query;
            if (predicate != null)
                query = _client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                    .Where(predicate)
                    .AsDocumentQuery();

            else
                query = _client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                    //.Where(predicate)
                    .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(query.ExecuteNextAsync<T>().Result);
            }

            return results;
        }

        public Document CreateItemAsync(T item)
        {
            return _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item).Result;
        }

        public async Task<Document> UpdateItemAsync(Guid id, T item)
        {
            return await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id.ToString()), item);
        }

        public void DeleteItemAsync(Guid id, string category)
        {
            _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id.ToString())).GetAwaiter().GetResult();
        }

        public void Initialize()
        {
            _client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
            CreateDatabaseIfNotExistsAsync();
            CreateCollectionIfNotExistsAsync();
        }

        private void CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseId)).GetAwaiter().GetResult();
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _client.CreateDatabaseAsync(new Database { Id = _databaseId }).GetAwaiter().GetResult();
                }
                else
                {
                    throw;
                }
            }
        }

        private void CreateCollectionIfNotExistsAsync()
        {
            try
            {
                _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId)).GetAwaiter().GetResult();
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_databaseId),
                        new DocumentCollection
                        {
                            Id = _collectionId
                        },
                        new RequestOptions { OfferThroughput = 400 }).GetAwaiter().GetResult();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
