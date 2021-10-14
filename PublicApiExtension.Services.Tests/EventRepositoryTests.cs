using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using PublicApiExtension.Models.Events;
using PublicApiExtension.Services.Repositories.Events;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PublicApiExtension.Services.Tests
{
    public class EventRepositoryTests
    {
        private readonly IEventRepository _eventRepository = new EventRepository(new Storage.SchedulerDatabaseContext(new DbContextOptionsBuilder().UseInMemoryDatabase("test_db").Options));

        [Theory]
        [AutoData]
        public async Task CanCreateAndRetrieveEvent(EventWriteModel @event)
        {
            var id = await _eventRepository.AddEvent(@event, CancellationToken.None);

            var savedEvent = await _eventRepository.GetById(id, CancellationToken.None);

            Assert.Equal(@event.Name, savedEvent.Name);
            Assert.Equal(@event.StartDate, savedEvent.StartDate);
            Assert.Equal(@event.EndDate, savedEvent.EndDate);
        }

        [Theory]
        [AutoData]
        public async Task CanCreateAndDeleteEvent(EventWriteModel @event)
        {
            var id = await _eventRepository.AddEvent(@event, CancellationToken.None);
            await _eventRepository.DeleteById(id, CancellationToken.None);

            var savedEvent = await _eventRepository.GetById(id, CancellationToken.None);

            Assert.Null(savedEvent);
        }

        [Theory]
        [AutoData]
        public async Task CanCreateAndUpdateEvent(EventWriteModel @event, EventWriteModel eventUpdate)
        {
            var id = await _eventRepository.AddEvent(@event, CancellationToken.None);
            await _eventRepository.UpdateEvent(id, eventUpdate, CancellationToken.None);

            var savedEvent = await _eventRepository.GetById(id, CancellationToken.None);

            Assert.Equal(eventUpdate.Name, savedEvent.Name);
            Assert.Equal(eventUpdate.StartDate, savedEvent.StartDate);
            Assert.Equal(eventUpdate.EndDate, savedEvent.EndDate);
        }
    }
}
