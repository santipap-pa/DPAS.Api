using System.Text.Json.Serialization;

namespace DPAS.Models.External
{
    public class USGSResponse
    {
        [JsonPropertyName("features")]
        public List<EarthquakeFeature>? Features { get; set; }
    }

    public class EarthquakeFeature
    {
        [JsonPropertyName("properties")]
        public EarthquakeProperties? Properties { get; set; }
    }

    public class EarthquakeProperties
    {
        [JsonPropertyName("mag")]
        public double? Mag { get; set; }
    }

    public class SeismicDataResponse
    {
        public double Magnitude { get; set; }
    }
}