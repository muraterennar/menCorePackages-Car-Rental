using MenCore.Persistence.Repositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Services.Repositories.BrandRepositories;

public interface IBrandRepository : IAsyncRepository<Brand, Guid>, IRepository<Brand, Guid>
{

}