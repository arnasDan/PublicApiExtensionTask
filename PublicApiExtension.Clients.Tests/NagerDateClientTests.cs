using AutoFixture.Xunit2;
using PublicApiExtension.Clients.NagerDate.Client;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PublicApiExtension.Clients.Tests
{
    public class NagerDateClientTests
    {
        private readonly INagerDateClient _client = new NagerDateClient(new System.Net.Http.HttpClient(), "https://date.nager.at/api/v3/PublicHolidays");

        [Theory]
        [InlineAutoData("AT")]
        [InlineAutoData("LT")]
        [InlineAutoData("US")]
        public async Task CanRetrieveHolidays(string countryCode, [Range(1995, 2100)] int year)
        {
            var holidays = await _client.GetHolidays(countryCode, year, CancellationToken.None);

            Assert.NotEmpty(holidays);
            Assert.All(holidays, holiday =>
            {
                Assert.Equal(countryCode, holiday.CountryCode);
                Assert.InRange(holiday.Date.Year, holiday.Date.Year - 1, holiday.Date.Year + 1);
            });
        }
    }
}
