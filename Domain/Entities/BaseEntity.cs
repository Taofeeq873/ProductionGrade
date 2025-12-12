using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public bool IsDeleted { get; set; } = false;


    public static bool operator ==(BaseEntity left, BaseEntity right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);

        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity left, BaseEntity right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        var other = (BaseEntity)obj;
        return Id == other.Id;
    }
}