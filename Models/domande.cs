using System;
using System.Collections.Generic;

#nullable disable

namespace CheckList.Models
{
    public partial class domande
    {
        public domande()
        {
            noconformita = new HashSet<noconformita>();
            riepilogo = new HashSet<riepilogo>();
        }

        public int id { get; set; }
        public string domanda { get; set; }
        public int idFase { get; set; }

        public virtual fase idFaseNavigation { get; set; }
        public virtual ICollection<noconformita> noconformita { get; set; }
        public virtual ICollection<riepilogo> riepilogo { get; set; }
    }
}
