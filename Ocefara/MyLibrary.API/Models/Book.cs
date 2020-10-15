using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyLibrary.API.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public int PublicationYear { get; set; }
        public string Publisher { get; set; }
        public int Edition { get; set; }
        public bool Digital { get; set; }
        public List<Author> Authors { get; set; }
    }
}
