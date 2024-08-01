using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;

namespace RentACar.Persistence.Repositories;

public class PasswordResetRepository:EfRepositoryBase<PasswordReset,int,BaseDatabaseContext>, IPasswordResetRepository
{
    public PasswordResetRepository(BaseDatabaseContext context) : base(context)
    {
    }
}