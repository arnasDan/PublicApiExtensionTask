using Microsoft.EntityFrameworkCore;
using PublicApiExtension.Models.Events;
using PublicApiExtension.Storage;
using PublicApiExtension.Storage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Services.Repositories.Events
{
    public class EventRepository : IEventRepository
    {
        private readonly SchedulerDatabaseContext _dbContext;

        public EventRepository(SchedulerDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<EventReadModel>> GetEvents(EventsFilter filter, CancellationToken token)
        {
            var query = _dbContext.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(e => e.Name == filter.Name);

            if (filter.StartsBefore.HasValue)
                query = query.Where(e => e.StartDate <= filter.StartsBefore);      
            
            if (filter.EndsAfter.HasValue)
                query = query.Where(e => e.EndDate >= filter.EndsAfter);

            return (await query.ToListAsync(token))
                .Select(e => e.ToModel());
        }

        public async Task<Guid> AddEvent(EventWriteModel model, CancellationToken token)
        {
            var entity = model.ToEntity();

            _dbContext.Events.Add(entity);

            await _dbContext.SaveChangesAsync(token);

            return entity.Id;
        }

        private Task<Event> GetEntityById(Guid id, CancellationToken token) =>
            _dbContext.Events.FirstOrDefaultAsync(x => x.Id == id, token);

        public async Task<EventReadModel> GetById(Guid id, CancellationToken token)
        {
            var entity = await GetEntityById(id, token);

            return entity.ToModel();
        }

        public async Task<bool> DeleteById(Guid id, CancellationToken token)
        {
            var entity = await GetEntityById(id, token);

            if (entity == null)
                return false;

            _dbContext.Events.Remove(entity);

            await _dbContext.SaveChangesAsync(token);

            return true;
        }

        public async Task<bool> UpdateEvent(Guid id, EventWriteModel model, CancellationToken token)
        {
            var entityToUpdate = await GetEntityById(id, token);

            if (entityToUpdate == null)
                return false;

            entityToUpdate.Name = model.Name;
            entityToUpdate.StartDate = model.StartDate;
            entityToUpdate.EndDate = model.EndDate;

            await _dbContext.SaveChangesAsync(token);

            return true;
        }
    }
}
