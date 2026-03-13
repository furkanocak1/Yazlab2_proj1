using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Dispatcher;
using System.Net;
using System.Net.Http.Json;

namespace Dispatcher.Tests
{
    public class HealthCheckTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public HealthCheckTest(WebApplicationFactory<Program> factory) => _factory = factory;

        [Fact]
        public async Task User_Registration_Should_Return_201_Created()
        {
            var client = _factory.CreateClient();
            var testUser = new { Username = "yazlab_deneme", Password = "123" };

            var response = await client.PostAsJsonAsync("/api/gateway/auth/register", testUser);

            // TDD: Şu an kod yazılı olmadığı için Fail (Kırmızı) verecektir.
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}