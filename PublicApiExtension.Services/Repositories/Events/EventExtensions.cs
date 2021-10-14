using PublicApiExtension.Models.Events;
using PublicApiExtension.Storage.Entities;

namespace PublicApiExtension.Services.Repositories.Events
{
    public static class EventExtensions
    {
        public static Event ToEntity(this EventWriteModel model)
        {
            return model == null
                ? null
                : new Event
                {
                    Name = model.Name,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
        }

        public static EventReadModel ToModel(this Event entity)
        {
            return entity == null
                ? null
                : new EventReadModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    CreatedAt = entity.CreatedAt,
                    ModifiedAt = entity.ModifiedAt
                };
        }
    }
}
