# EventBridgeDemo

A simple .NET 8.0 console application demonstrating AWS EventBridge event publishing and SQS message consumption.

## Features

- **Publish Event:** Send a custom event (with a user-provided name) to AWS EventBridge.
- **Poll SQS:** Poll messages from a specified AWS SQS queue and display them.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- AWS account with access to EventBridge and SQS
- AWS credentials configured (via environment variables, `~/.aws/credentials`, or IAM roles)
- The following NuGet packages (already referenced in the project):
  - `AWSSDK.EventBridge`
  - `AWSSDK.SQS`

## Project Structure

```
EventBridgeDemo/
│
├── Consumer/
│   └── SqsConsumer.cs
├── Producer/
│   └── EventPublisher.cs
├── Program.cs
├── EventBridgeDemo.csproj
└── ...
```

## Usage

1. **Restore dependencies:**
   ```sh
   dotnet restore
   ```

2. **Build the project:**
   ```sh
   dotnet build
   ```

3. **Run the application:**
   ```sh
   dotnet run
   ```

4. **Follow the prompts:**
   - Choose `1` to publish an event (you'll be asked for a name).
   - Choose `2` to poll messages from an SQS queue (you'll be asked for the queue name).
   - Choose `3` to exit.

## Example

```
Choose an option:
1. Publish event
2. Poll messages from SQS
3. Exit
Enter your choice: 1
Enter name to include in the event: Alice
Event published!

Choose an option:
1. Publish event
2. Poll messages from SQS
3. Exit
Enter your choice: 2
Enter SQS Queue Name: my-queue
Received: {"detail-type":"UserEvent","detail":{"name":"Alice"}}
```

## Notes

- Make sure your AWS credentials have permissions for EventBridge and SQS.
- The event and queue names should match your AWS setup.
