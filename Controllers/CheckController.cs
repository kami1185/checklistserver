using CheckList.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckList.Controllers
{

    //[Route("api/[controller]/")]
    [Route("/[controller]/")]
    [ApiController]
    [EnableCors("MyPolicyRule")]
    public class CheckController : ControllerBase
    {
        //https://localhost:44366/check/create
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                using (checklistContext db = new checklistContext())
                {
                    var domande = db.domande.ToList();
                    var nnc = db.noconformita.ToList();

                    JArray fase = new JArray();
                    foreach (domande d in domande)
                    {
                        //Console.WriteLine("domanda: " + d.domanda + " fase: " + d.idFase);

                        JArray risposte2 = new JArray();
                        foreach (noconformita nc in nnc)
                        {
                            if (d.id == nc.idDomande)
                            {
                                //Console.WriteLine("testo: " + nc.testo + " id_domanda: " + nc.idDomande);
                                JObject risposte = new JObject()
                                {
                                    { "id", nc.id },
                                    { "testo", nc.testo },
                                    { "level", nc.levelConformita },
                                };
                                risposte2.Add(risposte);
                            }
                        }

                        JObject fasi = new JObject
                        {
                            { "id", d.id },
                            { "domanda", d.domanda },
                            { "idfase", d.idFase },
                            new JProperty("risposte", risposte2)
                        };
                        fase.Add(fasi);
                    }

                    var str = JsonConvert.SerializeObject(fase, Formatting.Indented);
                    return new JsonResult(str);
                }
            }
            catch (Exception e)
            {
                //Logging.Instance.Error(this.GetType(), "Error: {0}" + e);
                return BadRequest(e.Message);
            }
        }

        //https://localhost:44366/check/save
        [Route("save")]
        [HttpPost]
        public IActionResult Save([FromBody] JObject jsondata)
        //public IActionResult Save([FromBody] )
        {
            try
            {
                string json = JsonConvert.SerializeObject(jsondata, Formatting.Indented);

                dynamic json2 = JObject.Parse(json);
                Console.WriteLine("Json: " + json2);

                JArray jchecklist;
                try
                { jchecklist = json2.checklist; }
                catch
                { jchecklist = new JArray() { json2.checklist }; }

                JArray jriepilogo;
                try
                { jriepilogo = json2.riepilogo; }
                catch
                { jriepilogo = new JArray() { json2.riepilogo }; }

                using (checklistContext db = new())
                {
                    // salviamo i dati nella tabella checklist
                    // creiamo l'oggetto checklist con i campi della tabella  
                    var checklist = new checklist();
                    for (int i = 0; i < jchecklist.Count; i++)
                    {
                        dynamic element = jchecklist[i];
                        
                        checklist.data = DateTime.Now.Date;  
                        checklist.idPaziente = 1;
                        checklist.idReparto = 8010;
                        checklist.signinInit = DateTime.Parse(element.signinInit.Value);
                        checklist.signinEnd = DateTime.Parse(element.signinEnd.Value);
                        checklist.timeoutInit = DateTime.Parse(element.timeoutInit.Value);
                        checklist.timeoutEnd = DateTime.Parse(element.timeoutEnd.Value);
                        checklist.signoutInit = DateTime.Parse(element.signoutInit.Value);
                        checklist.signoutEnd = DateTime.Parse(element.signoutEnd.Value);
                    }
                    db.checklist.Add(checklist);
                    db.SaveChanges();

                    ////////////// forma ottimizzata per salvare i dati //////////////////
                    // salviamo i dati nella tabella riepilogo usando una list e la 
                    // funzione AddRange
                    List<riepilogo> list_riepilogo = new List<riepilogo>();
                    for (int i = 0; i < jriepilogo.Count; i++)
                    {
                        dynamic element = jriepilogo[i];
                        var riepilogo1 = new riepilogo
                        {
                            idChecklist = checklist.id,
                            idDomande = Int32.Parse(element.idDomande.Value),
                            idNoconformita = Int32.Parse(element.idNoconformita.Value),
                            risposta = element.risposta.Value
                        };
                        
                        list_riepilogo.Add(riepilogo1);
                        //db.riepilogo.Add(riepilogo1);
                    }

                    db.riepilogo.AddRange(list_riepilogo);
                    // salviamo i dati nel database
                    db.SaveChanges();

                    ///////////////////////////////////////////////////////////////
                }

                var response_save = new
                {
                    successful = "save",
                    messagge = "La checklist si è salvata correttamente"
                };

                //Tranform it to Json object
                string json_data = JsonConvert.SerializeObject(response_save);
                return new JsonResult(json_data);
                //return Ok(json_data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message + e.InnerException);
            }
        }

        //https://localhost:44366/check/save
        [Route("savepaziente")]
        [HttpPost]
        public IActionResult SavePaziente([FromBody] JObject jsondata)
        {
            try
            {
                string json = JsonConvert.SerializeObject(jsondata, Formatting.Indented);
                dynamic data_json = JObject.Parse(json);

                using (checklistContext db = new())
                {
                    string nome = data_json.paziente_nome;
                    string cognome = data_json.paziente_cognome;
                    string paziente_nascita = data_json.paziente_nascita;
                    string paziente_sesso = data_json.paziente_sesso;
                    string paziente_codicefiscale = data_json.paziente_codicefiscale;
                    string paziente_cartella = data_json.paziente_cartella;
                    string paziente_unitaoperativa = data_json.paziente_unitaoperativa;
                    string paziente_percorso = data_json.paziente_percorso;
                    string paziente_dataricovero = data_json.paziente_dataricovero;
                    string paziente_indicazioni = data_json.paziente_indicazioni;
                    string paziente_diagnosi = data_json.paziente_diagnosi;
                }

                return Ok(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message + e.InnerException);
            }
        }

        //[HttpGet]
        //public IActionResult GetDataPatient()
        //{
        //    try
        //    {
        //        string outp = "READ";

        //        return Ok(outp);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }

        //}

        //[HttpGet]
        //public IActionResult Update()
        //{

        //    //Logging.Instance.Information(this.GetType(), "Received cmd: /data");
        //    try
        //    {
        //        string outp = "UPDATE";

        //        return Ok(outp);
        //    }
        //    catch (Exception e)
        //    {
        //        //Logging.Instance.Error(this.GetType(), "Error: {0}" + e);
        //        return BadRequest(e.Message);
        //    }

        //}

        //[HttpGet]
        //public IActionResult Delete()
        //{

        //    //Logging.Instance.Information(this.GetType(), "Received cmd: /data");
        //    try
        //    {
        //        string outp = "DEL";

        //        return Ok(outp);
        //    }
        //    catch (Exception e)
        //    {
        //        //Logging.Instance.Error(this.GetType(), "Error: {0}" + e);
        //        return BadRequest(e.Message);
        //    }

        //}
    }
}
