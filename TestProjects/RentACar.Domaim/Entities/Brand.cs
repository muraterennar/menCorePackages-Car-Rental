using MenCore.Persistence.Repositories;

namespace RentACar.Domaim.Entities;

public class Brand : Entity<Guid>
{
    public Brand()
    {
        Models = new HashSet<Model>();
    }

    public Brand(string name) : this()
    {
        Id = Guid.NewGuid();
        BrandName = name;
    }

    public Brand(Guid id, string name, DateTime createdDate, DateTime updatedDate, DateTime deletedDate) : this()
    {
        Id = id;
        BrandName = name;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        DeletedDate = deletedDate;
    }

    public string BrandName { get; set; }

    public virtual ICollection<Model> Models { get; set; }
}