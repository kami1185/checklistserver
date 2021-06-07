using System;
using System.Collections.Generic;

#nullable disable

namespace CheckList.Models
{
    public partial class paziente
    {
        public paziente()
        {
            checklist = new HashSet<checklist>();
        }

        public int id { get; set; }
        public string nome { get; set; }
        public string cognome { get; set; }
        public string sesso { get; set; }
        public string codiceFiscale { get; set; }
        public string procedura { get; set; }
        public string numeroCartella { get; set; }

        public virtual ICollection<checklist> checklist { get; set; }
    }
}
