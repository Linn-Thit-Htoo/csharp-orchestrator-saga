using csharp_orchestrator_saga.OrderOrchestrator.Configurations;
using csharp_orchestrator_saga.OrderOrchestrator.Entities;
using csharp_orchestrator_saga.OrderOrchestrator.Persistence.Base;
using csharp_orchestrator_saga.OrderOrchestrator.Services.OrderDetail;
using csharp_orchestrator_saga.OrderOrchestrator.Services.Stock;
using csharp_orchestrator_saga.OrderOrchestrator.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace csharp_orchestrator_saga.OrderOrchestrator.Dependencies
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder
                .Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile(
                    $"appsettings.{builder.Environment.EnvironmentName}.json",
                    optional: false,
                    reloadOnChange: true
                )
                .AddEnvironmentVariables();

            builder.Services.Configure<AppSetting>(builder.Configuration);

            builder
                .Services.AddControllers()
                .ConfigureApiBehaviorOptions(opt =>
                {
                    opt.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context
                            .ModelState.Where(ms => ms.Value!.Errors.Any())
                            .SelectMany(ms => ms.Value!.Errors.Select(e => e.ErrorMessage))
                            .ToList();

                        var errorMessage = string.Join("; ", errors);
                        var result = Result<object>.Fail(errorMessage);

                        return new OkObjectResult(result);
                    };
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            builder.Services.AddDbContext<AppDbContext>(
                (serviceProvider, opt) =>
                {
                    opt.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
                    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
            );

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
            });

            builder.Services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
            builder.Services.AddScoped<IStockService, StockService>();
            builder.Services.AddHealthChecks();

            return services;
        }
    }
}
