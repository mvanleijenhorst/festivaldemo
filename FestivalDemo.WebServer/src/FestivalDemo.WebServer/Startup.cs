using System;
using System.Reflection;
using FestivalDemo.WebServer.Authentication;
using FestivalDemo.WebServer.BackgroundServices;
using FestivalDemo.WebServer.Domain.Repository;
using FestivalDemo.WebServer.Domain.Services;
using FestivalDemo.WebServer.DomainService.Adapters;
using FestivalDemo.WebServer.DomainService.Commands;
using FestivalDemo.WebServer.ExceptionFilters;
using FestivalDemo.WebServer.Infrastructure.InMemory;
using FestivalDemo.WebServer.Infrastructure.WebSockets;
using FestivalDemo.WebServer.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FestivalDemo.WebServer
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
            services.AddSingleton<WebSocketClient>();
            services.AddSingleton<IServiceBusAdapter, InMemoryServiceBus>();
            services.AddSingleton<IFestivalRepository, FestivalRepository>();

            services.AddTransient<IFestivalService, FestivalService>();
            services.AddTransient<IGuestService, GuestService>();

            services.AddTransient<IFestivalCommandHandler, FestivalCommandHandler>();
            services.AddTransient<IGuestCommandHandler, GuestCommandHandler>();
            services.AddTransient<IFestivalCommandDispatcher, FestivalCommandDispatcher>();
            services.AddTransient<IGuestCommandDispatcher, GuestCommandDispatcher>();

            services.AddHostedService<OutgoingBackgroundService>();
            services.AddHostedService<IncomingBackgroundService>();

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddTransient<IUserService, UserService>();

            services.AddControllers(o => o.Filters.Add(new HttpResponseExceptionFilter()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Festival Demo API", Version = "v1" });
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            };
            app.UseWebSockets(webSocketOptions);
            app.UseMiddleware<WebSocketMiddleware>();

            if (env.IsDevelopment())
            {


                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Festival Demo Api");

                });
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
