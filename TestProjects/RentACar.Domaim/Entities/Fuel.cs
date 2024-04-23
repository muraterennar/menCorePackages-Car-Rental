using MenCore.Persistence.Repositories;

namespace RentACar.Domaim.Entities;

public class Fuel : Entity<Guid>
{
    public Fuel()
    {
        Models = new HashSet<Model>();
    }

    public Fuel(string fuelName) : this()
    {
        Id = Guid.NewGuid();
        FuelName = fuelName;
    }

    public Fuel(Guid id, string fuelName, DateTime createdDate, DateTime updatedDate, DateTime deletedDate) : this()
    {
        Id = id;
        FuelName = fuelName;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        DeletedDate = deletedDate;
    }

    public string FuelName { get; set; }

    public virtual ICollection<Model> Models { get; set; }
}