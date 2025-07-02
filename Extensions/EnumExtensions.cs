using System.Reflection;
using DPAS.Api.Attributes;

namespace DPAS.Api.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = field?.GetCustomAttribute<EnumDisplayAttribute>();
            return attribute?.Name ?? enumValue.ToString();
        }
    }
}