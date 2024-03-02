using MenCore.Persistence.Repositories;

namespace RentACar.Domaim.Entities;

public class Brand : Entity<Guid>
{
    public string Name { get; set; }
    public Brand ()
    {

    }

    public Brand (string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public Brand (Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}