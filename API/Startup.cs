using API.Constants;
using APP.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace API
{
    public class Startup
    {
        private string APIBaseURL
        {
            get
            {
                return Environment.GetEnvironmentVariable(APPConstants.API_BASE_URL);
            }
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<Domain>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(c => c.Id);
                cm.MapProperty(c => c.ListingID);
                cm.MapProperty(c => c.Company);
                cm.MapProperty(c => c.Image_List);
                cm.SetIgnoreExtraElements(true);
            });

            services.AddCors();
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddControllers();
            services.AddMvc()
          .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
          .AddNewtonsoftJson();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            ConfigureSwagger(services);
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            services.AddHttpClient(APPConstants.API, client =>
            {
                var ApiURI = APIBaseURL;
                client.BaseAddress = new Uri(ApiURI);
                client.DefaultRequestHeaders.Add(APPConstants.ACCEPT, APPConstants.CONTENT_TYPE_JSON);
            });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(APPConstants.Swagger.VERSION, new OpenApiInfo
                {
                    Version = APPConstants.Swagger.VERSION,
                    Title = APPConstants.Swagger.TITLE,
                    Description = APPConstants.Swagger.DESCRIPTION,
                    TermsOfService = new Uri(APPConstants.Swagger.TERM_SERVICE_URL),
                    Contact = new OpenApiContact
                    {
                        Name = APPConstants.Swagger.SUPPORT,
                        Email = APPConstants.Swagger.EMAIL,
                        Url = new Uri(APPConstants.Swagger.CONTACT_URL),
                    },
                    License = new OpenApiLicense
                    {
                        Name = APPConstants.Swagger.POLICY,
                        Url = new Uri(APPConstants.Swagger.LICENSE_URL)
                    }
                });


                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.DescribeAllParametersInCamelCase();
                c.CustomSchemaIds((type) => type.FullName);
                c.EnableAnnotations(true, true);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                var basePath = $"/";
                app.UseSwagger(c =>
                {
                    c.SerializeAsV2 = true;
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" } };
                    });
                });

                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint(APPConstants.Swagger.URL_JSON, APPConstants.Swagger.NAME);
#if (DEBUG)
                    c.SupportedSubmitMethods(new Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod[] { SubmitMethod.Get, SubmitMethod.Post });
#else
                    c.SupportedSubmitMethods(new Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod[] { });
#endif
                    c.DocExpansion(DocExpansion.None);
                    c.EnableDeepLinking();
                    c.ShowExtensions();
                    c.ShowCommonExtensions();
                    c.EnableValidator();
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseSerilogRequestLogging();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: APPConstants.DEFAULT,
                    pattern: APPConstants.CONTROLLER_ROUTE);
            });
        }
    }
}
