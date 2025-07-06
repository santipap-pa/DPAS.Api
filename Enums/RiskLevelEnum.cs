using DPAS.Api.Attributes;

namespace DPAS.Api.Enums
{
    public enum RiskLevelEnum
    {
        [EnumDisplay("Low")]
        Low = 1,
        [EnumDisplay("Medium")]
        Medium = 2,
        [EnumDisplay("High")]
        High = 3,
    }
}