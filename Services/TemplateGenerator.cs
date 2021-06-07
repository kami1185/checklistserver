using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckList.Services
{
    public class TemplateGenerator
    {

        private readonly IConverter _converter;

        public TemplateGenerator(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GetHtmlString(Object dataSummaryPatient)
        {

            //var pdf_html = new StringBuilder();
            var pdf_html =(@"
                <div class='titolo - div text - center'>
                    <p class='titolo-checklist'>Riepilogo Fase Sign Out</p>
                </div>
            ");
            return GeneratePdf(pdf_html);
        }

        private byte[] GeneratePdf(string htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 6, Bottom = 6 },
                //DocumentTitle = query.nome + " " + query.cognome
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 12, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontSize = 8, Center = "Riepilogo", Line = true },
            };

            var htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(htmlToPdfDocument);
        }
    }
}
