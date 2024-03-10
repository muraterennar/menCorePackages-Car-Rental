using MenCore.Persistence.Repositories;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class BrandRepository : EfRepositoryBase<Brand, Guid, BaseDatabaseContext>, IBrandRepository
{
    public BrandRepository (BaseDatabaseContext context) : base(context)
    {
    }
}