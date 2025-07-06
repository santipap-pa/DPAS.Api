using DPAS.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace DPAS.Api.Dtos
{
    public class CreateAlertDto
    {
        public required string RegionID { get; set; }
        public required DisasterTypeEnum DisasterType { get; set; }
        public required int ThresholdScore { get; set; }
    }

    public class GetAlertDto
    {
        public required string RegionID { get; set; }
        public required string DisasterType { get; set; }
        public required int ThresholdScore { get; set; }
    }

    public class UpdateAlertDto
    {
        public required DisasterTypeEnum DisasterType { get; set; }
        public required int ThresholdScore { get; set; }
    }
}