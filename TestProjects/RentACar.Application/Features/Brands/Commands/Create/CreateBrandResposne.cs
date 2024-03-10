namespace RentACar.Application.Features.Brands.Commands.Create;

public class CreateBrandResposne
{
    public Guid Id { get; set; }
    public string BrandName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}