using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class EmailAuthenticatorRepository : EfRepositoryBase<EmailAuthenticator, int, BaseDatabaseContext>, IEmailAuthenticatorRepository
{
    public EmailAuthenticatorRepository(BaseDatabaseContext context) : base(context)
    {
    }
}