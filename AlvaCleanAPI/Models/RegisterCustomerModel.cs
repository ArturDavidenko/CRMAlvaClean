using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AlvaCleanAPI.Models
{
    public class RegisterCustomerModel
    {
        [BsonElement("CustomerType")]
        public string CustomerType { get; set; }

        [BsonElement("ClientName")]
        public string ClientName { get; set; }

        [BsonElement("ContactPhone")]
        public string ContactPhone { get; set; }
    }

}

