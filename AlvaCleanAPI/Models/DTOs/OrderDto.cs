﻿using MongoDB.Bson.Serialization.Attributes;

namespace AlvaCleanAPI.Models.DTOs
{
    public class OrderDto
    {
        [BsonElement("OrderType")]
        public string OrderType { get; set; }

        [BsonElement("CustomerId")]
        public string CustomerId { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }

        [BsonElement("OrderPriceType")]
        public string OrderPriceType { get; set; }

        [BsonElement("ClientComments")]
        public string? ClientComments { get; set; }

        [BsonElement("OrderStartDate")]
        public DateTime OrderStartDate { get; set; }

        [BsonElement("Area")]
        public double Area { get; set; }

        [BsonElement("Hour")]
        public double Hour { get; set; }

        [BsonElement("CustomerName")]
        public string CustomerName { get; set; }

        [BsonElement("Price")]
        public double Price { get; set; }

    }
}
