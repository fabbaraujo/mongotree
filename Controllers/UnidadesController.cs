using System.Collections.Generic;
using mongotree.Models;
using mongotree.DAO;
using Microsoft.AspNetCore.Mvc;
using mongotree.DTO;

namespace mongotree.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController : ControllerBase
    {
        private readonly UnidadeDAO _unidadeDAO;
        public UnidadesController(UnidadeDAO unidadeDAO)
        {
            _unidadeDAO = unidadeDAO;
        }   

        [HttpGet]
        public ActionResult<List<Unidade>> Get()
        {
            return _unidadeDAO.Get();
        }

        [HttpGet("nivel/{nivel}")]
        public ActionResult<List<Unidade>> GetNivel(int nivel)
        {
            return _unidadeDAO.GetNivel(nivel);
        }

        [HttpGet("filhos/{pai}")]
        public ActionResult<List<Unidade>> GetFilhos(string pai)
        {
            //proximo nivel a partir do pai
            return _unidadeDAO.GetFilhos(pai);
        }

        [HttpPost("adicionarNo")]
        public ActionResult<Unidade> AdicionarNo(UnidadeDTO unidade)
        {
            /*
                {
                    "Filho": "Equipe 02",
                    "Pai": "Gerencia de Teste 01"
                }
             */
            
            _unidadeDAO.AdicionarNo(unidade);

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Unidade unidadeNovo)
        {//ENVIA SÓ O FILHO NO JSON BODY
        //VALIDAR NA BLL SOBRE O PRIMEIRO NÓ - PREFEITURA
        /*
            "id"
            "Filho": "Diretoria de Urbanismo"
         */
            var unidade = _unidadeDAO.Get(id);

            if (unidade == null)
            {
                return NotFound();
            }

            _unidadeDAO.UpdateNo(id, unidadeNovo, unidade);

            return NoContent();
        }

        [HttpPut("moverNo/{id}")]
        public IActionResult MoverNo(string id, UnidadeDTO unidadePosicaoNovo)
        {//ENVIA O FILHO E NOVO PAI NO JSON BODY
        //VALIDAR NA BLL SOBRE O PRIMEIRO NÓ - PREFEITURA
        //VALIDAR NA BLL SOBRE MOVER ELE PARA ELE MESMO E PARA OS FILHOS

        /* {
            "Filho": "Diretoria de Urbanismo",
            "NovoPai": "Secretaria de Administração",
            "IdNovoPai":"5c9e5cb5e8ee2a515940f14b"
        } */

            var unidadePosicaoAntigo = _unidadeDAO.Get(id);

            if (unidadePosicaoAntigo == null)
            {
                return NotFound();
            }

            _unidadeDAO.MoverNo(id, unidadePosicaoNovo, unidadePosicaoAntigo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {//NA BLL NÃO PODE DELETAR O PRIMEIRO NÓ - PREFEITURA
            var unidade = _unidadeDAO.Get(id);

            if (unidade == null)
            {
                return NotFound();
            }

            _unidadeDAO.RemoveNo(unidade);

            return NoContent();
        }
    }
}