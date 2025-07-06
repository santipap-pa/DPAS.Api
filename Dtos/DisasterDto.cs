using DPAS.Api.Enums;

namespace DPAS.Api.Dtos
{
    public class GetDisasterRiskDto
    {
        public string? RegionID { get; set; }
        public string? DisasterType { get; set; }
        public int? RiskScore { get; set; }
        public string? RiskLevel { get; set; }
        public bool? AlertTrigger { get; set; }

    }
}