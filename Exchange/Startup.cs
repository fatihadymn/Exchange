using Exchange.Core;
using Exchange.Core.Services;
using Exchange.Data;
using Exchange.Infrastructure;
using Exchange.Items;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Exchange
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersCustom()
                    .AddValidators(typeof(ItemIdentifier));

            services.AddAutoMapper(Assembly.GetAssembly(typeof(ItemIdentifier)));

            services.AddServices(typeof(CoreIdentifier));

            services.AddSwaggerCustom();

            ConfigureDatabase(services);

            if (Environment.EnvironmentName == "Docker")
            {
                services.AddHangfire(x =>
                {
                    x.UsePostgreSqlStorage(Configuration.GetConnectionString("PgSql"), new PostgreSqlStorageOptions()
                    {
                        SchemaName = "scheduler"
                    });
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseErrorHandler();

            app.UseRouting();

            app.UseEndpoints(ep =>
            {
                ep.MapControllers();
            });

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerCustom();

            if (Environment.EnvironmentName == "Docker")
            {
                Initializer.InitializeDatabase(app);

                Initializer.InitializeCurrencies(app);

                app.UseHangfireDashboard();

                app.UseHangfireServer();

                ConfigureHangfireTasks();
            }
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PgSql"), x =>
            {
                x.MigrationsAssembly(typeof(DataIdentifier).Namespace);
                x.MigrationsHistoryTable("ef_migrations", "public");
            }));

            services.AddScoped(typeof(DbContext), typeof(ApplicationContext));
        }

        private void ConfigureHangfireTasks()
        {
            RecurringJob.AddOrUpdate<ICurrencyService>("SaveDailyRatesAsync", service => service.SaveDailyRatesAsync(), Configuration["ProcessTimes:DailyJob"]);
        }
    }
}
