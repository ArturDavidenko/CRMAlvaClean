using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AlvaCleanAPI.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("CustomerType")]
        public string CustomerType { get; set; }

        [BsonElement("ClientName")]
        public string ClientName { get; set; }

        [BsonElement("ContactPhone")]
        public string ContactPhone { get; set; }

        [BsonElement("Orders")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Orders { get; set; }
    }
}
