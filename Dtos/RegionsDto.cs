using DPAS.Api.Enums;
using DPAS.Api.Models.Data;

namespace DPAS.Api.Dtos
{
    public class GetRegionDto
    {
        public required string RegionID { get; set; }
        public LocationCoordinates? LocationCoordinates { get; set; }
        public List<string>? DisasterTypes { get; set; } 
    }

    public class CreateRegionDto
    {
        public required string RegionID { get; set; }
        public LocationCoordinates? LocationCoordinates { get; set; }
        public List<DisasterTypeEnum>? DisasterTypes { get; set; }
    }

    public class UpdateRegionDto
    {
        public LocationCoordinates? LocationCoordinates { get; set; }
        public List<DisasterTypeEnum>? DisasterTypes { get; set; }
    }
}