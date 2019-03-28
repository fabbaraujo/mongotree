using System.Collections.Generic;
using mongotree.Models;
using mongotree.DAO;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id:length(24)}", Name = "GetUnidade")]
        public ActionResult<Unidade> Get(string id)
        {
            var unidade = _unidadeDAO.Get(id);

            if (unidade == null)
            {
                return NotFound();
            }

            return unidade;
        }

        [HttpPost]
        public ActionResult<Unidade> Create(Unidade unidade)
        {
            _unidadeDAO.Create(unidade);

            return Ok();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Unidade unidadeIn)
        {
            var unidade = _unidadeDAO.Get(id);

            if (unidade == null)
            {
                return NotFound();
            }

            _unidadeDAO.Update(id, unidadeIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var unidade = _unidadeDAO.Get(id);

            if (unidade == null)
            {
                return NotFound();
            }

            _unidadeDAO.Remove(unidade.Id);

            return NoContent();
        }
    }
}