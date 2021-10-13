using PublicApiExtension.Models.Events;
using PublicApiExtension.Services.Exceptions;
using PublicApiExtension.Services.Repositories.Events;
using PublicApiExtension.Services.Services.PublicHolidays;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Services.Services.Events
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IPublicHolidayService _publicHolidayService;

        public EventService(IEventRepository eventRepository, IPublicHolidayService publicHolidayService)
        {
            _eventRepository = eventRepository;
            _publicHolidayService = publicHolidayService;
        }

        public async Task<Guid> Create(EventWriteModel model, CancellationToken token)
        {
            await ValidateRange(model.StartDate, model.EndDate, token);

            return await _eventRepository.AddEvent(model, token);
        }

        public async Task DeleteById(Guid id, CancellationToken token)
        {
            var result = await _eventRepository.DeleteById(id, token);

            if (!result)
                throw new DomainException(DomainErrorCode.NotFound, EventErrors.NotFound);
        }

        public Task<IEnumerable<EventReadModel>> Get(EventsFilter filter, CancellationToken token)
        {
            return _eventRepository.GetEvents(filter, token);
        }

        public async Task<EventReadModel> GetById(Guid id, CancellationToken token)
        {
            var @event = await _eventRepository.GetById(id, token) ?? throw new DomainException(DomainErrorCode.NotFound, EventErrors.NotFound);

            return @event;
        }

        public async Task Update(Guid id, EventWriteModel model, CancellationToken token)
        {
            var result = await _eventRepository.UpdateEvent(id, model, token);

            if (!result)
                throw new DomainException(DomainErrorCode.NotFound, EventErrors.NotFound);
        }

        private async Task ValidateRange(DateTime start, DateTime end, CancellationToken token)
        {
            if(end > start)
                throw new DomainException(DomainErrorCode.InvalidOperation, EventErrors.InvalidRange);

            var holidays = await _publicHolidayService.GetHolidaysInRange(start, end, token);
            if (holidays.Any())
                throw new DomainException(DomainErrorCode.InvalidOperation, EventErrors.RangeContainsHolidays(holidays));

        }
    }
}
