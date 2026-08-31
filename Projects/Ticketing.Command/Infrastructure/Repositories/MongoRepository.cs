using DnsClient.Protocol;
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
            return _collection.AsQueryable();
        }

        public async Task<IClientSessionHandle> BeginSessionAsync(CancellationToken cancellationToken)
        {
            var option = new ClientSessionOptions();
            option.DefaultTransactionOptions = new TransactionOptions();
            return await _collection.Database.Client.StartSessionAsync(option, cancellationToken);
        }

        public void BeginTransaction(IClientSessionHandle clientSessionHandle)
        {
            clientSessionHandle.StartTransaction();
        }

        public Task CommitTransactionAsync(IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
        {
            return clientSessionHandle.CommitTransactionAsync(cancellationToken);
        }

        public void DisposeSession(IClientSessionHandle clientSessionHandle)
        {
            clientSessionHandle.Dispose();
        }

        public async Task InsertOneAsync(TDocument document, IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(
                clientSessionHandle, document, null, cancellationToken
            );
        }

        public Task RollbackTransactionAsync(IClientSessionHandle clientSessionHandle, CancellationToken cancellationToken) => clientSessionHandle.AbortTransactionAsync();
    }
}