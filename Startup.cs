using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TokenAccessor
{
    public class Startup
    {
        private readonly TokenCredential _credentials = new DefaultAzureCredential();

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/getToken", async context =>
                {
                    context.Request.ContentType = "application/json";
                    TokenRequest request = null;
                    if (context.Request.ContentLength > 0)
                    {
                        request = await context.Request.ReadFromJsonAsync<TokenRequest>(context.RequestAborted);
                    }

                    string[] scopes = request?.Scopes ?? new[] { "https://management.azure.com/.default" };

                    TokenRequestContext tokenRequestContext = new(scopes);
                    AccessToken token = await _credentials.GetTokenAsync(tokenRequestContext, context.RequestAborted);
                    await context.Response.WriteAsJsonAsync(token, context.RequestAborted);
                });
            });
        }

        private record TokenRequest(string[] Scopes);
    }
}
