using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetWebApiGateway
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {





            var secret = "Thisismytestprivatekey";
            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });






            services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
            services.AddSwaggerForOcelot(Configuration);
            /*
                        services.AddSwaggerForOcelot(Configuration,
                        (o) =>
                        {
                           // o.GenerateDocsForGatewayItSelf = true;
                        });
            */






            /*
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            */

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // app.UsePathBase("/gateway");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            //app.UseSwagger();


            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome To Api Gateway of !!");
                });
            });
            // app.UseSwagger().UseOcelot().Wait();
            // app.UseSwagger();

            /*
            app.UseSwaggerForOcelotUI(opt =>
            {
                //opt.PathToSwaggerGenerator = "/swagger/docs";
                //opt.
            })
            .UseOcelot().Wait();
            

            app.UseSwagger();
            */
            // app.UseStaticFiles();
            /*  app.UseSwaggerForOcelotUI(opt =>
              {
                  opt.DownstreamSwaggerEndPointBasePath = "/gateway/swagger/docs";
                  opt.PathToSwaggerGenerator = "/swagger/docs";
              })
              .UseOcelot()
              .Wait();

              app.UseSwagger();

             */

            app.UseSwaggerForOcelotUI()
         .UseOcelot()
         .Wait();

        }
    }
}
