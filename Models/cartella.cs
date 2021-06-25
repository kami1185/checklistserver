using System;
using System.Collections.Generic;

#nullable disable

namespace CheckList.Models
{
    public partial class cartella
    {
        public int id { get; set; }
        public int? numeroCartella { get; set; }
        public string unitaOperativa { get; set; }
        public string percorsoAssistenziale { get; set; }
        public DateTime? dataPianificata { get; set; }
        public string indicazioniRicovero { get; set; }
        public string diagnosi { get; set; }
        public int? idPaziente { get; set; }

        public virtual paziente idPazienteNavigation { get; set; }
    }
}
