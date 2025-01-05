using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AlvaCleanAPI.Models.DTOs
{
    public class EmployeerDtoForFetchData
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

        [BsonElement("Orders")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Orders { get; set; }
    }
}
