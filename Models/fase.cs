using System;
using System.Collections.Generic;

#nullable disable

namespace CheckList.Models
{
    public partial class fase
    {
        public fase()
        {
            domande = new HashSet<domande>();
        }

        public int id { get; set; }
        public string nome { get; set; }

        public virtual ICollection<domande> domande { get; set; }
    }
}
