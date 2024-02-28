using Microsoft.OpenApi.Models;
using NewsApi.Application.Interfaces;
using NewsApi.Infrastructure.Authentication;
using NewsApi.Infrastructure.HttpClients;
using System.Text.Json.Serialization;

namespace NewsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
            builder.Services.AddScoped<INewsApiOrgHttpClient, NewsApiOrgHttpClient>();
            builder.Services.AddScoped<ApiKeyAuthFilter>();

            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x => 
            { 
                x.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "An API key to access the API",
                    Name = "X-Api-Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKeyScheme",
                });
                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                };
                var requirement = new OpenApiSecurityRequirement
                {
                    { scheme, new List<string>() }
                };
                x.AddSecurityRequirement(requirement);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
