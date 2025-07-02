using DPAS.Api.Attributes;

namespace DPAS.Api.Enums
{
    public enum DisasterTypeEnum
    {
        [EnumDisplay("Other")]
        Other = 99,
        [EnumDisplay("Earthquake")]
        Earthquake = 1,
        [EnumDisplay("Flood")]
        Flood = 2,
        [EnumDisplay("Wildfire")]
        Wildfire = 3,
       
    }
}