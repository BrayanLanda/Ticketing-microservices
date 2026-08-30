using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ticketing.Command.Application.Models;
using Ticketing.Command.Domain.Abstracts;
using Ticketing.Command.Domain.Common;

namespace Ticketing.Command.Infrastructure.Repositories
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(
            IMongoClient mongoClient,
            IOptions<MongoSettings> options
        )
        {
            _collection = mongoClient
                .GetDatabase(options.Value.Database)
                .GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            var name = documentType
                .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault();
            if(name is not null)
                return ((BsonCollectionAttribute)name).CollectionName;

            throw new ArgumentException("The collection is unknown");
        }

        public IQueryable<TDocument> AsQueryable()
        {
            throw new NotImplementedException();
        }

        public Task<IClientSessionHandle> BeginSessionAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction(IClientSessionHandle clientSessionHandle)
        {
            throw new NotImplementedException();
        }

        public Task CommitTransactionAsync(IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void DisposeSession(IClientSessionHandle clientSessionHandle)
        {
            throw new NotImplementedException();
        }

        public Task InsertOneAsync(TDocument document, IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RollbackTransactionAsync(IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}