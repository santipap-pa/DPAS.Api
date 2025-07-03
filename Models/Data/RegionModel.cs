using System.ComponentModel.DataAnnotations.Schema;
using DPAS.Api.Enums;
using DPAS.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DPAS.Api.Models.Data
{
    public class RegionModel : BaseModel
    {
        public required string RegionID { get; set; }
        public required LocationCoordinates LocationCoordinates { get; set; }
        public required List<DisasterTypeEnum> DisasterTypes { get; set; }
    }

    public class LocationCoordinates
    {
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
    }
}