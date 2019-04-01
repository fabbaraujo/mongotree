using System.Collections.Generic;
using mongotree.Models;

namespace mongotree.DTO
{
    public class UnidadeDTO
    {
        public string Id {get; set;}
        public string Pai {get; set;}
        public string Filho {get; set;}
        public string Posicao { get; set; }

        public string NovoPai {get; set;}
        public string IdNovoPai {get; set;}
    }
}