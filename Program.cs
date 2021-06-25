using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckList
{
    public class Program
    {

        //creare il modello entity framework 
        // Scaffold-DbContext "Server=localhost;Database=checklist;user=sa;password=root;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -UseDatabaseNames -NoPluralize -force
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
