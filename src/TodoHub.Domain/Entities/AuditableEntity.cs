namespace TodoHub.Domain.Entities;

public class AuditableEntity : BaseEntity
{
    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? LastUpdatedBy { get; set; }

    public DateTime LastUpdated { get; set; }
}
