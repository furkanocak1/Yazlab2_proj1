using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EventService.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Capacity { get; set; }
    }
}
