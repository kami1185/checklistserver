using CheckList.Interfaces;
using CheckList.Services;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckList
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
//////////************* Creiamo la regola per abilitare CORS **************///////////////////
            services.AddTransient<IPazienteCheckListService, PazienteCheckListService>();
            services.AddSingleton<IConfiguration>(Configuration);
/// ///////////////////////////////////////////////////////////////////////////////////////////

            services.AddControllersWithViews();

//////////************* Creiamo la regola per abilitare CORS **************///////////////////
            services.AddCors(options =>
            {
                // questo nome glielo diamo alla regola: MyPolicyRule
                options.AddPolicy(name: "MyPolicyRule",
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
            });

            // ho aggiunto il service x creare il report del riepilogo in pdf
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            // dipendeza per creare il file pdf/ classe interface e la classe in cui si crea il pdf
            services.AddTransient<IDocumentService, DocumentService>();

            services.AddMvc().AddNewtonsoftJson();
        }
/// ///////////////////////////////////////////////////////////////////////////////////////////


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // aggiungiamo la regola CORS create precedentemente 
            app.UseCors("MyPolicyRule");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
