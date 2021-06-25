using CheckList.Services;
using CheckList.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckList.Models.ViewModel;
using Newtonsoft.Json.Linq;

namespace CheckList.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [EnableCors("MyPolicyRule")]
    public class ListaController : Controller
    {

        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        IPazienteCheckListService PazienteService;
        public ListaController(IPazienteCheckListService _pazienteService)
        {
            PazienteService = _pazienteService;
        }

        [Route("checklist")]
        [HttpPost]
        public IActionResult Checklist()
        {
            try
            {
                draw = Request.Form["draw"].FirstOrDefault();
                start = Request.Form["start"].FirstOrDefault();
                length = Request.Form["length"].FirstOrDefault();
                sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                searchValue = Request.Form["search[value]"].FirstOrDefault();
                pageSize = length != null ? Convert.ToInt32(length) : 0;
                skip = start != null ? Convert.ToInt32(start) : 0;
                recordsTotal = 0;
                // questi parametri sono inviati dal metodo ajax di DataTable
                // li abbiamo definiti per cercare i dati per la data
                var dateStart = Request.Form["dateinit"].FirstOrDefault();
                //var dateEnd = Request.Form["dateend"].FirstOrDefault();

                IEnumerable<PazienteList> pazienti = PazienteService.GetListPatient();

                recordsTotal = pazienti.Count();
                pazienti = pazienti.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = pazienti };
                return Ok(jsonData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message + e.InnerException);
            }
        }

        [Route("paziente")]
        [HttpPost]
        public IActionResult Paziente([FromBody] JObject jsondata)
        {
            try
            {
                PazienteCheckList pazienti = PazienteService.GetPatient(jsondata); 
                return Ok(pazienti);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message + e.InnerException);
            }
        }

    }
}
