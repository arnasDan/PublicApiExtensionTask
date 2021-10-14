using AutoFixture.Xunit2;
using Moq;
using PublicApiExtension.Models;
using PublicApiExtension.Models.Events;
using PublicApiExtension.Services.Exceptions;
using PublicApiExtension.Services.Repositories.Events;
using PublicApiExtension.Services.Services.Events;
using PublicApiExtension.Services.Services.PublicHolidays;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PublicApiExtension.Services.Tests
{
    public class EventServiceTests
    {
        private readonly Mock<IPublicHolidayService> _publicHolidayMock = new();
        private readonly Mock<IEventRepository> _eventRepositoryMock = new();
        private readonly IEventService _eventService;

        public EventServiceTests()
        {
            _eventService = new EventService(_eventRepositoryMock.Object, _publicHolidayMock.Object);
        }

        [Theory]
        [AutoData]
        public async Task CannotCreateEventWithInvalidRange(EventWriteModel @event)
        {
            @event.EndDate = @event.StartDate.AddDays(-1);

            await Assert.ThrowsAsync<DomainException>(() => _eventService.Create(@event, CancellationToken.None));
        }

        [Theory]
        [AutoData]
        public async Task CannotCreateEventWithHolidayInRange(EventWriteModel @event, Holiday holiday)
        {
            @event.EndDate = @event.StartDate.AddDays(10);
            _publicHolidayMock
                .Setup(h => h.GetHolidaysInRange(@event.StartDate, @event.EndDate, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<Holiday> { holiday }));

            await Assert.ThrowsAsync<DomainException>(() => _eventService.Create(@event, CancellationToken.None));
        }

        [Theory]
        [AutoData]
        public async Task CanCreateValidEvent(EventWriteModel @event)
        {
            @event.EndDate = @event.StartDate.AddDays(10);
            _publicHolidayMock
                .Setup(h => h.GetHolidaysInRange(@event.StartDate, @event.EndDate, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<Holiday>()));

            await _eventService.Create(@event, CancellationToken.None);

            _eventRepositoryMock.Verify(p => p.AddEvent(@event, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Theory]
        [AutoData]
        public async Task CanGetEvents(IEnumerable<EventReadModel> events)
        {
            _eventRepositoryMock
                .Setup(r => r.GetEvents(It.IsAny<EventsFilter>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(events));

            var savedEvents = await _eventService.Get(new EventsFilter(), CancellationToken.None);

            Assert.Equal(events, savedEvents);
        }

        [Theory]
        [AutoData]
        public async Task CannotUpdateNonExistantEvent(EventWriteModel @event)
        {
            @event.EndDate = @event.StartDate.AddDays(10);
            _publicHolidayMock
                .Setup(h => h.GetHolidaysInRange(@event.StartDate, @event.EndDate, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<Holiday>()));

            var exception = await Assert.ThrowsAsync<DomainException>(() => _eventService.Update(Guid.Empty, @event, CancellationToken.None));
            Assert.Equal(DomainErrorCode.NotFound, exception.ErrorCode);
        }

        [Theory]
        [AutoData]
        public async Task CannotDeleteNonExistantEvent(EventWriteModel @event)
        {
            @event.EndDate = @event.StartDate.AddDays(10);
            _publicHolidayMock
                .Setup(h => h.GetHolidaysInRange(@event.StartDate, @event.EndDate, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<Holiday>()));

            var exception = await Assert.ThrowsAsync<DomainException>(() => _eventService.DeleteById(Guid.Empty, CancellationToken.None));
            Assert.Equal(DomainErrorCode.NotFound, exception.ErrorCode);
        }
    }
}
