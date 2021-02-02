using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SampleWebApp.Models;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("sms/events/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger _logger;
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World!");
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            System.Diagnostics.Trace.TraceInformation("My message!");
            Debug.WriteLine("C# HTTP trigger function processed a request.");
            string response = string.Empty;
            const string SmsDeliveryReportEvent = "Microsoft.Communication.SMSDeliveryReportReceived";
            string requestContent = await new StreamReader(Request.Body).ReadToEndAsync();
            Debug.WriteLine($"Received events: {requestContent}");

            EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();
            eventGridSubscriber.AddOrUpdateCustomEventMapping(SmsDeliveryReportEvent, typeof(SmsDeliveryReportEventData));
            EventGridEvent[] eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(requestContent);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                if (eventGridEvent.Data is SubscriptionValidationEventData eventData)
                {
                    Debug.WriteLine(eventData.GetType().Name);
                    Debug.WriteLine($"Got SubscriptionValidation event data, validation code: {eventData.ValidationCode}, topic: {eventGridEvent.Topic}");
                    // Do any additional validation (as required) and then return back the below response

                    var responseData = new SubscriptionValidationResponse()
                    {
                        ValidationResponse = eventData.ValidationCode
                    };
                    return new OkObjectResult(responseData);
                }
                else if (eventGridEvent.Data is SmsDeliveryReportEventData deliveryReportEventData)
                {
                    var jsonEventData = JsonConvert.SerializeObject(deliveryReportEventData);
                    Debug.WriteLine(jsonEventData);
                    return new OkObjectResult(deliveryReportEventData.MessageId);
                }
            }
            return new OkObjectResult(response);
        }
    }

}
