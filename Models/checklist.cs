using System;
using System.Collections.Generic;

#nullable disable

namespace CheckList.Models
{
    public partial class checklist
    {
        public checklist()
        {
            riepilogo = new HashSet<riepilogo>();
        }

        public int id { get; set; }
        public DateTime? data { get; set; }
        public int? idPaziente { get; set; }
        public int? idReparto { get; set; }
        public DateTime? signinInit { get; set; }
        public DateTime? signinEnd { get; set; }
        public DateTime? timeoutInit { get; set; }
        public DateTime? timeoutEnd { get; set; }
        public DateTime? signoutInit { get; set; }
        public DateTime? signoutEnd { get; set; }
        public string diagnosi { get; set; }
        public string percorso { get; set; }

        public virtual paziente idPazienteNavigation { get; set; }
        public virtual reparto idRepartoNavigation { get; set; }
        public virtual ICollection<riepilogo> riepilogo { get; set; }
    }
}
