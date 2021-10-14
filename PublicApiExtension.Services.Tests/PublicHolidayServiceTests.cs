using PublicApiExtension.Services.Services.PublicHolidays;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using PublicApiExtension.Models;
using System.Threading;
using Xunit;
using AutoFixture.Xunit2;
using System.ComponentModel.DataAnnotations;

namespace PublicApiExtension.Services.Tests
{
    public class PublicHolidayServiceTests
    {
        private readonly Mock<IHolidayProvider> _holidayProviderMock = new();
        private readonly IPublicHolidayService _service;

        public PublicHolidayServiceTests()
        {
            var fixture = new Fixture();
            _service = new PublicHolidayService(_holidayProviderMock.Object, fixture.Create<string>());
        }

        [Theory]
        [AutoData]
        public async Task CanRetrieveHolidayDays([Range(1995, 2100)] int year, IEnumerable<Holiday> holidays)
        {
            holidays = SetupMockForCurrentYear(year, holidays);

            var result = await _service.GetHolidaysInRange(new DateTime(year, 1, 1), new DateTime(year, 12, 31), CancellationToken.None);
            Assert.Collection(result, holidays
                .Select(h => new Action<Holiday>(holidayToCheck => Assert.Equal(h.Date, holidayToCheck.Date)))
                .ToArray());
        }

        [Theory]
        [AutoData]
        public async Task HolidayDaysAreCached([Range(1995, 2100)] int year, IEnumerable<Holiday> holidays)
        {
            SetupMockForCurrentYear(year, holidays);

            await _service.GetHolidaysInRange(new DateTime(year, 1, 1), new DateTime(year, 12, 31), CancellationToken.None);
            await _service.GetHolidaysInRange(new DateTime(year, 1, 1), new DateTime(year, 12, 31), CancellationToken.None);

            _holidayProviderMock.Verify(p => p.GetHolidays(It.IsAny<string>(), year, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Theory]
        [AutoData]
        public async Task NoHolidayDaysReturnedForWrongYear([Range(1995, 2100)] int year, IEnumerable<Holiday> holidays)
        {
            SetupMockForCurrentYear(year, holidays);

            year++;

            var result = await _service.GetHolidaysInRange(new DateTime(year, 1, 1), new DateTime(year, 12, 31), CancellationToken.None);
            Assert.Empty(result);
        }

        private IEnumerable<Holiday> SetupMockForCurrentYear(int year, IEnumerable<Holiday> holidays)
        {
            holidays = holidays.Select(h => h with { Date = new DateTime(year, h.Date.Month, h.Date.Day) });
            _holidayProviderMock
                .Setup(p => p.GetHolidays(It.IsAny<string>(), year, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(holidays));

            return holidays;
        }
    }
}
