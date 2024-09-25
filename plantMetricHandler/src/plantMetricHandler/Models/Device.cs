using System;
using System.Collections.Generic;

namespace plantMetricHandler.Models;

public partial class Device
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
}
