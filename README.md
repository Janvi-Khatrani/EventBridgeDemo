# End-to-End AWS EventBridge Integration with .NET 8  
---

## 📖 Introduction

This demo showcases how to build a robust, event-driven integration using **AWS EventBridge**, **Amazon SQS**, and **AWS Lambda** with a .NET 8 console application.  
**EventBridge** is a serverless event bus that makes it easy to connect application components using events, enabling scalable, loosely-coupled architectures.  
This pattern is ideal for decoupling microservices, handling asynchronous workflows, and ensuring reliable message delivery with dead-letter queue (DLQ) support.

---

## 🌍 Real-World Use Cases

- **Order Processing Pipelines:** Decouple order creation from downstream fulfillment, inventory, and notification services.
- **Audit/Event Logging:** Capture and route business events for compliance or analytics.
- **IoT Event Ingestion:** Aggregate device events and trigger processing workflows.
- **Error Handling:** Ensure failed events are not lost by routing them to a DLQ for later inspection.

---
## 🚀 Workflow Overview

- ✅ Publish structured events from a .NET console app to EventBridge
- ✅ Route events to an SQS queue using an EventBridge rule
- ✅ Process SQS messages in a Lambda function
- ✅ Simulate message failure and route it to a **Dead Letter Queue (DLQ)**
- ✅ Log all Lambda executions via **CloudWatch Logs**
![EventBridge Flow](https://github.com/user-attachments/assets/79ece3d9-5eae-4b11-bf32-79b5a184ce64)

---

## ✅ Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- AWS CLI installed & configured (`aws configure`)
- AWS Account with IAM permissions for:
  - EventBridge
  - Lambda
  - SQS
  - CloudWatch Logs
- Visual Studio 2022+ (with AWS Toolkit) OR .NET CLI
- NuGet packages:
  - `AWSSDK.EventBridge`
  - `AWSSDK.SQS`
  - `Amazon.Lambda.Core`, `Amazon.Lambda.SQSEvents`

---

## 🧩 Project Structure

```
EventBridgeDemo/
├── EventBridgeDemo/                # .NET Console App (Event Publisher)
│   ├── Program.cs
│   └── Producer/
│       └── EventPublisher.cs
│
├── SqsConsumerLambda/             # AWS Lambda Function (Event Consumer)
│   ├── Function.cs
│   ├── Models/
│   │   └── EventBridgeEnvelope.cs
│   └── aws-lambda-tools-defaults.json
│
└── EventBridgeDemo.sln            # Solution file with both projects
```

---

## 🔧 Setup & Usage

### 1. Clone the Repository

```bash
git clone https://github.com/Janvi-Khatrani/EventBridgeDemo.git
cd EventBridgeDemo
```

### 2. AWS Setup

- Create SQS Queues:
  - `MyDemoQueue` (main queue)
  - `MyLambdaDLQ` (DLQ)
- Set DLQ configuration for `MyDemoQueue`
- Create EventBridge Rule with pattern:
  ```json
  {
    "source": ["my.custom.source"]
  }
  ```
- Target: `MyDemoQueue`
- Ensure EventBridge has permission to send messages to SQS.

### 3. Deploy the Lambda (via Visual Studio)

1. Open the solution in Visual Studio.
2. Right-click the `SqsConsumerLambda` project → **Publish to AWS Lambda**
3. Set:
   - Function Name: `SqsConsumerLambda`
   - IAM Role: Lambda role with SQS and CloudWatch permissions
   - Timeout: 30s, Memory: 128 MB
4. Click “Publish” to deploy.

After deployment, add the function as a trigger to your SQS queue (`MyDemoQueue`) in the AWS Console.`

### 4. Connect Lambda Trigger

- Go to **MyDemoQueue → Lambda Triggers**
- Add `SqsConsumerLambda` as a trigger

### 5. Publish Events (Console App)

```bash
cd EventBridgeDemo
dotnet restore
dotnet run
```

You’ll see:
```
Choose an option:
1. Publish event
2. Exit
```

Enter a name → sends an event to EventBridge.

---

Follow the prompts to publish events.

---

## 🧪 Testing the Flow

### ✅ Success Case

**Steps:**
1. Run the console app and choose `1` to publish an event.
2. Enter a name (e.g., `World`).
3. The event is sent to EventBridge, routed to SQS, and processed by Lambda.

**How to Check:**
- Go to **AWS Console → CloudWatch Logs → Log group for `SqsConsumerLambda`**.
- Find the latest log stream and verify the message content is logged.

**Expected Output:**

✅ Processed CloudWatchLog Entries
- Processing message: {
  "version": "0",
  "id": "0a88e39a-9c9d-8558-59c8-09b55ced30dd",
  ...
  "message": "Hello from World!"
}

- Message content: Hello from World!

### ❌ DLQ Failure Case

**Steps:**
1. Run the console app and choose `1` to publish an event.
2. Enter `fail-test` as the name.
3. Lambda will throw an intentional error.

**How to Check:**
- Go to **AWS Console → SQS → `MyLambdaDLQ` → Messages tab**.
- Inspect the failed message in the DLQ.
- Also, check CloudWatch Logs for error details.

**Expected Output:**
- Message will appears in DLQ for later review
  - {
  "version": "0",
  "id": "0a88e39a-9c9d-8558-59c8-09b55ced30dd",
  ...
  "message": "Hello from fail-test!"
}   
- 📄 CloudWatch Log Entries (Failure)
  - Error processing message bfa21123-c694-43da-a35f-36d96419bbd3: Simulated failure for testing DLQ.
  fail  
  System.Exception: Simulated failure for testing DLQ.
     at SqsConsumerLambda.Function.FunctionHandler(SQSEvent evnt, ILambdaContext context) 
        in C:\Users\janvi\EventBridge_Demo\EventBridgeDemo\SqsConsumerLambda\Function.cs:line 39
     at lambda_method1(Closure, Stream, ILambdaContext, Stream)
     at Amazon.Lambda.RuntimeSupport.HandlerWrapper.<>c__DisplayClass8_0.<GetHandlerWrapper>b__0(InvocationRequest invocation)
        in /src/Repo/Libraries/src/Amazon.Lambda.RuntimeSupport/Bootstrap/HandlerWrapper.cs:line 54
     at Amazon.Lambda.RuntimeSupport.LambdaBootstrap.InvokeOnceAsync(CancellationToken cancellationToken)
        in /src/Repo/Libraries/src/Amazon.Lambda.RuntimeSupport/Bootstrap/LambdaBootstrap.cs:line 269

---

## 🛠️ Troubleshooting Tips

- **❌ Event not triggering Lambda**
  - Ensure the EventBridge rule's `source` matches the event payload (`"my.custom.source"`).
  - Confirm the SQS queue has the Lambda function added as a trigger.
  - Check IAM permissions for EventBridge, SQS, and Lambda.

- **❌ No logs in CloudWatch**
  - Verify Lambda execution role includes CloudWatch permissions.
  - Make sure Lambda is deployed in the correct region.

- **❌ Messages not appearing in DLQ**
  - Confirm DLQ is configured for the SQS queue.
  - Check Lambda's error handling and retry settings.

- **❌ Malformed Event**
  - Ensure the event payload matches the expected JSON structure.
  - Lambda logs will show "Message content missing or malformed" if deserialization fails.

---

## 📄 License

This project is provided for demonstration purposes. You are free to use and modify it under the terms of the MIT license.
