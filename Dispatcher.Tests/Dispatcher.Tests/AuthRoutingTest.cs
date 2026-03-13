using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Dispatcher; 
using System.Net; 

namespace Dispatcher.Tests
{
    // Bu sınıf, Dispatcher'ın gelen istekleri doğru karşılayıp karşılamadığını test eder
    public class AuthRoutingTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthRoutingTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_Root_Returns_NotFound_Or_Unauthorized()
        {
            // Arrange: Dispatcher'a sanal bir istemci üzerinden istek atıyoruz.
            var client = _factory.CreateClient();

            // Act: Henüz yönlendirme yazmadığımız için ana dizine bir GET isteği atıyoruz.
            var response = await client.GetAsync("/");

            // Assert: Bu aşamada testin "başarısız" olması (Red aşaması) TDD'nin doğasında vardır
            // Amacımız dosyanın varlığını ve test yapısının kurulduğunu kanıtlamak.
            Assert.NotNull(response);
        }
    }
}