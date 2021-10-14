using PublicApiExtension.Models.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Services.Services.Events
{
    public interface IEventService
    {
        Task<List<EventReadModel>> Get(EventsFilter filter, CancellationToken token);
        Task<Guid> Create(EventWriteModel model, CancellationToken token);
        Task Update(Guid id, EventWriteModel model, CancellationToken token);
        Task<EventReadModel> GetById(Guid id, CancellationToken token);
        Task DeleteById(Guid id, CancellationToken token);
    }
}
