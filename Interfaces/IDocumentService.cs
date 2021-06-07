using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckList.Models;
using Newtonsoft.Json.Linq;

namespace CheckList.Interfaces
{
    public interface IDocumentService
    {
        byte[] GeneratePdfFromString(paziente data, JArray checklist);

        //byte[] GeneratePdfFromRazorView();
    }
}
