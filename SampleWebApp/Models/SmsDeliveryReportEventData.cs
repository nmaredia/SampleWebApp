using System;
using Newtonsoft.Json;

namespace SampleWebApp.Models
{
    public class SmsDeliveryReportEventData
    {
        public SmsDeliveryReportEventData()
        {
        }

        public SmsDeliveryReportEventData(string messageId, string from, string to, string deliveryStatus, string deliveryStatusDetails, string receivedTimestamp, SmsDeliveryAttempt[] deliveryAttempts)
        {
            MessageId = messageId;
            From = from;
            To = to;
            DeliveryStatus = deliveryStatus;
            DeliveryStatusDetails = deliveryStatusDetails;
            ReceivedTimestamp = receivedTimestamp;
            DeliveryAttempts = deliveryAttempts;
        }

        [JsonProperty(PropertyName = "messageId")]
        public string MessageId { get; set; }

        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        [JsonProperty(PropertyName = "deliveryStatus")]
        public string DeliveryStatus { get; set; }

        [JsonProperty(PropertyName = "deliveryStatusDetails")]
        public string DeliveryStatusDetails { get; set; }

        [JsonProperty(PropertyName = "receivedTimestamp")]
        public string ReceivedTimestamp { get; set; }

        [JsonProperty(PropertyName = "deliveryAttempts")]
        public SmsDeliveryAttempt[] DeliveryAttempts { get; set; }
    }
}
