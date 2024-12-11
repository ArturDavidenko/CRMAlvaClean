using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AlvaCleanAPI.Models.DTOs
{
    public class EmployeerDto
    {
        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("Role")]
        public string Role { get; set; }
    }
}
