using CheckList.Interfaces;
using CheckList.Models;
using DinkToPdf;
using DinkToPdf.Contracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckList.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IConverter _converter;
        //private readonly IRazorRendererHelper _razorRendererHelper;

        //public DocumentService(IConverter converter, IRazorRendererHelper razorRendererHelper)
        public DocumentService(IConverter converter)
        {
            _converter = converter;
            //_razorRendererHelper = razorRendererHelper;
        }

        public byte[] GeneratePdfFromString(PazienteCheckListPDF paziente_data, JArray checklist)
        {

            //foreach (JObject item in checklist) // <-- Note that here we used JObject instead of usual JProperty
            //{
            //    string domanda = item.GetValue("domanda").ToString();
            //    JToken risposte = item.GetValue("risposte");
            //    foreach (JObject risposta in risposte)
            //    {
            //        string risp = risposta.GetValue("testo").ToString();
            //    }
            //}

            string nome_paziente = paziente_data.Nome +" " + paziente_data.Cognome;

			//System.Diagnostics.Debug.WriteLine("fasi: " + checklist);

			var htmlContent = new StringBuilder();
            //var htmlContent = ($@"
            htmlContent.Append(@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <style>
			        
					#container-riepilogo{
						padding-top: 3px;
					}

					.titolo-riepilogo {
						text-align: center;
						font-size: 2.0rem;
						color: #007bff;
					}

					//.data-paziente {
					//	font-size: 1.5rem;
					//}
	
					.titolo-checklist {
						font-size: 2.0rem;
						color: #007bff;
						padding-left: 40px;
					}
			
					li {
						list-style-type: none;
						margin-bottom: 4px;
					}			  

			        //ol.check-list li {
				       // border: 1.5px solid #333;
				       // border-radius: 10px;
				       // -moz-border-radius: 10px;
				       // -webkit-border-radius: 10px;
				       // -o-border-radius: 10px;
				       // -ms-border-radius: 10px;
				       // box-sizing: border-box;
				       // display: block;
				       // margin: 10px 0;
				       // padding: 5px 10px;
				       // text-decoration: none;
			        //}

			        ol.check-list li.column {
				        border: 1.7px solid #04b72d;
				        border-radius: 10px;
				        -moz-border-radius: 10px;
				        -webkit-border-radius: 10px;
				        -o-border-radius: 10px;
				        -ms-border-radius: 10px;
				        box-sizing: border-box;
				        display: block;
				        margin: 10px 5px;
				        padding: 8px 5px;
				        text-decoration: none;
				        max-width: 90%;
				        background: #fdfdfd;
			        }

			        .riepilogo-domanda {
				        margin: 10px 0;
				        font-size: 1.4rem;
			        }

			        .riepilogo-buttons i {
				        text-align: center;
				        font-size: 2rem;
				        color: #28BF8D;
			        }

			        .button-non-conformita{
				        background: #007bff;
				        width: 35px; 
				        height: 35px;
				        border-radius: 50%;
				        display: flex; /* or inline-flex */
				        align-items: center; 
				        justify-content: center;
				        color: #fff;
			        }

			        .riepilogo-conformita {
				        font-size: 1.3rem;
			        }

			        hr {
				        border: 0;
				        height: 1.4px;
				        background: #ccc;
				        /* background-image: linear-gradient(to right, #ccc, #333, #ccc); */
			        }

			        .label-data {
				        color: #222222;
				        font-size: 1.4rem;
				        padding-left: 5px;
						padding-top: 6px;
			        }

					textarea{
						color: #222222;
				        font-size: 1.4rem;	
					}
		        </style>
            </head>
            <body>

                <div id='container-riepilogo' style='width:100%; page-break-after: always;'>
                    
			            <div>
				            <h1 class='titolo-riepilogo'>Riepilogo Check List</h1>
			            </div>
		                
			            <div>
				            <ol id='fase_signin' class='check-list'>
					                
					            <li class='column data-paziente'>");
                                htmlContent.AppendFormat(@"
                                    <label class='label-data'>Nome:           <span >{0}</span></label><br>
						            <label class='label-data'>Cognome:        <span class='label-data'>{1}</span></label><br>
									<label class='label-data'>Data nascita:   <span class='label-data'>{2}</span></label><br>
                                    <label class='label-data'>Data ricovero: <span class='label-data'>{3}</span></label><br>
									<label class='label-data'>Percorso: <span class='label-data'>{4}</span></label><br>
									<label class='label-data'>Diagnosi: </label><textarea  class='form-control' rows='3' cols='75%' style='border: solid 0px; vertical-align: top;'>{5}</textarea>
								  </li>
					                
				            </ol>
			            </div>
                        ", paziente_data.Nome.ToUpper(), paziente_data.Cognome.ToUpper(), paziente_data.DataNascista, paziente_data.DataRicovero.ToUpper(),
						   paziente_data.Percorso.ToUpper(), paziente_data.Diagnosi.ToUpper());

							foreach (JObject item in checklist)
							{
								if (item.GetValue("idfase").ToString() == "1" && item.GetValue("id").ToString() == "1")
								{
									htmlContent.Append(@"
										<div class='row'>
											<div class='titolo-div text-center'>
												<h1 class='titolo-checklist'>Fase Sign In</h1>
											</div>
											<hr>
										</div>");
								}
								if (item.GetValue("idfase").ToString() == "2" && item.GetValue("id").ToString() == "12") {
									htmlContent.Append(@"
										<div class='row'>
											<div class='titolo-div text-center'>
												<h1 class='titolo-checklist'>Fase Time Out</h1>
											</div>
											<hr>
										</div>");
								}

								if (item.GetValue("idfase").ToString() == "3" && item.GetValue("id").ToString() == "19")
								{
									htmlContent.Append(@"
										<div class='row'>
											<div class='titolo-div text-center'>
												<h1 class='titolo-checklist'>Fase Sign Out</h1>
											</div>
											<hr>
										</div>");
								}

								htmlContent.AppendFormat(@"
									<table style='width:100%; page-break-after: avoid;'>
										<col width='50%' />
										<col width='50%' />
										<tr> <!-- This is the first row -->
											<td>
												<ol id='fase_signin' class='check-list'>
													<li class='column'>
														<div class='riepilogo-domanda'>
															<div class='row'>
																<div class='col-11'>
																	<span class='title'>{0}</span>
																</div>
															</div>
														</div>
														<hr>
														<div class='riepilogo-conformita'>
															<div class='row'>
																<div class='col-9 riepilogo-buttons'>
																	<ol>", item.GetValue("domanda").ToString());

																		JToken risposte = item.GetValue("risposte");
																		foreach (JObject risposta in risposte)
																		{
																			htmlContent.AppendFormat(@"
																			<li>{0}</li>
																			",risposta.GetValue("testo").ToString());
																		}
																		
																	htmlContent.Append(@"</ol>
																</div>
																<div class='col-1'>
																	<div class='col riepilogo-buttons'>
																		<i class='fas fa-exclamation-triangle'></i>
																	</div>
																</div>
															</div>
														</div>
													</li>
												</ol>
											</td>
											
										</tr>
									</table>
								");
							}
							htmlContent.Append(@"
                </div>

            </body>
            </html>
            ");
            return GeneratePdf(htmlContent.ToString(), nome_paziente);
        }

        private string GenerateFase()
        {
            return "a";
        }

        private byte[] GeneratePdf(string htmlContent, string nome_paziente)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 5, Left = 1, Right = 1,  Bottom = 10 },
				DocumentTitle = nome_paziente
			};

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                //HeaderSettings = { FontSize = 11, Center = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontSize = 8, Center = "Riepilogo", Line = true },
            };

            var htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(htmlToPdfDocument);
        }

        //public byte[] GeneratePdfFromRazorView()
        //{
        //    var invoiceViewModel = GetInvoiceModel();
        //    var partialName = "/Views/PdfTemplate/InvoiceDetails.cshtml";
        //    var htmlContent = _razorRendererHelper.RenderPartialToString(partialName, invoiceViewModel);

        //    return GeneratePdf(htmlContent);
        //}

        //private InvoiceViewModel GetInvoiceModel()
        //{
        //    var invoiceViewModel = new InvoiceViewModel
        //    {
        //        OrderDate = DateTime.Now,
        //        OrderId = 1234567890,
        //        DeliveryDate = DateTime.Now.AddDays(10),
        //        Products = new List<Product>()
        //        {
        //            new Product
        //            {
        //                ItemName = "Hosting (12 months)",
        //                Price = 200
        //            },
        //            new Product
        //            {
        //                ItemName = "Domain name (1 year)",
        //                Price = 12
        //            },
        //            new Product
        //            {
        //                ItemName = "Website design",
        //                Price = 1000

        //            },
        //            new Product
        //            {
        //                ItemName = "Maintenance",
        //                Price = 300
        //            },
        //            new Product
        //            {
        //                ItemName = "Customization",
        //                Price = 400
        //            },
        //        }
        //    };

        //    invoiceViewModel.TotalAmount = invoiceViewModel.Products.Sum(p => p.Price);

        //    return invoiceViewModel;
        //}
    }
}
