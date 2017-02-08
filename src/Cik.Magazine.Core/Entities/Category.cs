using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cik.Magazine.Core.Entities
{
    public class Category
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
    }
}