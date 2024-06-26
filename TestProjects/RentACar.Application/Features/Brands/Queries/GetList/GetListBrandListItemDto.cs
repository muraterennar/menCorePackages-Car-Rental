﻿namespace RentACar.Application.Features.Brands.Queries.GetList;

public class GetListBrandListItemDto
{
    public Guid Id { get; set; }
    public string BrandName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}