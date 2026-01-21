#nullable enable
using System.Collections.Generic;

public class FeatureCollection
{
    // USGS GeoJSON has: { "features": [ ... ] }
    public List<Feature> Features { get; set; } = new();
}

public class Feature
{
    // Each feature has: { "properties": { "mag": ..., "place": ... } }
    public Properties? Properties { get; set; }
}

public class Properties
{
    public double? Mag { get; set; }
    public string? Place { get; set; }
}