using PublicApiExtension.Models.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Services.Repositories.Events
{
    public interface IEventRepository
    {
        Task<List<EventReadModel>> GetEvents(EventsFilter filter, CancellationToken token);
        Task<Guid> AddEvent(EventWriteModel model, CancellationToken token);
        Task<EventReadModel> GetById(Guid id, CancellationToken token);
        Task DeleteById(Guid id, CancellationToken token);
    }
}
