using DPAS.Api.Enums;
using DPAS.Api.Models.Data;

namespace DPAS.Api.Dtos
{
    public class GetRegionDto
    {
        public string RegionID { get; set; } = string.Empty;
        public LocationCoordinates? LocationCoordinates { get; set; }
        public List<string>? DisasterTypes { get; set; } 
    }

    public class CreateRegionDto
    {
        public string RegionID { get; set; } = string.Empty;
        public LocationCoordinates? LocationCoordinates { get; set; }
        public List<DisasterTypeEnum>? DisasterTypes { get; set; }
    }

    public class UpdateRegionDto
    {
        public LocationCoordinates? LocationCoordinates { get; set; }
        public List<DisasterTypeEnum>? DisasterTypes { get; set; }
    }
}