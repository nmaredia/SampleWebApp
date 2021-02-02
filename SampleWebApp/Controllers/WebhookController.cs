using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using SampleWebApp.Models;
using System.IO;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("sms/events/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger _logger;

        public WebhookController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WebhookController>();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string response = string.Empty;
            const string SmsDeliveryReportEvent = "Microsoft.Communication.SMSDeliveryReportReceived";
            string requestContent = await new StreamReader(Request.Body).ReadToEndAsync();
            _logger.LogInformation($"Received events: {requestContent}");

            EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();
            eventGridSubscriber.AddOrUpdateCustomEventMapping(SmsDeliveryReportEvent, typeof(SmsDeliveryReportEventData));
            EventGridEvent[] eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(requestContent);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                if (eventGridEvent.Data is SubscriptionValidationEventData eventData)
                {
                    _logger.LogInformation($"Got SubscriptionValidation event data, validation code: {eventData.ValidationCode}, topic: {eventGridEvent.Topic}");
                    // Do any additional validation (as required) and then return back the below response

                    var responseData = new SubscriptionValidationResponse()
                    {
                        ValidationResponse = eventData.ValidationCode
                    };
                    return new OkObjectResult(responseData);
                }
                else if (eventGridEvent.Data is SmsDeliveryReportEventData deliveryReportEventData)
                {
                    _logger.LogInformation($"Got SmsDeliveryReport event data, messageId: {deliveryReportEventData.MessageId}, DeliveryStatus: {deliveryReportEventData.DeliveryStatus}");
                    return new OkObjectResult(deliveryReportEventData.MessageId);
                }
            }
            return new OkObjectResult(response);
        }
    }

}
