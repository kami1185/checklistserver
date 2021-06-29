using CheckList.Interfaces;
using CheckList.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CheckList.Services
{
    public class PazienteCheckListService : IPazienteCheckListService
    {

        private readonly string _connectionString;
        public PazienteCheckListService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }
  

        public IEnumerable<PazienteList> GetListPatient()
        {
            List<PazienteList> pazienteList = new();
            //PazienteCheckList lstpaz = new()
            //{
            //    Nome = "Camilo Andres",
            //    Cognome = "Duarte",
            //    DataRicovero = "23-GIU-21",
            //    //Diagnosi = reader.GetString(3),
            //    //Stato = reader.GetString(6),
            //    Percorso = "Ricovero ordinario",
            //    Identifier = "Camilo Andres".Replace(" ", "-") + "," + "Duarte" + "," + "23-GIU-21"
            //};
            //pazienteList.Add(lstpaz);

            //PazienteCheckList lstpaz2 = new()
            //{
            //    Nome = "vanessa",
            //    Cognome = "firenze",
            //    DataRicovero = "23-GIU-21",
            //    //Diagnosi = reader.GetString(3),
            //    //Stato = reader.GetString(6),
            //    Percorso = "Ricovero ordinario",
            //    Identifier = "vanessa".Replace(" ", "-") + "," + "firenze" + "," + "23-GIU-21"
            //};
            //pazienteList.Add(lstpaz2);

            using (OracleConnection con = new(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;

                        //Use the command to display paziente data from the V_LISTAATTESA table
                        cmd.CommandText = "select \"nome\", \"cognome\", \"data ricovero\", \"DIAGNOSI\", \"percorso\" from V_LISTAATTESA where \"data ricovero\" = :data";
                        //cmd.CommandText = "select \"nome\", \"cognome\", \"data nascita\", \"data ricovero\", \"dayservice\", \"DIAGNOSI\", \"stato\", \"percorso\" from V_LISTAATTESA";

                        DateTime thisDay = DateTime.Today;
                        string data_ricovero = thisDay.ToString("dd-MMM-yy", CultureInfo.CreateSpecificCulture("it-IT"));

                        // Assign date to the paziente date :data 
                        OracleParameter nome = new(":data", data_ricovero);
                        cmd.Parameters.Add(nome);

                        //Execute the command and use DataReader to display the data
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            System.Diagnostics.Debug.WriteLine("name: " + reader.GetString(0) + " cognome: " + reader.GetString(1) + "\n");

                            string nome2 = reader.GetString(0);
                            nome2 = nome2.Replace(" ", "-").Replace("'", "\\'");

                            string cognome2 = reader.GetString(1);
                            cognome2 = cognome2.Replace(" ", "-").Replace("'", "\\'");

                            PazienteList lstpaz = new()
                            {
                                Nome = reader.GetString(0),
                                Cognome = reader.GetString(1),
                                DataRicovero = reader.GetString(2),
                                //Diagnosi = reader.GetString(3),
                                //Stato = reader.GetString(6),
                                Percorso = reader.GetString(4),
                                //Identifier = reader.GetString(0).Replace(" ", "-") + "," + reader.GetString(1).Replace(" ", "-") + "," + reader.GetString(2)
                                Identifier = nome2 + "," + cognome2 + "," + reader.GetString(2)
                            };
                            pazienteList.Add(lstpaz);
                        }

                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        //await context.Response.WriteAsync(ex.Message);
                    }
                }
            }
            return pazienteList;
        }


        public PazienteCheckList GetPatient(JObject paziente)
        {
            string json = JsonConvert.SerializeObject(paziente, Formatting.Indented);
            dynamic data_paziente = JObject.Parse(json);

            string p_nome1 = data_paziente.nome;
            string p_cognome1 = data_paziente.cognome;


            //string p_nome2 = Regex.Replace(p_nome1, @"[^0-9]", "");

            //p_nome1 = p_nome1.Replace("-", " ");

            string p_nome = p_nome1.Replace("-", " ");
            string p_cognome = p_cognome1.Replace("-", " ");
            string p_ricovero = data_paziente.data_ricovero;

            PazienteCheckList lstpaz = new();

            //PazienteCheckList lstpaz = new()
            //{
            //    Nome = "Camilo Andres",
            //    Cognome = "Duarte",
            //    DataRicovero = "23-GIU-21",
            //    Diagnosi = "20200 LINFOMA NODULARE,SITO NON SPECIFICATO,ORGANI SOLIDI O SITI EXTRANODALI",
            //    //Stato = reader.GetString(6),
            //    Percorso = "Ricovero ordinario",
            //    Identifier = "Camilo Andres".Replace(" ", "-") + "," + "Duarte" + "," + "23-GIU-21"
            //};

            using (OracleConnection con = new(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;

                        //Use the command to display paziente data from the V_LISTAATTESA table
                        cmd.CommandText = "select \"nome\", \"cognome\", \"data nascita\", \"DIAGNOSI\", \"percorso\" from V_LISTAATTESA where \"nome\"= :nome and \"cognome\"= :cognome and \"data ricovero\" = :data";
                        //cmd.CommandText = "select \"nome\", \"cognome\", \"data nascita\", \"data ricovero\", \"dayservice\", \"DIAGNOSI\", \"stato\", \"percorso\" from V_LISTAATTESA";

                        DateTime thisDay = DateTime.Today;
                        string data_ricovero = thisDay.ToString("dd-MMM-yy", CultureInfo.CreateSpecificCulture("it-IT"));

                        // Assign date to the paziente date :data 
                        OracleParameter nome = new(":nome", p_nome);
                        cmd.Parameters.Add(nome);
                        OracleParameter cognome = new(":cognome", p_cognome);
                        cmd.Parameters.Add(cognome);
                        OracleParameter data = new(":data", p_ricovero);
                        cmd.Parameters.Add(data);

                        //Execute the command and use DataReader to display the data
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            System.Diagnostics.Debug.WriteLine("name: " + reader.GetString(0) + " cognome: " + reader.GetString(1) + "\n");
                            lstpaz = new()
                            {
                                Nome = reader.GetString(0),
                                Cognome = reader.GetString(1),
                                DataNascista = reader.GetString(2),
                                Diagnosi = reader.GetString(3),
                                Percorso = reader.GetString(4),
                                //Identifier = reader.GetString(0).Replace(" ", "-") + "," + reader.GetString(1).Replace(" ", "-") + "," + reader.GetString(2)
                            };
                        }
                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
            return lstpaz;
        }

    }
}
