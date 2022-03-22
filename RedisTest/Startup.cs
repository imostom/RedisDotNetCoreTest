using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RedisTest.Repository;
using RedisTest.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisTest
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

            services.AddControllers();
            services.AddDistributedMemoryCache();


            //Configure other services up here
            //var multiplexer = ConnectionMultiplexer.Connect("localhost:6379");
            //services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            //var vv = Configuration["RedisCache"];
            services.AddStackExchangeRedisCache(options =>
            {
                //options.Configuration = "localhost:6379";
                options.Configuration = Configuration["RedisCache"];
            });

            services.AddDbContext<RedisTest.Data.AppContext>(options =>
            options.UseSqlServer(
            Configuration.GetConnectionString("TestConnectionString"), providerOptions => providerOptions.EnableRetryOnFailure()));

            services.AddScoped<IDapperGenericRepository, DapperGenericRepository>();
            services.AddScoped<IRedisTestRepository, RedisTestRepository>();
            services.AddScoped<IRedisTestService, RedisTestService>();
            //services.AddScoped<RedisHelper, RedisHelper>();
            services.AddScoped<CacheProvider, CacheProvider>();



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RedisTest", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RedisTest v1"));
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
