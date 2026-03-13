using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Dispatcher;
using System.Net;

namespace Dispatcher.Tests
{
    public class SecurityTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public SecurityTests(WebApplicationFactory<Program> factory) => _factory = factory;

        [Fact]
        public async Task Accessing_Tickets_Without_Token_Should_Return_401()
        {
            var client = _factory.CreateClient();

            // Token yok, yetki yok!
            var response = await client.GetAsync("/api/gateway/tickets");

            // Richardson Olgunluk Modeli: Yetkisiz giriş 401 dönmeli.
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
