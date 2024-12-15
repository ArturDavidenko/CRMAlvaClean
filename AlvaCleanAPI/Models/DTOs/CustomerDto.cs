﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AlvaCleanAPI.Models.DTOs
{
    public class CustomerDto
    {
        [BsonElement("ClientName")]
        public string ClientName { get; set; }

        [BsonElement("CompanyName")]
        public string CompanyName { get; set; }

        [BsonElement("ContactPhone")]
        public string ContactPhone { get; set; }
    }
}