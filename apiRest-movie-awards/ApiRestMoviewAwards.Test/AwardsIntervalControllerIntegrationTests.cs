using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiRestMovieAwards.BFF;
using ApiRestMoviewAwards.Test.WebApplicationFactory;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ApiRestMoviewAwards.Test
{
	public class AwardsIntervalControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly HttpClient _client;

		public AwardsIntervalControllerIntegrationTests(CustomWebApplicationFactory factory)
		{
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task GetAll_ShouldReturnOkAndData()
		{
			// Act
			var response = await _client.GetAsync("/api/awards-interval");

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			var responseBody = await response.Content.ReadAsStringAsync();
			var awardsInterval = JsonSerializer.Deserialize<AwardsIntervalDTO>(responseBody, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			Assert.NotNull(awardsInterval);
			Assert.NotEmpty(awardsInterval.Min);
			Assert.NotEmpty(awardsInterval.Max);
		}
	}
}
