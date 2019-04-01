using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongotree.Models
{
    public class Funcionario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; }

        [BsonElement("Unidade")]
        public string Unidade { get; set; }

        [BsonElement("IdCargo")]
        public string IdCargo { get; set; }

        [BsonElement("IdFuncao")]
        public string IdFuncao { get; set; }
    }
}