using System.Threading.Tasks;
  using Xunit;
  using Microsoft.AspNetCore.Mvc.Testing;

  public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
  {
      private readonly WebApplicationFactory<Program> _factory;
      public ApiTests(WebApplicationFactory<Program> factory) => _factory = factory;

      [Fact]
      public async Task Get_HomePage_ReturnsSuccess()
      {
          var client = _factory.CreateClient();
          var res = await client.GetAsync("/");
          res.EnsureSuccessStatusCode();
          var body = await res.Content.ReadAsStringAsync();
          Assert.Contains("Index", body); // adjust assertion to fit your page
      }
  }