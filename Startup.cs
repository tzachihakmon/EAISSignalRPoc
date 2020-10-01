using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using EAISSignalRPoc.Helpers;
using EAISSignalRPoc.Hubs;
using EAISSignalRPoc.Controllers;

namespace EAISSignalRPoc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(new Random());
            services.AddSingleton<EngagmentReportGenerator>();
            services.AddSingleton<HubsCommon>();

            services.AddHttpContextAccessor();
            services.AddSignalR(/*c => c.*/).AddMessagePackProtocol();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();

            app.UseSignalR(routes => routes.MapHub<EaisHub>("/signalRPoc"));
            app.UseMvc();
        }
    }
}
