using System;
using System.Collections.Generic;

namespace plantMetricHandler.Models;

public partial class PlantDatum
{
    public int SensorId { get; set; }

    public DateTime Timestamp { get; set; }

    public double? Moisture { get; set; }

    public int? Sunlight { get; set; }

    public int? Temp { get; set; }

    public bool? VibrationStatus { get; set; }
}
