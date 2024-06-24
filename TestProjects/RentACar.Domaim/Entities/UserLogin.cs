using MenCore.Persistence.Repositories;
using MenCore.Security.Entities;

namespace RentACar.Domaim.Entities;

public class UserLogin : Entity<int>
{
    public string LoginProvider { get; set; }
    public string ProviderKey { get; set; }
    public string ProviderDisplayName { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
}