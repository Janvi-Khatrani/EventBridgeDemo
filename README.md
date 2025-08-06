# EventBridgeDemo

This repository demonstrates an end-to-end integration between an **AWS EventBridge** event publisher and an **AWS Lambda** consumer triggered via **Amazon SQS**, implemented in **C# targeting .NET 8**.

---

## 🚀 Features

- ✅ Publish structured events from a .NET console app to EventBridge
- ✅ Route events to an SQS queue using an EventBridge rule
- ✅ Process SQS messages in a Lambda function
- ✅ Simulate message failure and route it to a **Dead Letter Queue (DLQ)**
- ✅ Log all Lambda executions via **CloudWatch Logs**

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
- Add permission to allow EventBridge to send messages to SQS

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

## 🧪 Testing the Flow

### ✅ Success Case

- Publish event with a name like `World`
- Lambda logs will show the processed message in **CloudWatch Logs**

### ❌ DLQ Failure Case

- Publish event with message: `fail-test`
- Lambda throws an intentional error
- Message is sent to `MyLambdaDLQ`

---

## 📝 Notes

- AWS credentials must have required permissions
- Ensure EventBridge rule pattern and source match
- Lambda uses `System.Text.Json` with case-insensitive options
- Error handling is implemented for test and production scenarios

---

## 📄 License

This project is provided for demonstration purposes. You are free to use and modify it under the terms of the MIT license.
