using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsApi.Application.DTO.NewsApiOrg;
using NewsApi.Application.Enums.NewsApiOrg;
using NewsApi.Application.Interfaces;
using NewsApi.Controllers;
using NewsApi.Infrastructure.HttpClients;
using Xunit;

namespace NewsApi.Tests
{
    public class NewsControllerTests
    {
        private readonly IMediator _mediator;

        public NewsControllerTests()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<INewsApiOrgHttpClient, NewsApiOrgHttpClient>();
            var serviceProvider = services.BuildServiceProvider();
            _mediator = serviceProvider.GetService<IMediator>() ?? throw new ArgumentNullException("Could not get IMediator service");
        }

        [Fact]
        public async Task GetRecentNewsByTitleKeyword_ShouldReturnHTTP200_AndStatusPropertyShouldBeNotNullAsync()
        {
            //Arrange
            var controller = new NewsController(_mediator);

            //Act
            var result = await controller.GetRecentNewsByTitleKeywordAsync("Test", Language.EN);
            var okResult = result.Result as OkObjectResult;
            var value = okResult?.Value as ArticlesResultDto;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(200, okResult?.StatusCode);
            Assert.NotNull(value?.Status);
            Assert.Equal("ok", value?.Status);
        }
    }
}
