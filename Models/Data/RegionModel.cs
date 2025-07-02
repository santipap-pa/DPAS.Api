using DPAS.Api.Enums;
using DPAS.Models.Data;

namespace DPAS.Api.Models.Data
{
    public class RegionModel : BaseModel
    {
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public required DisasterTypeEnum DisasterType { get; set; }
    }
}