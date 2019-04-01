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

        public Unidade Get(string id)
        {
            return _unidades.Find<Unidade>(u => u.Id == id).FirstOrDefault();
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

        public void RemoveNo(Unidade unidade){
            var unidadesDel = _unidades.Find(u => u.Pai == unidade.Filho).ToList();
            unidadesDel.Add(unidade);
            foreach(var item in unidadesDel){
                _unidades.DeleteOne(unid => unid.Id == item.Id);
            }
        }

        public void UpdateNo(string id, Unidade unidadeNovo, Unidade unidadeAntigo){
            var unidadesUpd = _unidades.Find(u => u.Pai == unidadeAntigo.Filho).ToList();

            Unidade objetoFilhoNovo = new Unidade{
                Id = unidadeAntigo.Id,
                Pai = unidadeAntigo.Pai,
                Filho = unidadeNovo.Filho,
                Posicao = unidadeAntigo.Posicao
            };

            _unidades.ReplaceOne(u => u.Id == id, objetoFilhoNovo);

            foreach(var item in unidadesUpd){
                Unidade objetoPaiNovo = new Unidade{
                    Id = item.Id,
                    Pai = unidadeNovo.Filho,
                    Filho = item.Filho,
                    Posicao = item.Posicao
                };

                _unidades.ReplaceOne(unid => unid.Id == objetoPaiNovo.Id, objetoPaiNovo);
            }
        }
    }
}