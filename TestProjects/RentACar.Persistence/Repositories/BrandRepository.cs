using MenCore.Persistence.Repositories;
using RentACar.Application.Services.Repositories.BrandRepositories;
using RentACar.Domaim.Entities;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class BrandRepository : EfRepositoryBase<Brand, Guid, BaseDbContext>, IBrandRepository
{
    public BrandRepository (BaseDbContext context) : base(context)
    {
    }
}