using DPAS.Api.Enums;
using DPAS.Models.Data;

namespace DPAS.Api.Models.Data
{
    public class AlertModel : BaseModel
    {
        public required Guid RegionId { get; set; }
        public required DisasterTypeEnum DisasterType { get; set; }
        public required int ThresholdScore { get; set; }
        public virtual RegionModel Region { get; set; } = null!;
    }
} 