using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongotree.Models
{
    public class Unidade
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Pai")]
        public string Pai { get; set; }

        [BsonElement("Filho")]
        public string Filho { get; set; }

        [BsonElement("Posicao")]
        public string Posicao { get; set; }
    }
}