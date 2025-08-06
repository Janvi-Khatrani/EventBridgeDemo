var publisher = new EventPublisher();

while (true)
{
  Console.WriteLine("\nChoose an option:");
  Console.WriteLine("1. Publish event");
  Console.WriteLine("2. Exit");
  Console.Write("Enter your choice: ");

  var input = Console.ReadLine();

  switch (input)
  {
    case "1":
      Console.Write("Enter name to include in the event: ");
      var name = Console.ReadLine();
      await publisher.PublishEventAsync(name);
      break;

    case "2":
      Console.WriteLine("Exiting...");
      return;

    default:
      Console.WriteLine("Invalid choice. Please try again.");
      break;
  }
}
