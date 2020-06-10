using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfoAPI.Contexts;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace CityInfoAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? 
                throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //este metodo estaba vacio 
            //para ejecutar el MVC
            //services.AddMvc();

            //Si queremos cmbiar o eliminar el formato json que es el que viene por defecto
            services.AddMvc()
                .AddMvcOptions(o =>
                {
                    o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                });


            //En caso se necesite serializar el formato Json, para que los parametros comincen con letra mayuscula, Ejemplo id con este codigo ahora sera: Id Id
            //services.AddMvc()
            //    .AddJsonOptions(o =>
            //    {
            //        if (o.SerializerSettings.ContractResolver != null)
            //        {
            //            var castedResolved = o.SerializerSettings.ContractResolver
            //                                 as DefaultContractResolver;
            //            castedResolved.NamingStrategy = null;
            //        }
            //    });


            //directivas del compilador hace queomita o especifique parte del codigo, dependiendo del tipo de simbolo que se use, y lo podemos ver colocando el DEBUG en Release
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif


            //usando la 1era opcion:
            //services.AddDbContext<CityInfoContext>();


            //usando la 2da opcion:
            //usaremos una base de datos local localdb, ya que se intala automaticamente con Visual studio
            // despues de la barra inversa \ es el nombre de instancia predeterminado no puede sser diferente en su maquina.
            //en caso de no estar seguro de que no cambi podemos ver la intancia en viausl studio, en el buscador escribimos: sql server object y vemos el nombre de la instancia (localdb)\MSSQLLocalDB
            //luego esta el nombre de la base de datos CityInfoDB
            //luego debemos hacer la autenticacion y establecer como seguro con el true
            var connectionString = _configuration["connectionStrings:cityInfoDBConnectionString"];
            services.AddDbContext<CityInfoContext>(o =>
            {
                o.UseSqlServer(connectionString);
            });

            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ////if (env.IsDevelopment())
            ////{
            ////    app.UseDeveloperExceptionPage();
            ////}

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else //este trozo fue añadido y el las propiedades del proyecto CityInfoAPI, cambiamos el environment de Development a Production, guardamos y corremos la app, veremos que salta el herror pero una sola vez y no toda la inf que solo los desarrolladores debemos ver jejeje.
            {
                app.UseExceptionHandler();
            }

            //Me permite ver el mensaje del status en postman 
            //app.UseStatusCodePages();

            app.UseMvc(); //Desde este momento, el MVC middleware will handle HTTP requests.
                          //Eso significa que ahora podemos deshacerse del código del módulo anterior. 

            ////Este trozo comentado fue eliminado
            ////app.Run(async (context) =>
            //app.Run(async (context) =>
            // {
            //    //await context.Response.WriteAsync("Hello World!");
            //    throw new Exception("Example exception");
            //});

            app.UseStatusCodePages(); // el not found retorna un simple text-based message
                                      //Me permite ver el mensaje del status en postman 
        }
    }
}
