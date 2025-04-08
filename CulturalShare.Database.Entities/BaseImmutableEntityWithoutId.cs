namespace MX.Database.Entities;

public class BaseImmutableEntityWithoutId
{
    /// <summary>
    /// Date of creation
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public void AdjustCreationDate(TimeSpan timespan)
    {
        CreatedAt = CreatedAt.Add(timespan);
    }
}
