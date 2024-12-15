using MongoDB.Bson.Serialization.Attributes;

namespace AlvaCleanAPI.Models.DTOs
{
    public class OrderDto
    {
        [BsonElement("OrderType")]
        public string OrderType { get; set; }

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
