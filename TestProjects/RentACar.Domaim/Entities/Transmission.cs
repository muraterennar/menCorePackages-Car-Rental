using MenCore.Persistence.Repositories;

namespace RentACar.Domaim.Entities;

public class Transmission : Entity<Guid>
{
    public Transmission()
    {
        Models = new HashSet<Model>();
    }

    public Transmission(string transmissionName) : this()
    {
        Id = Guid.NewGuid();
        TransmissionName = transmissionName;
    }

    public Transmission(Guid id, string transmissionName, DateTime createdDate, DateTime updatedDate,
        DateTime deletedDate) : this()
    {
        Id = id;
        TransmissionName = transmissionName;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        DeletedDate = deletedDate;
    }

    public string TransmissionName { get; set; }

    public virtual ICollection<Model> Models { get; set; }
}