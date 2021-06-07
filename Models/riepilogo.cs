using System;
using System.Collections.Generic;

#nullable disable

namespace CheckList.Models
{
    public partial class riepilogo
    {
        public int id { get; set; }
        public int idChecklist { get; set; }
        public int idDomande { get; set; }
        public int? idNoconformita { get; set; }
        public string risposta { get; set; }

        public virtual checklist idChecklistNavigation { get; set; }
        public virtual domande idDomandeNavigation { get; set; }
        public virtual noconformita idNoconformitaNavigation { get; set; }
    }
}
