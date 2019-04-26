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

        public List<Unidade> GetNivel(int nivel)
        {
            return _unidades.Find(u => u.Posicao.Length  == nivel).ToList();
        }
        public List<Unidade> GetFilhos(string pai)
        {
            var filhosEncontrados = _unidades.Find(p => p.Pai == pai).ToList();

            return filhosEncontrados;
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

        public void MoverNo(string id, UnidadeDTO unidadePosicaoNovo, Unidade unidadePosicaoAntigo){
            var unidadesUpd = _unidades.Find(u => u.Pai == unidadePosicaoAntigo.Filho).ToList();
            var infoNovoPai = _unidades.Find(u => u.Id == unidadePosicaoNovo.IdNovoPai).ToList();
            var quantidadeElementosNovoPai = _unidades.Find(u => u.Pai == unidadePosicaoNovo.NovoPai).CountDocuments();

            string novaPosicao = infoNovoPai.ElementAt(0).Posicao+"-"+(quantidadeElementosNovoPai+1);

             Unidade objetoMoverNovo = new Unidade{
                Id = unidadePosicaoAntigo.Id,
                Pai = unidadePosicaoNovo.NovoPai,
                Filho = unidadePosicaoNovo.Filho,
                Posicao = novaPosicao
            };

            _unidades.ReplaceOne(u => u.Id == id, objetoMoverNovo);
            int qtdElementosMoverPai = 0;

            foreach(var item in unidadesUpd){
                qtdElementosMoverPai++;
                Unidade objetoFilhoMoverNovo = new Unidade{
                    Id = item.Id,
                    Pai = item.Pai,
                    Filho = item.Filho,
                    Posicao = objetoMoverNovo.Posicao+"-"+qtdElementosMoverPai
                };

                _unidades.ReplaceOne(unid => unid.Id == objetoFilhoMoverNovo.Id, objetoFilhoMoverNovo);
            } 
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