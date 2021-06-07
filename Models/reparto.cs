using System;
using System.Collections.Generic;

#nullable disable

namespace CheckList.Models
{
    public partial class reparto
    {
        public reparto()
        {
            checklist = new HashSet<checklist>();
        }

        public int id { get; set; }
        public string nome { get; set; }

        public virtual ICollection<checklist> checklist { get; set; }
    }
}
