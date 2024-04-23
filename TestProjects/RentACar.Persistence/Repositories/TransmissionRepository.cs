using MenCore.Persistence.Repositories;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class TransmissionRepository : EfRepositoryBase<Transmission, Guid, BaseDatabaseContext>, ITransmissionRepository
{
    public TransmissionRepository(BaseDatabaseContext context) : base(context)
    {
    }
}