using AspNetCoreWebApiCustomTypesSample.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace AspNetCoreWebApiCustomTypesSample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateBindingProvider());
            })
            //.AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new DateNewtonsoftJsonConverter()));
           .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateJsonConverter()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.MapType<Date>(() => new OpenApiSchema { Type = "string", Format = "date", 
                    Example = new CustomOpenApiDateTime(new DateTime(2020, 1, 1)) });

                // Set the comments path for the Swagger JSON and UI.
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            TypeConverterAttribute typeConverterAttribute = new TypeConverterAttribute(typeof(CustomParameterTypeConverter));
            TypeDescriptor.AddAttributes(typeof(Date), typeConverterAttribute);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}