using System.Collections.Generic;
using System.Linq;
using mongotree.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace mongotree.DAO
{
    public class UnidadeDAO
    {
        private readonly IMongoCollection<Unidade> _unidades;

        public UnidadeDAO(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("ModelTree"));
            var database = client.GetDatabase("ModelTree");
            _unidades = database.GetCollection<Unidade>("Unidade");
        }

        public List<Unidade> Get()
        {
            return _unidades.Find(unidade => true).ToList();
        }

        public Unidade Get(string id)
        {
            return _unidades.Find<Unidade>(unidade => unidade.Id == id).FirstOrDefault();
        }

        public Unidade Create(Unidade unidade)
        {
            _unidades.InsertOne(unidade);
            return unidade;
        }

        public void Update(string id, Unidade unidadeIn)
        {
            _unidades.ReplaceOne(unidade => unidade.Id == id, unidadeIn);
        }

        public void Remove(string id)
        {
            _unidades.DeleteOne(unidade => unidade.Id == id);
        }
    }
}