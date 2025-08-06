using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using SqsConsumerLambda.Models;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SqsConsumerLambda;

public class Function
{
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
      foreach (var message in evnt.Records)
      {
        try
        {
          context.Logger.LogLine($"Processing message: {message.Body}");

          var envelope = JsonSerializer.Deserialize<EventBridgeEnvelope>( 
                          message.Body,
                          new JsonSerializerOptions
                          {
                            PropertyNameCaseInsensitive = true
                          });

          if (envelope?.Detail?.Message == null)
          {
            context.Logger.LogLine("Message content missing or malformed.");
            continue;
          }

          string content = envelope.Detail.Message;
          context.Logger.LogLine($"Message content: {content}");

          if (content.Contains("fail-test", StringComparison.OrdinalIgnoreCase))
          {
            context.Logger.LogLine("Intentional failure triggered.");
            throw new Exception("Simulated failure for testing DLQ.");
          }

        }
        catch (Exception ex)
        {
          context.Logger.LogLine($"Error processing message {message.MessageId}: {ex.Message}");
          throw;
        }
      }
    }
}
