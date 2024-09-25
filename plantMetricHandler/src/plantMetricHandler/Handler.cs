using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Microsoft.EntityFrameworkCore;
using plantMetricHandler.Models;

// Assembly attribute to specify the serializer
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace plantMetricHandler
{
    public class Function
    {
        private static readonly MorteinContext dbContext;

        // Static constructor to initialize the DbContext once per Lambda container
        static Function()
        {

            // Instantiate the DbContext with the configured options
            dbContext = new MorteinContext();
        }

        /// <summary>
        /// Lambda function handler to process PlantData and insert it into RDS.
        /// </summary>
        /// <param name="plantDatum">The PlantData object deserialized from the IoT message.</param>
        /// <param name="context">Lambda context for logging.</param>
        /// <returns></returns>
        public async Task FunctionHandler(PlantDatum plantDatum, ILambdaContext context)
        {
            try
            {
                // Set Timestamp if not provided
                if (plantDatum.Timestamp == default)
                {
                    plantDatum.Timestamp = DateTime.UtcNow;
                }

                // Log the incoming data for debugging
                context.Logger.LogLine($"Received PlantData: SensorId={plantDatum.SensorId}, Timestamp={plantDatum.Timestamp}, Moisture={plantDatum.Moisture}, Sunlight={plantDatum.Sunlight}, Temp={plantDatum.Temp}, VibrationStatus={plantDatum.VibrationStatus}");

                // Add the PlantData entry
                dbContext.PlantData.Add(plantDatum);

                // Save changes to the database
                await dbContext.SaveChangesAsync();

                context.Logger.LogLine("PlantData inserted successfully.");
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error inserting PlantData: {ex.Message}");
                context.Logger.LogLine(ex.StackTrace);

                throw;
            }
        }
    }
}
