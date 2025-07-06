using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoHub.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
}
