using MenCore.Persistence.Repositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Services.Repositories;

public interface IModelRepository : IAsyncRepository<Model, Guid>, IRepository<Model, Guid>
{

}
