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

        [HttpPost("adicionarNo")]
        public ActionResult<Unidade> AdicionarNo(UnidadeDTO unidade)
        {
            
            _unidadeDAO.AdicionarNo(unidade);

            return Ok();
        }
    }
}