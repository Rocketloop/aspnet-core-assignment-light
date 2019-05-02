using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore_assignment.Models;
using aspnetcore_assignment.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace aspnetcore_assignment {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("aspnetcore-assignment"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.CreateScope()) {
                var context = serviceScope.ServiceProvider.GetService<ApiDbContext>();
                AddTestData(context);
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        /// <summary>
        /// Seed some test data - NOTE NONE OF THE CHANGES WILL BE PERSISTED!
        /// </summary>
        /// <param name="context">Context.</param>
        private static void AddTestData(ApiDbContext context) {
           for(int i=0; i<100; i++) {
                Todo t = new Todo {
                    Done = i % 2 == 0,
                    Title = $"Todo #${i}"
                };
                context.Todos.Add(t);
            }
            context.SaveChanges();
        }

    }
}
