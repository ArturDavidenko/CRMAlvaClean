﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AlvaCleanAPI.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ClientName")]
        public string ClientName { get; set; }

        [BsonElement("CompanyName")]
        public string CompanyName { get; set; }

        [BsonElement("ContactPhone")]
        public string ContactPhone { get; set; }

        [BsonElement("Orders")]
        public List<ObjectId> Orders { get; set; }
    }
}