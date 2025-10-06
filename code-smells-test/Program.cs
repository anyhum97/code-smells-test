using Microsoft.EntityFrameworkCore;

using Serilog;

using code_smells_test.Data;
using code_smells_test.Repositories;
using code_smells_test.Repositories.Interfaces;
using code_smells_test.Services.Interfaces;
using code_smells_test.Services;

namespace code_smells_test
{
    public class Program
    {
        public static void Main(string[] args)
        {
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
				.CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddHttpContextAccessor();

			builder.Services.AddScoped<ITimeZoneService, TimeZoneService>();
			builder.Services.AddScoped<ITicketRepository, TicketRepository>();

			builder.Host.UseSerilog();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

			# region [Глобальная обработка исключений]

			app.UseExceptionHandler(errorApp =>
			{
			    errorApp.Run(async context =>
			    {
			        context.Response.ContentType = "application/json";
			        context.Response.StatusCode = 500;
					
			        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
			        
					if(feature != null)
			        {
			            Log.Error(feature.Error, "UnhandledException on path \"{Path}\"", feature.Path);

			            var result = System.Text.Json.JsonSerializer.Serialize(new
			            {
			                message = "Произошла ошибка на сервере. Попробуйте позже.",
							traceId = context.TraceIdentifier	// Можно использовать для связи с логами
			            });

			            await context.Response.WriteAsync(result);
			        }
			    });
			});

			#endregion

			if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
