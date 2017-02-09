using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cik.Magazine.Core.Views
{
    public class CategoryView
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
    }

    public class CategoryDto
    {
        public string Name { get; set; }
    }
}