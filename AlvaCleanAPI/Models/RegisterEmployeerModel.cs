using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AlvaCleanAPI.Models
{
    public class RegisterEmployeerModel
    {
        [BsonElement("Password")]
        public string Password { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("Role")]
        public string Role{ get; set; }

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
