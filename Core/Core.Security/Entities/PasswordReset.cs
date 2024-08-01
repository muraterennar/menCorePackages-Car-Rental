using MenCore.Persistence.Repositories;

namespace MenCore.Security.Entities;

public class PasswordReset:Entity<int>
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public virtual User User { get; set; }
}