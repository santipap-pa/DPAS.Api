using System.ComponentModel.DataAnnotations;

namespace DPAS.Models.Data
{
    public abstract class BaseModel
    {
        [Key]
        public Guid Id { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}