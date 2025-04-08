namespace MX.Database.Entities;

public class BaseEntity<TId> : BaseImmutableEntity<TId> where TId : struct
{
    public DateTime? UpdatedAt { get; set; }
}
