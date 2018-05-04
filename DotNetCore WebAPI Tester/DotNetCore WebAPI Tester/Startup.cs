using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DotNetCore_WebAPI_Tester.Models;
using Microsoft.EntityFrameworkCore;
using DotNetCore_WebAPI_Tester.DatabaseContext;
using DotNetCore_WebAPI_Tester.Interfaces;
using DotNetCore_WebAPI_Tester.DataAccessServices;

namespace DotNetCore_WebAPI_Tester
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
            //In order to inject the database context into the controller, we need to register it with the dependency injection container. 
            //services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddDbContext<PersonalExpenseContext>(opt => opt.UseSqlServer("Server=localhost;Database=ExpensesDB;Trusted_Connection=True"));

            services.AddScoped<IExpenseGroupService, ExpenseGroupService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
