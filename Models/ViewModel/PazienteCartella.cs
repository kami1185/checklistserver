using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckList.Models.ViewModel
{
    public class PazienteCartella
    {
        public int Id { get; set; }
        public string CodiceFiscale { get; set; }

        public List<Cartella> paziente_cartella = new();
    }

    public class Cartella
    {
        public DateTime? DataPianificata { get; set; }
        public int IdPaziente { get; set; }
    }
}
