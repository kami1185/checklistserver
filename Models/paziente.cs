using System;
using System.Collections.Generic;

#nullable disable

namespace CheckList.Models
{
    public partial class paziente
    {
        public paziente()
        {
            cartella = new HashSet<cartella>();
            checklist = new HashSet<checklist>();
        }

        public int id { get; set; }
        public string nome { get; set; }
        public string cognome { get; set; }
        public string sesso { get; set; }
        public string codiceFiscale { get; set; }
        public DateTime? dataNascita { get; set; }

        public virtual ICollection<cartella> cartella { get; set; }
        public virtual ICollection<checklist> checklist { get; set; }
    }
}
