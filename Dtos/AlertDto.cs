using DPAS.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace DPAS.Api.Dtos
{
    public class CreateAlertDto
    {
        [Required]
        public required string RegionID { get; set; }
        
        [Required]
        public required DisasterTypeEnum DisasterType { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Threshold score must be a non-negative integer")]
        public required int ThresholdScore { get; set; }
    }

    public class GetAlertDto
    {
        public Guid Id { get; set; }
        public required string RegionID { get; set; }
        public required DisasterTypeEnum DisasterType { get; set; }
        public required int ThresholdScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UpdateAlertDto
    {
        public DisasterTypeEnum? DisasterType { get; set; }
        public int? ThresholdScore { get; set; }
    }
}