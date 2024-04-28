namespace RentACar.Application.Features.UserOperationClaims.Commands.Create;

public class CreateUserOperationClaimResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }
    public string? OperationClaimName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}