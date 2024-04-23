using MenCore.Persistence.Repositories;

namespace RentACar.Domaim.Entities;

public class Model : Entity<Guid>
{
    public Model()
    {
        Cars = new HashSet<Car>();
    }

    public Model(Guid id, Guid brandId, Guid transmissionId, string modelName, decimal dailyPrice, string imageUrl,
        Brand? brand, Transmission transmission, ICollection<Car> cars) : this()
    {
        Id = id;
        BrandId = brandId;
        TransmissionId = transmissionId;
        ModelName = modelName;
        DailyPrice = dailyPrice;
        ImageUrl = imageUrl;
        Brand = brand;
        Transmission = transmission;
        Cars = cars;
    }

    public Model(Guid id, Guid brandId, Guid transmissionId, string modelName, decimal dailyPrice, string imageUrl,
        Brand? brand, Transmission transmission, ICollection<Car> cars, DateTime createdDate, DateTime updatedDate,
        DateTime deletedDate) : this()
    {
        Id = id;
        BrandId = brandId;
        TransmissionId = transmissionId;
        ModelName = modelName;
        DailyPrice = dailyPrice;
        ImageUrl = imageUrl;
        Brand = brand;
        Transmission = transmission;
        Cars = cars;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        DeletedDate = deletedDate;
    }

    public Guid BrandId { get; set; }
    public Guid TransmissionId { get; set; }
    public Guid FuelId { get; set; }
    public string ModelName { get; set; }
    public decimal DailyPrice { get; set; }
    public string ImageUrl { get; set; }

    public virtual Brand? Brand { get; set; }
    public virtual Transmission Transmission { get; set; }
    public virtual Fuel Fuel { get; set; }

    public virtual ICollection<Car> Cars { get; set; }
}