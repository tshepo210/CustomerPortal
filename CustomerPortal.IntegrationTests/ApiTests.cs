using System.Threading.Tasks;
  using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class ApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    public ApiTests(CustomWebApplicationFactory factory) => _factory = factory;

      [Fact]
      public async Task Get_HomePage_ReturnsSuccess()
      {
          var client = _factory.CreateClient();
          var res = await client.GetAsync("/");
          res.EnsureSuccessStatusCode();
            var body = await res.Content.ReadAsStringAsync();
            // The app redirects unauthenticated users to the login page, accept either the Index page or the Login page
            Assert.True(body.Contains("Index") || body.Contains("Login"));
      }
  }