using CheckList.Interfaces;
using CheckList.Models;
using CheckList.Models.ViewModel;
using CheckList.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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


                JArray jpaziente;
                try
                { jpaziente = json2.paziente; }
                catch
                { jpaziente = new JArray() { json2.paziente }; }

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

                    var paziente = new paziente();

                    for (int i = 0; i < jpaziente.Count; i++)
                    {
                        dynamic element = jpaziente[i];

                        paziente.nome = element.nome;
                        paziente.cognome = element.cognome;

                        string aaa = element.datanascita;

                        var itCulture = CultureInfo.GetCultureInfo("it-IT");
                        string date = DateTime.Parse(aaa, itCulture).ToString("dd-MM-yy");
                        
                        paziente.dataNascita = DateTime.Parse(date);
                        paziente.codiceFiscale = "DFTMUY85A11Z604W";

                    }
                    db.paziente.Add(paziente);
                    db.SaveChanges();

                    // salviamo i dati nella tabella checklist
                    // creiamo l'oggetto checklist con i campi della tabella  
                    var checklist = new checklist();
                    for (int i = 0; i < jchecklist.Count; i++)
                    {
                        dynamic element = jchecklist[i];
                        
                        checklist.data = DateTime.Now.Date;  
                        checklist.idPaziente = paziente.id;
                        checklist.idReparto = 8010;
                        checklist.signinInit = DateTime.Parse(element.signinInit.Value);
                        checklist.signinEnd = DateTime.Parse(element.signinEnd.Value);
                        checklist.timeoutInit = DateTime.Parse(element.timeoutInit.Value);
                        checklist.timeoutEnd = DateTime.Parse(element.timeoutEnd.Value);
                        checklist.signoutInit = DateTime.Parse(element.signoutInit.Value);
                        checklist.signoutEnd = DateTime.Parse(element.signoutEnd.Value);
                        checklist.diagnosi = element.diagnosi;
                        checklist.percorso = element.percorso;
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

                PazienteCartella paziente_cartella = new PazienteCartella();
                string codiceFiscale = data_json.paziente_codicefiscale;

                string response = "";
                DateTime dataricovero;

                using (checklistContext db = new())
                {

                    paziente_cartella = (from p in db.paziente
                                         join ca in db.cartella on p.id equals ca.idPaziente
                                         where p.codiceFiscale == codiceFiscale
                                         select new PazienteCartella
                                         {
                                             Id = p.id,
                                             CodiceFiscale = p.codiceFiscale,
                                             paziente_cartella = (
                                                from c in db.cartella
                                                where c.idPaziente == p.id
                                                select new Cartella
                                                {
                                                    DataPianificata = c.dataPianificata
                                                }).ToList()
                                         }).FirstOrDefault();


                    //foreach(var item in paziente_cartella.paziente_cartella)
                    //{
                    //    DateTime dp = (DateTime)item.dataPianificata;
                    //}


                    //var query = (from p in db.paziente
                    //             join ca in db.cartella on p.id equals ca.idPaziente
                    //             where p.codiceFiscale == codiceFiscale
                    //             select new 
                    //             {
                    //                 p.id,
                    //                 p.codiceFiscale,
                    //                 ca.dataPianificata
                    //             }).ToList();

                    if (paziente_cartella != null) {

                        string data_ricovero = data_json.paziente_dataricovero;
                        dataricovero = DateTime.Parse(data_ricovero);
                        //DateTime datapianificata = (DateTime)query.dataPianificata;
                        //DateTime result = DateTime.ParseExact(data_json.paziente_dataricovero, "dd-MM-yyyy hh.mm.ss", CultureInfo.InvariantCulture);
                        //DateTime dateRicovero = DateTime.ParseExact(data_json.paziente_dataricovero, "yyyyMMddhhmm", null);
                        //DateTime dateRicovero = DateTime.Parse(data_json.paziente_dataricovero);
                        //JValue dateRicovero = new JValue(new DateTime(data_json.paziente_dataricovero));
                        //string dateRicovero1 = (string)dateRicovero;

                        if(paziente_cartella.CodiceFiscale == codiceFiscale && paziente_cartella.paziente_cartella.Count > 0) {

                            bool data_check = false;

                            foreach (var item in paziente_cartella.paziente_cartella) {
                                //if (item.DataPianificata != dataricovero)
                                //{
                                    
                                //}
                                if (item.DataPianificata == dataricovero)
                                {
                                    data_check = true;

                                }
                            }

                            if (!data_check)
                            {
                                var cartella_data = new cartella
                                {
                                    numeroCartella = data_json.paziente_cartella,
                                    unitaOperativa = data_json.paziente_unitaoperativa,
                                    percorsoAssistenziale = data_json.paziente_percorso,
                                    dataPianificata = dataricovero,
                                    indicazioniRicovero = data_json.paziente_indicazioni,
                                    diagnosi = data_json.paziente_diagnosi,
                                    idPaziente = paziente_cartella.Id
                                };

                                db.cartella.Add(cartella_data);
                                db.SaveChanges();

                                response = "Paziente già esistente, la data del ricovero è stata salvata correttamente";

                            }
                            else {
                                response = "Il paziente presenta un ricovero in questa data";
                            }
                        }
                    }
                    else
                    {
                        string data_ricovero = data_json.paziente_dataricovero;
                        dataricovero = DateTime.Parse(data_ricovero);

                        var paziente_data = new paziente
                        {
                            nome = data_json.paziente_nome,
                            cognome = data_json.paziente_cognome,
                            codiceFiscale = data_json.paziente_codicefiscale,
                            dataNascita = data_json.paziente_nascita
                        };

                        db.paziente.Add(paziente_data);
                        db.SaveChanges();

                        var cartella_data = new cartella
                        {
                            numeroCartella = data_json.paziente_cartella,
                            unitaOperativa = data_json.paziente_unitaoperativa,
                            percorsoAssistenziale = data_json.paziente_percorso,
                            dataPianificata = dataricovero,
                            indicazioniRicovero = data_json.paziente_indicazioni,
                            diagnosi = data_json.paziente_diagnosi,
                            idPaziente = paziente_data.id
                        };

                        db.cartella.Add(cartella_data);
                        db.SaveChanges();

                        response = "Paziente salvato correttamento";
                    }
                }

                return Ok(response);
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
