using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Dispatcher;
using System.Net;

namespace Dispatcher.Tests
{
    public class InvalidRoutingTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public InvalidRoutingTests(WebApplicationFactory<Program> factory) => _factory = factory;

        [Fact]
        public async Task Invalid_Service_Request_Should_Return_404()
        {
            var client = _factory.CreateClient();

            // Tanımsız servis yolu
            var response = await client.GetAsync("/api/gateway/tanimsiz-servis");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}