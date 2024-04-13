using MenCore.Security.Enums;

namespace RentACar.Application.Features.Users.Commands.Delete;

public class DeletedUserResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? FullName { get; set; }
    public string Username { get; set; }
    public string? IdentityNumber { get; set; }
    public short? BirthYear { get; set; }
    public string Email { get; set; }
    public bool Status { get; set; }
    public AuthenticatorType AuthenticatorType { get; set; }
}