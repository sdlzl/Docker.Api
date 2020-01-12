using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Data;
using Api.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
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
            services.AddDbContext<UserContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySqlConString"));
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=User}/{action=Get}");
                //endpoints.MapControllers();
            });

            InitialDataBase(app);
        }

        public void InitialDataBase(IApplicationBuilder app,int? retry=0)
        {
            var retryTimes = retry.Value;
            //等待Db容器启动成功
            Thread.Sleep(8000);

            using (var scope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<UserContext>();
                    context.Database.Migrate();
                    if (!context.Users.Any())
                    {
                        context.Users.Add(new User()
                        {
                            Company = "kingdee",
                            Name = "LZL",
                            Title = "2020",
                            Id = 1
                        });
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    retryTimes ++;
                    if(retryTimes<20)
                    {
                        InitialDataBase(app, retryTimes);
                    }

                }
            }
        }

    }
}
