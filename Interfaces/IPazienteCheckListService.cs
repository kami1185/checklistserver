using CheckList.Models.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckList.Interfaces
{
    public interface IPazienteCheckListService
    {
        IEnumerable<PazienteList> GetListPatient();
        PazienteCheckList GetPatient(JObject paziente);
    //    void AddStudent(PazienteCheckList paziente);
    //    //void EditStudent(Student student);
    //    void DeleteStudent(PazienteCheckList paziente);
    }
}
