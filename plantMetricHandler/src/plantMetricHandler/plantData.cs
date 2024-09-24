using System;

namespace plantMetricHandler
{
    [Table("plant_data")]
    public class PlantData
    {
        [Key]
        
        public int DeviceId { get; set; }

        [Required]
        [Column("sensor_id")]
        public string SensorId { get; set; }

        [Required]
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("moisture")]
        public double Moisture { get; set; }

        [Column("sunlight")]
        public int Sunlight { get; set; }

        [Column("temp")]
        public int Temp { get; set; }

        [Column("vibration_status")]
        public bool VibrationStatus { get; set; }
    }
}
