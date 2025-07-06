using System.ComponentModel.DataAnnotations;
using DPAS.Models.Data;

namespace DPAS.Api.Models.Data
{
    public class LoggingModel : BaseModel
    {
        public string? LogType { get; set; }
        public string? Action { get; set; }
        public string? Description { get; set; } 
        public string? Endpoint { get; set; }
        public string? HttpMethod { get; set; }
        public int? ResponseStatusCode { get; set; }
        public long? ResponseTime { get; set; }
        public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
    }
}