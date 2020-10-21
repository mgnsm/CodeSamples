This project contains the Avro schema and the auto-generated C# types to be shared between the Kafka producer and the consumer.

Here is how to define the log messages:

1. Define or modify the [schema](Schema.v1.avsc).
2. Install the [Confluent.Apache.Avro.AvroGen](https://www.nuget.org/packages/Confluent.Apache.Avro.AvroGen/) .NET Core Tool using the .NET Core CLI:

        dotnet tool install --global Confluent.Apache.Avro.AvroGen

3. Run the tool in the `Avro` project directory to generate the C# types:

        avrogen -s Schema.v1.avsc .

4. Install the [Apache.Avro](https://www.nuget.org/packages/Apache.Avro/) NuGet package.