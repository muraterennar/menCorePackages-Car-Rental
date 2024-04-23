namespace RentACar.Application.Features.UserOperationClaims.Queries.GetByUserId;

public class GetByUserIdUserOperationClaimResponse
{
    public int Id { get; set; }
    public int OpeartionClaimId { get; set; }
    public int UserId { get; set; }
    public string OperationClaimName { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}