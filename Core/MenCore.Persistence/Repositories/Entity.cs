namespace MenCore.Persistence.Repositories;

public class Entity<TId> : IEntityTimeStamps
{
    public TId Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    public Entity ()
    {

    }

    public Entity (TId id, DateTime createdDate, DateTime? updatedDate, DateTime? deletedDate)
    {
        Id = id;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        DeletedDate = deletedDate;
    }

    public Entity (TId id)
    {
        Id = id;
    }
}