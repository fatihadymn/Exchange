using Exchange.Core.Services;
using Exchange.Infrastructure.Attributes;
using Exchange.Infrastructure.Middlewares;
using Exchange.Infrastructure.Swagger;
using Exchange.Items.Exceptions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace Exchange.Infrastructure
{
    public static class Extensions
    {
        public static IMvcBuilder AddControllersCustom(this IServiceCollection services)
        {
            return services.AddControllers(opt =>
            {
                opt.Filters.Add<ModelValidatorAttribute>();
            });
        }

        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services, Type implementationType)
        {
            services.AddTransient<IServiceBase, ServiceBase>();

            var repositories = implementationType.Assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IServiceBase))) && !t.IsInterface);

            foreach (var repository in repositories)
            {
                var repositoryType = repository.GetInterfaces().Where(x => x.Name.Contains(repository.Name)).FirstOrDefault();

                if (repositoryType == null)
                {
                    throw new BusinessException($"Base class did not found for type: {repository.FullName}", 500);
                }

                services.AddTransient(repositoryType, repository);
            }

            return services;
        }

        public static void AddValidators(this IMvcBuilder builder, Type validatorsAssemblyType)
        {
            builder.AddFluentValidation(x =>
            {
                x.RegisterValidatorsFromAssembly(validatorsAssemblyType.Assembly);
                x.LocalizationEnabled = true;
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {

                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var firstMessage = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                                                                .SelectMany(v => v.Errors)
                                                                .Select(v => new
                                                                {
                                                                    Message = (!string.IsNullOrEmpty(v.ErrorMessage) || v.Exception == null) ? v.ErrorMessage : v.Exception.Message
                                                                })
                                                                .FirstOrDefault();


                    return new BadRequestObjectResult(new
                    {
                        firstMessage.Message
                    });
                };
            });
        }

        public static IServiceCollection AddSwaggerCustom(this IServiceCollection services)
        {
            SwaggerOptions options;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();

                var section = configuration.GetSection("swagger");
                services.Configure<SwaggerOptions>(section);
                options = new SwaggerOptions();
                section.Bind(options);
            }

            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(options.Version, new OpenApiInfo { Title = options.Title, Version = options.Version });
            });
        }

        public static IApplicationBuilder UseSwaggerCustom(this IApplicationBuilder builder)
        {
            var options = new SwaggerOptions();

            builder.ApplicationServices
                   .GetService<IConfiguration>()
                   .GetSection("swagger").Bind(options);

            builder.UseSwagger();

            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", options.Title);
            });

            return builder;
        }
    }
}
