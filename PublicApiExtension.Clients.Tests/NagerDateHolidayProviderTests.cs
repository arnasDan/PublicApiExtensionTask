using AutoFixture.Xunit2;
using Moq;
using PublicApiExtension.Clients.NagerDate;
using PublicApiExtension.Clients.NagerDate.Client;
using PublicApiExtension.Clients.NagerDate.Models;
using PublicApiExtension.Models;
using PublicApiExtension.Services.Services.PublicHolidays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PublicApiExtension.Clients.Tests
{
    public class NagerDateHolidayProviderTests
    {
        private readonly Mock<INagerDateClient> _clientMock = new();
        private readonly IHolidayProvider _holidayProvider;

        public NagerDateHolidayProviderTests()
        {
            _holidayProvider = new NagerDateHolidayProvider(_clientMock.Object);
        }

        [Theory]
        [AutoData]
        public async Task CanRetrieveHolidays(string countryCode, int year, IEnumerable<NagerDateHoliday> holidays)
        {
            _clientMock
                .Setup(c => c.GetHolidays(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(holidays.ToList()));

            var retrievedHolidays = await _holidayProvider.GetHolidays(countryCode, year, CancellationToken.None);

            Assert.NotEmpty(holidays);
            Assert.Collection(
                retrievedHolidays,
                holidays
                    .Select(h => new Action<Holiday>(holidayToCheck => Assert.Equal(h.Date, holidayToCheck.Date)))
                    .ToArray());
        }
    }
}
