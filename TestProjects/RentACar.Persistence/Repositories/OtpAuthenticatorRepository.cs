using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class OtpAuthenticatorRepository : EfRepositoryBase<OtpAuthenticator, int, BaseDatabaseContext>, IOtpAuthenticatorRepository
{
    public OtpAuthenticatorRepository(BaseDatabaseContext context) : base(context)
    {
    }
}