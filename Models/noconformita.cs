using System;
using System.Collections.Generic;

#nullable disable

namespace CheckList.Models
{
    public partial class noconformita
    {
        public noconformita()
        {
            riepilogo = new HashSet<riepilogo>();
        }

        public int id { get; set; }
        public string testo { get; set; }
        public int idDomande { get; set; }
        public int levelConformita { get; set; }

        public virtual domande idDomandeNavigation { get; set; }
        public virtual ICollection<riepilogo> riepilogo { get; set; }
    }
}
