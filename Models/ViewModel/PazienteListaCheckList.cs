using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckList.Models.ViewModel
{
    public class PazienteList
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        //public DateTime DataNascista { get; set; }
        public string DataRicovero { get; set; }
        //public string Dayservice { get; set; }
        public string Diagnosi { get; set; }
        public string Percorso { get; set; }
        public string Identifier { get; set; }

    }

    // classe usata per inviare i dati ottenuti dal database
    // alla classe PazienteCheckListService
    public class PazienteCheckList
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string DataNascista { get; set; }
        //public string DataRicovero { get; set; }
        //public string CodiceFiscale { get; set; }
        public string Diagnosi { get; set; }
        public string Percorso { get; set; }

    }
}
