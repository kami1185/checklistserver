using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckList.Models
{

    public class RiepilogoPdf
    {
        //public string idpaziente { get; set; }
        //public string nome { get; set; }
        //public string cognome { get; set; }
        //public string sesso { get; set; }
        //public string codiceFiscale { get; set; }
        //public string procedura { get; set; }
        public string checkId { get; set; }
        public string domandaId { get; set; }
        public string faseId { get; set; }
        public string domanda { get; set; }
        public string noconformitaId { get; set; }
        public string risposta { get; set; }
        public List<noconformita> testonc { get; set; }
    }

    // classe usata per inviare i dati ottenuti dal database
    // alle classi RiepilogoController
    public class PazienteCheckListPDF
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string DataNascista { get; set; }
        public string DataRicovero { get; set; }
        //public string CodiceFiscale { get; set; }
        public string Diagnosi { get; set; }
        public string Percorso { get; set; }
    }

}
