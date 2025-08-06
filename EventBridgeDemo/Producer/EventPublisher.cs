using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using System.Text.Json;

public class EventPublisher
{
  private readonly AmazonEventBridgeClient _client;

  public EventPublisher()
  {
    _client = new AmazonEventBridgeClient();
  }

  public async Task PublishEventAsync(string name)
  {
    try
    {
      // Create the event request with a single entry.
      var request = new PutEventsRequest
      {
        Entries = new List<PutEventsRequestEntry>
            {
                new PutEventsRequestEntry
                {
                    Source = "my.custom.source", // Custom source identifier
                    DetailType = "MyCustomEvent", // Event type
                    Detail = JsonSerializer.Serialize(new { message = $"Hello from {name}!" }), // Event payload
                    EventBusName = "default" // Target event bus
                }
            }
      };

      // Send the event to EventBridge.
      var response = await _client.PutEventsAsync(request);

      foreach (var entry in response.Entries)
      {
        Console.WriteLine(entry.ErrorCode ?? "Event sent successfully!");
      }
    }
    catch (AmazonEventBridgeException ex)
    {
      Console.WriteLine($"AWS EventBridge error: {ex.Message}");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"General error while sending event: {ex.Message}");
    }

  }
}
