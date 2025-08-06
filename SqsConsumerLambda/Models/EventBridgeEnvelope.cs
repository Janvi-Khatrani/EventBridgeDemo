
namespace SqsConsumerLambda.Models;

public class EventBridgeDetail
{
  public string Message { get; set; }
}

public class EventBridgeEnvelope
{
  public string Version { get; set; }
  public string Id { get; set; }
  public string DetailType { get; set; }
  public string Source { get; set; }
  public string Account { get; set; }
  public string Time { get; set; }
  public string Region { get; set; }
  public List<string> Resources { get; set; }
  public EventBridgeDetail Detail { get; set; }
}
