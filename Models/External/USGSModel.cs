using System.Text.Json.Serialization;

namespace DPAS.Models.External
{
    public class USGSResponse
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("metadata")]
        public USGSMetadata? Metadata { get; set; }

        [JsonPropertyName("features")]
        public List<EarthquakeFeature>? Features { get; set; }
    }

    public class USGSMetadata
    {
        [JsonPropertyName("generated")]
        public long Generated { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("api")]
        public string? Api { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    public class EarthquakeFeature
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("properties")]
        public EarthquakeProperties? Properties { get; set; }

        [JsonPropertyName("geometry")]
        public EarthquakeGeometry? Geometry { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }

    public class EarthquakeProperties
    {
        [JsonPropertyName("mag")]
        public double? Mag { get; set; }

        [JsonPropertyName("place")]
        public string? Place { get; set; }

        [JsonPropertyName("time")]
        public long Time { get; set; }

        [JsonPropertyName("updated")]
        public long Updated { get; set; }

        [JsonPropertyName("tz")]
        public string? Tz { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("detail")]
        public string? Detail { get; set; }

        [JsonPropertyName("felt")]
        public int? Felt { get; set; }

        [JsonPropertyName("cdi")]
        public double? Cdi { get; set; }

        [JsonPropertyName("mmi")]
        public double? Mmi { get; set; }

        [JsonPropertyName("alert")]
        public string? Alert { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("tsunami")]
        public int Tsunami { get; set; }

        [JsonPropertyName("sig")]
        public int Sig { get; set; }

        [JsonPropertyName("net")]
        public string? Net { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("ids")]
        public string? Ids { get; set; }

        [JsonPropertyName("sources")]
        public string? Sources { get; set; }

        [JsonPropertyName("types")]
        public string? Types { get; set; }

        [JsonPropertyName("nst")]
        public int? Nst { get; set; }

        [JsonPropertyName("dmin")]
        public double? Dmin { get; set; }

        [JsonPropertyName("rms")]
        public double? Rms { get; set; }

        [JsonPropertyName("gap")]
        public double? Gap { get; set; }

        [JsonPropertyName("magType")]
        public string? MagType { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }
    }

    public class EarthquakeGeometry
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("coordinates")]
        public List<double>? Coordinates { get; set; }
    }

    public class SeismicDataResponse
    {
        public double Magnitude { get; set; }
    }
}