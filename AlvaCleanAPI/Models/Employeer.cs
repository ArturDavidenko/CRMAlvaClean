using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;

namespace AlvaCleanAPI.Models
{
    public class Employeer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("Role")]
        public string Role { get; set; }

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("ImageId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ImageId { get; set; }

        [BsonElement("Orders")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Orders { get; set; }
    }
}
