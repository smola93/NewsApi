using Microsoft.AspNetCore.Mvc;
using NewsApi.Controllers;
using Xunit;

namespace NewsApi.Tests
{
    public class StatusControllerTests
    {
        [Fact]
        public void StatusEnspointShouldReturnHTTP200()
        {
            var controller = new StatusController();
            var result = controller.Get();
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}