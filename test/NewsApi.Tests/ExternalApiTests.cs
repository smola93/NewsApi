using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsApi.Application.DTO.NewsApiOrg;
using NewsApi.Application.Interfaces;
using NewsApi.Infrastructure.HttpClients;
using Xunit;

namespace NewsApi.Tests
{
    public class ExternalApiTests
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpRequest<ArticlesResultDto> _newsApiOrgHttpRequest;

        public ExternalApiTests()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IHttpRequest<ArticlesResultDto>, NewsApiOrgHttpClient>();
            var serviceProvider = services.BuildServiceProvider();
            _configuration = serviceProvider.GetService<IConfiguration>() ?? 
                throw new ArgumentNullException("Could not get IConfiguration service");
            _newsApiOrgHttpRequest = serviceProvider.GetService<IHttpRequest<ArticlesResultDto>>() ?? 
                throw new ArgumentNullException("Could not get IHttpRequest service");
        }

        [Fact]
        public async Task NewsApiOrgHttpRequest_WithQueryParams_ShouldReturnOkStatusAsync()
        {
            //Arrange
            var baseUrl = _configuration.GetSection("NewsApiOrg").GetValue<string>("BaseUrl");

            //Act
            var result = await _newsApiOrgHttpRequest.MakeRequest(
                baseUrl ?? throw new ArgumentNullException("Could not retrieve newsapi.org base url from configuration"),
                "everything",
                "q=Test",
                HttpMethod.Get);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("ok", result?.Status);
        }

        [Fact]
        public async Task NewsApiOrgHttpRequest_WithoutQueryParams_ShouldReturnErrorStatusAsync()
        {
            //Arrange
            var baseUrl = _configuration.GetSection("NewsApiOrg").GetValue<string>("BaseUrl");

            //Act
            var result = await _newsApiOrgHttpRequest.MakeRequest(
                baseUrl ?? throw new ArgumentNullException("Could not retrieve newsapi.org base url from configuration"),
                "everything",
                null,
                HttpMethod.Get);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("error", result?.Status);
        }
    }
}
