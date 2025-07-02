using DPAS.Api.Enums;
using DPAS.Models;

namespace DPAS.Api.Models
{
    public class AlertSettingModel : BaseModel
    {
        public required Guid RegionId { get; set; }
        public required DisasterTypeEnum DisasterType { get; set; }
        public required int ThresholdScore { get; set; }
        public virtual RegionModel Region { get; set; } = null!;
    }
} 