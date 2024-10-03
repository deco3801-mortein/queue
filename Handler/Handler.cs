using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Mortein.Types;
using System.Text.Json;
using NodaTime.Serialization.SystemTextJson;
using NodaTime;
using Microsoft.EntityFrameworkCore;
// Assembly attribute to specify the serializer
[assembly: LambdaSerializer(typeof(Handler.CustomLambdaSerializer))]

namespace Handler;

public class Function
{
    private static readonly DatabaseContext dbContext;

    // Static constructor to initialize the DbContext once per Lambda container
    static Function()
    {

        // Instantiate the DbContext with the configured options
        dbContext = new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>()
            .EnableSensitiveDataLogging().Options);

        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME")))
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }

    /// <summary>
    /// Lambda function handler to process PlantData and insert it into RDS.
    /// </summary>
    /// <param name="healthcheckDatum">The PlantData object deserialized from the IoT message.</param>
    /// <param name="context">Lambda context for logging.</param>
    /// <returns></returns>
    public async Task FunctionHandler(HealthcheckDatum healthcheckDatum, ILambdaContext context)
    {
        // Log the incoming data for debugging
        context.Logger.LogLine($"Received PlantData: {JsonSerializer.Serialize(healthcheckDatum)}");

        try
        {
            // Add the HealthcheckData entry
            dbContext.HealthcheckData.Add(healthcheckDatum);
            context.Logger.LogLine("Added");
            // Save changes to the database
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error inserting PlantData: {ex.Message}");
            context.Logger.LogLine(ex.StackTrace);

            dbContext.HealthcheckData.Remove(healthcheckDatum);
            throw;
        }
        context.Logger.LogLine("PlantData inserted successfully.");
    }
}

public class CustomLambdaSerializer : DefaultLambdaJsonSerializer
{
    public CustomLambdaSerializer()
        : base(CreateCustomizer())
    { }

    private static Action<JsonSerializerOptions> CreateCustomizer()
    {
        return (JsonSerializerOptions options) =>
        {
            options.ConfigureForNodaTime(DateTimeZoneProviders.Bcl);
        };
    }
}
