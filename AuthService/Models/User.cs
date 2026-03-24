using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthService.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string StudentId { get; set; } // Öðrenci Numarasý

        public string Email { get; set; }

        public string PasswordHash { get; set; } // þifrelenmiþ (hash) tutacaðýz

        public string Role { get; set; } // Örn: "Student", "Admin"
    }
}