using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System;
using Microsoft.Extensions.DependencyInjection;
using DfT.DTRO.DAL;
using Microsoft.EntityFrameworkCore;

namespace DfT.DTRO.IntegrationTests.DtroUsersIntegrationTests
{
    public class DtroUsersIntegrationTests : IClassFixture<WebApplicationFactory<DfT.DTRO.Startup>>
    {
        private readonly WebApplicationFactory<DfT.DTRO.Startup> _factory;
        private readonly string _url;

        public DtroUsersIntegrationTests(WebApplicationFactory<DfT.DTRO.Startup> factory)
        {
            _factory = factory;
            _url = "/dtroUsers";

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DtroContext>();
                dbContext.Database.Migrate();
            }
        }

        [Fact]
        public async Task Test_No_X_App_Id_Returns_500()
        {
            var client = _factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, _url);
            var response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains("Middleware, access denied: x-App-Id", responseBody);
        }
    }
}
