using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AlvaCleanAPI.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("CustomerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        [BsonElement("OrderType")]
        public string OrderType { get; set; }

        [BsonElement("OrderDate")]
        public DateTime OrderDate { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }

        [BsonElement("OrderPriceType")]
        public string OrderPriceType { get; set; }

        [BsonElement("ClientComments")]
        public string ClientComments { get; set; }
    }
}
