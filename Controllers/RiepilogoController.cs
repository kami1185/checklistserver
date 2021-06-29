using CheckList.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using CheckList.Interfaces;
using System.IO;
using System.Collections;
using CheckList.Models.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheckList.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [EnableCors("MyPolicyRule")]
    public class RiepilogoController : ControllerBase
    {
        // inyection della dipendenza per la creazione del riepilogo pdf
        private readonly IDocumentService _documentService;
        public RiepilogoController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        /// /////////////////////////////////////////////////////////////

        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        //https://localhost:44366/riepilogo/getpatient
        [Route("getpatient")]
        [HttpPost]
        public IActionResult GetPatient()
        {
            try
            {
                //string json_data = JsonConvert.SerializeObject(date, Formatting.Indented);
                //dynamic json2 = JObject.Parse(json_data);

                // dati che vengono impostati x il plugin
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

                List<object> data = new();
                //List<paziente> query = new List<paziente>();
                

                using (checklistContext db = new())
                {
                    //DateTime startDate = DateTime.Parse(json2.data.Value);
                    //DateTime startDate = DateTime.Parse(date);
                    

                    //List<checklist> query3 = new List<checklist>();

                    //List<checklist> data_date = db.checklist.Where(c => c.data >= startDate && c.data <= endDate).ToList();

                    //foreach (checklist ck in data_date)
                    //{
                    //    int? ip = ck.idPaziente;
                    //    var query = (from p in db.paziente
                    //                 join ch in db.checklist on p.id equals ch.idPaziente
                    //                 where p.id == ip
                    //                 select new
                    //                 {
                    //                     p.id,
                    //                     p.nome,
                    //                     p.cognome,
                    //                     p.codiceFiscale,
                    //                     ch.data
                    //                 }).ToList();

                    //    data.Add(query);
                    //}

                    //var  query3 = (from chk in db.checklist
                    //          join pz in db.paziente on chk.idPaziente equals pz.id
                    //          where chk.data >= startDate && chk.data <= endDate &&
                    //             pz.id == chk.idPaziente
                    //          select new
                    //          {
                    //              pz.id,
                    //              pz.nome,
                    //              pz.cognome,
                    //              pz.codiceFiscale,
                    //              chk.data
                    //          }).ToList();


                    if (searchValue != "") 
                    {
                        var query2 = (from p in db.paziente
                                        join ch in db.checklist on p.id equals ch.idPaziente
                                      where
                                        p.id == ch.idPaziente
                                        && p.nome.Contains(searchValue.Trim())
                                        || p.cognome.Contains(searchValue.Trim()) 
                                        || p.codiceFiscale.Contains(searchValue.Trim())
                                        orderby ch.data descending
                                      //|| string.Concat(p.nome, " ", p.cognome).Contains(searchValue)
                                      select new
                                        {
                                            ch.id,
                                            //p.id,
                                            p.nome,
                                            p.cognome,
                                            p.codiceFiscale,
                                            ch.data,
                                            idchecklist = ch.id
                                        }).ToList();

                        recordsTotal = query2.Count();
                        query2 = query2.Skip(skip).Take(pageSize).ToList();

                        var jsonData2 = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = query2 };
                        return Ok(jsonData2);
                    }
                    if (dateStart != "")
                    {
                        DateTime startDate = DateTime.Parse(dateStart);
                        var query = (from p in db.paziente
                                     join ch in db.checklist on p.id equals ch.idPaziente
                                     where ch.data == startDate &&
                                        p.id == ch.idPaziente
                                        orderby ch.data descending
                                     select new
                                     {
                                         ch.id,
                                         //p.id,
                                         p.nome,
                                         p.cognome,
                                         p.codiceFiscale,
                                         ch.data,
                                         idchecklist = ch.id
                                     }).ToList();

                        recordsTotal = query.Count();
                        query = query.Skip(skip).Take(pageSize).ToList();

                        var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = query };
                        return Ok(jsonData);
                    }
                    else 
                    {
                        //DateTime startDate = DateTime.Parse(dateStart);
                        //DateTime endDate = DateTime.Parse(dateEnd);
                        //if(dateStart != "" && dateEnd != "") { 
                        var query = (from p in db.paziente
                                        join ch in db.checklist on p.id equals ch.idPaziente
                                     where p.id == ch.idPaziente
                                        orderby ch.data descending
                                     select new
                                     {
                                         ch.id,
                                         //p.id,
                                         p.nome,
                                         p.cognome,
                                         p.codiceFiscale,
                                         ch.data,
                                         idchecklist = ch.id
                                     }).ToList();

                    //}
                        recordsTotal = query.Count();
                        query = query.Skip(skip).Take(pageSize).ToList();

                        var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = query };
                        return Ok(jsonData);
                    }
                }
                //var data_table = data.Skip(skip).Take(pageSize).ToList();
                //return new JsonResult(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message + e.InnerException);
            }
        }


        //https://localhost:44366/riepilogo/getsummarypatient
        [Route("getsummarypatient")]
        [HttpPost]
        public IActionResult GetSummaryPatient([FromBody] int id_checklist)
        {
            try
            {
                //List<paziente> paziente_data = new List<paziente>();
                //var paziente_data = new paziente();
                List<RiepilogoPdf> checklist_data = new();
                List<riepilogo> domande_data = new();
                List<paziente> paziente_data2 = new();
                using (checklistContext db = new())
                {

                    PazienteCheckListPDF paziente_data = new();

                    //var paziente_data1 = (from p in db.paziente
                    //                  join ch in db.checklist on p.id equals ch.idPaziente
                    //                  where ch.id == id_checklist
                    //                  select new paziente
                    //                  {
                    //                      id = p.id,
                    //                      nome = p.nome,
                    //                      cognome = p.cognome,
                    //                      dataNascita = p.dataNascita,
                    //                      codiceFiscale = p.codiceFiscale

                    //                  }).SingleOrDefault();

                    paziente_data = (from p in db.paziente
                                join ch in db.checklist on p.id equals ch.idPaziente
                                where ch.id == id_checklist
                                select new PazienteCheckListPDF
                                {
                                    Nome = p.nome,
                                    Cognome = p.cognome,
                                    DataNascista = p.dataNascita.ToString(),
                                    DataRicovero = ch.data.ToString(),
                                    Diagnosi = ch.diagnosi,
                                    Percorso = ch.percorso

                                }).SingleOrDefault();

                    string filename = paziente_data.Nome + " " + paziente_data.Cognome +".pdf";

                    checklist_data = (from r in db.riepilogo
                                      join ch in db.checklist on r.idChecklist equals ch.id
                                      join d in db.domande on r.idDomande equals d.id
                                      join nc in db.noconformita on r.idNoconformita equals nc.id
                                      where r.idChecklist == id_checklist
                                      orderby nc.id ascending
                                      select new RiepilogoPdf
                                      {
                                          checkId = r.idChecklist.ToString(),
                                          domandaId = d.id.ToString(),
                                          faseId = d.idFase.ToString(),
                                          domanda = d.domanda,
                                          noconformitaId = nc.id.ToString(),
                                          risposta = nc.testo,
                                      }).ToList();
                    

                    var domande = db.domande.ToList();
                    var nnc = db.noconformita.ToList();
                    var fasi = db.fase.ToList();

                    JArray checklist = new JArray();
                    foreach (domande d in domande)
                    {
                        JArray risposte = new JArray();
                        System.Diagnostics.Debug.WriteLine("domandaid: " + d.id);
                        System.Diagnostics.Debug.WriteLine("domanda: " + d.domanda);

                        foreach (var item in checklist_data)
                        {
                            if (Int32.Parse(item.domandaId) == d.id)
                            {
                                System.Diagnostics.Debug.WriteLine("noconformitaid: " + item.noconformitaId);
                                System.Diagnostics.Debug.WriteLine("testo: " + item.risposta);
                                JObject risposta = new JObject()
                                    {
                                        { "testo", item.risposta },
                                        { "idnc", item.noconformitaId }
                                    };
                                risposte.Add(risposta);
                            }
                        }

                        JObject questions = new();
                        questions = new JObject
                        {
                            { "idfase", d.idFase },
                            { "id", d.id },
                            { "domanda", d.domanda },
                            new JProperty("risposte", risposte)
                        };
                        
                        checklist.Add(questions);
                    }

                    System.Diagnostics.Debug.WriteLine("fases: " + checklist);

                    var pdfFile = _documentService.GeneratePdfFromString(paziente_data, checklist);
                    //return new FileContentResult(pdfFile, "application/pdf");
                    return File(pdfFile, "application/pdf", filename);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message + e.InnerException);
            }
        }

        public void aa(paziente data)
        {
            System.Diagnostics.Debug.WriteLine(data.nome);
            System.Diagnostics.Debug.WriteLine(data.cognome);
        }

        //https://localhost:44366/riepilogo/getpatientdate
        [Route("getpatientdate")]
        [HttpPost]
        public IActionResult GetPatientDate()
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

                List<object> data = new();
                using (checklistContext db = new())
                {
                    //DateTime startDate = DateTime.Parse(json2.data.Value);
                    DateTime startDate = DateTime.Parse(dateStart);
                    //DateTime startDate = DateTime.Parse("23/04/2021");
                    //DateTime endDate = DateTime.Parse(dateEnd);

                    var query = (from p in db.paziente
                                 join ch in db.checklist on p.id equals ch.idPaziente
                                 where ch.data == startDate &&
                                    p.id == ch.idPaziente
                                 select new
                                 {
                                     p.id,
                                     p.nome,
                                     p.cognome,
                                     p.codiceFiscale,
                                     ch.data
                                 }).ToList();

                    recordsTotal = query.Count();
                    query = query.Skip(skip).Take(pageSize).ToList();

                    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = query };
                    return Ok(jsonData);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message + e.InnerException);
            }
        }

        //https://localhost:44366/riepilogo/getpatientname
        [Route("getpatientname")]
        [HttpGet]
        public IActionResult GetPatientName(string q)
        {
            try
            { 
                using (checklistContext db = new())
                {
                    //checklist data_date = new checklist();
                    var data_paziente = db.paziente.Where(
                            p => p.nome.Contains(q) || p.cognome.Contains(q)
                        ).Select(p => p.nome).ToList();

                    //var data_paziente = (from p in db.paziente
                    //             where p.nome.Contains(q) || p.cognome.Contains(q)
                    //             select new
                    //             {
                    //p.nome
                    //             }).ToList();

                    return Ok(data_paziente);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message + e.InnerException);
            }
        }

        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<ValuesController>
        //[HttpPost]
        //public string Post([FromBody] string value)
        //{
        //    return value;
        //}

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
