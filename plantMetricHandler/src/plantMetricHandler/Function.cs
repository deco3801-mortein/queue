using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace plantMetricHandler;

public class PlantData
{
    public required string sensor_id { get; set; }
    public required string timestamp { get; set; }
    public double moisture { get; set; }
    public int sunlight { get; set; }
    public int temp { get; set; }
}
public class Function
{
    
    /// <summary>
    /// Sends Data to RDS service/// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public string FunctionHandler(PlantData input, ILambdaContext context)
    {
        return input.sensor_id;
    }
}
