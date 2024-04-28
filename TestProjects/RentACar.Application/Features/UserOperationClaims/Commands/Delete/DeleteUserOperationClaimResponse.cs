namespace RentACar.Application.Features.UserOperationClaims.Commands.Delete;

public class DeleteUserOperationClaimResponse
{
    public int Id { get; set; }
    public int OperationClaimId { get; set; }
    public int UserId { get; set; }
}