using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;

namespace RentACar.Application.Services.Repositories;

public interface IOtpAuthenticatorRepository : IAsyncRepository<OtpAuthenticator, int>,
    IRepository<OtpAuthenticator, int>
{
}