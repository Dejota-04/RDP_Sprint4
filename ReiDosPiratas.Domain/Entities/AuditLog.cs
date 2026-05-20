using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ReiDosPiratas.Domain.Entities
{
    public class AuditLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string Acao { get; set; } = string.Empty;

        public DateTime DataAcesso { get; set; }

        public string? IpAddress { get; set; }
    }
}