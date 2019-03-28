using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongotree.Models
{
    public class Unidade
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; }

        [BsonElement("children")]
        public List<Unidade> children {get; set;}
    }
}