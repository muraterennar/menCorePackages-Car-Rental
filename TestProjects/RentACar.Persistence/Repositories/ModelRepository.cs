using MenCore.Persistence.Repositories;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class ModelRepository : EfRepositoryBase<Model, Guid, BaseDatabaseContext>, IModelRepository
{
    public ModelRepository(BaseDatabaseContext context) : base(context)
    {
    }
}