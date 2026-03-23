using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TicketService.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string EventId { get; set; } = null!; // Hangi etkinlik?
        public string UserId { get; set; } = null!;  // Kim aldı?
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public decimal Price { get; set; }
    }
}
