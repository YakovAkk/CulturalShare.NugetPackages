namespace MX.Database.Entities;

public class BaseImmutableEntity<TId> : BaseImmutableEntityWithoutId where TId : struct
{
    public TId Id { get; private set; }
}
