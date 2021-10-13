using PublicApiExtension.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Services.Services.Events
{
    public interface IEventService
    {
        Task<IEnumerable<EventReadModel>> Get(EventsFilter filter, CancellationToken token);
        Task<Guid> Create(EventWriteModel model, CancellationToken token);
        Task Update(Guid id, EventWriteModel model, CancellationToken token);
        Task<EventReadModel> GetById(Guid id, CancellationToken token);
        Task DeleteById(Guid id, CancellationToken token);
    }
}
