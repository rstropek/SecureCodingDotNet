using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace LoggingSerilog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        // Respond with error information according to https://tools.ietf.org/html/rfc7807.
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/problem+json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            type = "https://netsecdev.demo.com/errors/internal-server-error",
                            title = "Internal Server Error",
                            status = 500,
                            detail = $"If this error continues to occur, please contact support (request id: {context.TraceIdentifier})",
                            traceIdentifier = context.TraceIdentifier
                        }));
                    });
                });
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
