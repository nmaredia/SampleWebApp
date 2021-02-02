using Newtonsoft.Json;
using System;

namespace SampleWebApp.Models
{
    public class SmsDeliveryAttempt
    {
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = "segmentsSucceeded")]
        public int SegmentsSucceeded { get; set; }

        [JsonProperty(PropertyName = "segmentsFailed")]
        public int SegmentsFailed { get; set; }
    }
}
