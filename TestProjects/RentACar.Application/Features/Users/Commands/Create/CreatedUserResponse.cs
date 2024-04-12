namespace RentACar.Application.Features.Users.Commands.Create;

public class CreatedUserResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }

    public CreatedUserResponse()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        Status = string.Empty;
    }

    public CreatedUserResponse(int id, string firstName, string lastName, string email, string status)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Status = status;
    }
}