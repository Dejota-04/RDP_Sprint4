using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ReiDosPiratas.Domain.Entities;
using ReiDosPiratas.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReiDosPiratas.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly IMongoCollection<AuditLog> _logsCollection;

        public AuditLogRepository(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration["MongoDb:ConnectionString"]);
            var mongoDatabase = mongoClient.GetDatabase(configuration["MongoDb:DatabaseName"]);
            _logsCollection = mongoDatabase.GetCollection<AuditLog>("AuditLogs");
        }

        public async Task RegistrarAcessoAsync(AuditLog log)
        {
            await _logsCollection.InsertOneAsync(log);
        }

        public async Task<IEnumerable<AuditLog>> ObterUltimosAcessosAsync(int quantidade = 50)
        {
            return await _logsCollection.Find(_ => true)
                                        .SortByDescending(l => l.DataAcesso)
                                        .Limit(quantidade)
                                        .ToListAsync();
        }
    }
}