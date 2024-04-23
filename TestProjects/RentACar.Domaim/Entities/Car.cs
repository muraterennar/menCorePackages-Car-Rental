using MenCore.Persistence.Repositories;
using RentACar.Domaim.Enums;

namespace RentACar.Domaim.Entities;

public class Car : Entity<Guid>
{
    public Car()
    {
    }

    public Car(Guid modelId, int kilometer, short modelYear, string plate, CarState carState) : this()
    {
        ModelId = modelId;
        Kilometer = kilometer;
        ModelYear = modelYear;
        Plate = plate;
        CarState = carState;
    }

    public Car(Guid modelId, int kilometer, short modelYear, string plate, CarState carState, DateTime createdDate,
        DateTime updatedDate, DateTime deletedDate) : this()
    {
        ModelId = modelId;
        Kilometer = kilometer;
        ModelYear = modelYear;
        Plate = plate;
        CarState = carState;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        DeletedDate = deletedDate;
    }

    public Guid ModelId { get; set; }
    public int Kilometer { get; set; }
    public short ModelYear { get; set; }
    public string Plate { get; set; }
    public CarState CarState { get; set; }

    public virtual Model Model { get; set; }
}