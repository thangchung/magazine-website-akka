using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cik.Magazine.Shared.Queries
{
    [Serializable]
    public class CategoryViewRequest : Request
    {
        public CategoryViewRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class CategoryViewResponse
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid ParentId { get; set; }
    }
}