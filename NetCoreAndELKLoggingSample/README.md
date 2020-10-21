# Application Logging in .NET Core Using Apache Kafka and the ELK Stack

An example of how to use Apache Kafka to implement streaming logging in .NET Core and how to use the ELK stack (Elasticsearch, Logstash and Kibana) to store, index and display the log messages.

## Components
Docker is used to start the following components in separate containers. You can click on the links below to get more information about a specific component. The configuration for the Kafka related components is generally taken from [Confluent's official repository](https://github.com/confluentinc/cp-all-in-one/blob/6.0.0-post/cp-all-in-one-community/docker-compose.yml).

- A Zookeeper to keep track of status of the single-node (for development and testing purposes only) Kafka cluster
- A Kafka broker to handle produce, consume, and metadata requests from clients
- A schema registry to store schemas that describe the data models for the serialized Avro messages
- A single node (for development and testing purposes only) [Elasticsearch cluster](elasticsearch)
- A [Logstash data processing pipeline](logstash) that consumes the Kafka topic and inserts the data into Elasticsearch
- A Kibana web user interface that lets you visualize the logging data in Elasticsearch
- A [custom C#/.NET Core Kafka producer](KafkaProducer) microservice that is implemented as a console app

The solution also includes [a sample C#/.NET console app](KafkaConsumer) that can be used to consume the Kafka topic in real-time.

**Note:** If you intend to deploy a similar solution in production, you should refer to the official documentation that [Confluent](https://docs.confluent.io/5.0.0/installation/docker/docs/installation/clustered-deployment.html) and [Elastic](https://www.elastic.co/guide/en/elasticsearch/reference/current/docker.html) provide for Kafka and Elasticsearch respectively. The setup of the containers in this example is intended for development and testing purposes only.

## Run
Prerequisites:
- [Docker Desktop for Mac or Windows](https://www.docker.com/products/docker-desktop) or the [Docker Compose CLI for Linux](https://docs.docker.com/engine/context/aci-integration/#install-the-docker-compose-cli-on-linux) including [Docker Compose](https://docs.docker.com/compose/install/)
- Git

To be able to build and run the sample consumer app, which is the only component that doesn't run inside a container, you also need either the [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1?WT.mc_id=DOP-MVP-5001077) or an up-to-date version of Visual Studio 2019 or Visual Studio for Mac.

1. Clone this repository:

        git clone https://github.com/mgnsm/CodeSamples.git
        cd NetCoreAndELKLoggingSample

2. Optionally, modify the default passwords for the `elastic` and `kibana_system` and custom `log_reader` and `logstash_writer` users in the [.env](.env) file. The `elastic` superuser is used to [setup](elasticsearch) Elasticsearch using [this](elasticsearch/setup.sh) Bash script.

    **Note:** The `.env` file is provided for the sole purpose of being able to quickly get this sample solution up and running. In general, you should *not* store any secrets in a git repository.

3. Start the containers using the `docker-compose up` command:

        docker-compose up --build

4. Wait until all components have been initialized. You can list all running containers using the `docker ps` command.

5. Sign in to Kibana at http://localhost:5601/ using the built-in `elastic` superuser to set up some visualization of the logging data that Logstash inserts into Elasticsearch. Alternatively, you could query Eleasticsearch for some data using the [Search API](https://www.elastic.co/guide/en/elasticsearch/reference/current/search-search.html):

        http://localhost:9200/logs/_search?q=logLevel:Error&pretty

    Or run the .NET Core app to consume all log messages from the Kafka topic. It expects the addresses of the Kafka broker and the schema registry to be passed as command-line arguments:

        dotnet run -p KafkaConsumer/KafkaConsumer.csproj -c Release -- localhost:9092 localhost:8081

    Press any key to stop consuming messages and exit the sample app.

6. Gracefully stop the containers by pressing Ctrl + C and then remove them using the `docker-compose down` command. The `--volumes` (`-v`) option removes the [volumes](https://docs.docker.com/storage/volumes/) where the data is stored:

        docker-compose down --rmi all -v

### Visual Studio
You can also start the containers and debug and run the sample code using an up-to-date version of Visual Studio 2019 with the **.NET Core cross-platform development** workload installed:

1. Open the [solution](NetCoreAndELKLoggingSample.sln) file.
2. Right-click on the `docker-compose` project in the **Solution Explorer** and select **Set as Startup Project***. If you also want to start the consumer, [set multiple startup projects](https://docs.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?WT.mc_id=DOP-MVP-5001077).
3. Press F5 (or choose **Debug**->**Start Debugging** on the menu bar) to start with the debugger attached or press Ctrl + F5 (or choosing **Debug**->**Start Without Debugging** on the menu bar) to start without debugging.

## Questions and Feedback
If you have any questions or feedback, please [open an issue](https://docs.github.com/en/free-pro-team@latest/github/managing-your-work-on-github/creating-an-issue).
