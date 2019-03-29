using System.Collections.Generic;
using System.Linq;
using mongotree.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using mongotree.DTO;

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

        public void AdicionarNo(UnidadeDTO unidade)
        {
            var elementoPaiFilho = _unidades.Find(u => u.Filho == unidade.Pai).ToList();

            var quantidadeElementosPaiFilho = _unidades.Find(u => u.Pai == unidade.Pai).CountDocuments();

            string novaPosicao = elementoPaiFilho.ElementAt(0).Posicao+"-"+(quantidadeElementosPaiFilho+1);

            Unidade novaUnidade = new Unidade{
                Id = unidade.Id,
                Pai = unidade.Pai,
                Filho = unidade.Filho,
                Posicao = novaPosicao
            };
            
            _unidades.InsertOne(novaUnidade);
            
        }
    }
}