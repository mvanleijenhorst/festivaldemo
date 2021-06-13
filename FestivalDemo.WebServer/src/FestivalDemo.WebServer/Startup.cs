﻿using System;
using FestivalDemo.WebServer.BackgroundServices;
using FestivalDemo.WebServer.Domain.Repository;
using FestivalDemo.WebServer.Domain.Services;
using FestivalDemo.WebServer.DomainService.Adapters;
using FestivalDemo.WebServer.DomainService.Commands;
using FestivalDemo.WebServer.Infrastructure.InMemory;
using FestivalDemo.WebServer.Infrastructure.WebSockets;
using FestivalDemo.WebServer.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            //services.AddHostedService<IncomingBackgroundService>();
            services.AddHostedService<OutgoingBackgroundService>();
            services.AddHostedService<ListenerBackgroundService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FestivalDemo.WebServer", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FestivalDemo.WebServer v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}