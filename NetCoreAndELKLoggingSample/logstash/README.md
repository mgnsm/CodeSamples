The [docker-compose.yml](../docker-compose.yml) file uses [this Dockerfile][Dockerfile] to build a custom Logstash image that is based on the [official image](https://www.docker.elastic.co/r/logstash/logstash:7.9.2) and also installs the [Avro Schema Registry Codec](https://github.com/revpoint/logstash-codec-avro_schema_registry) plugin that is used to decode Avro records as Logstash events using an Avro schema from a [Confluent schema registry](https://github.com/confluentinc/schema-registry).
 
[kafka-pipeline.conf](kafka-pipeline.conf) defines the pipeline configuration that Logstash uses to consume the log messages from the Kafka topic, transform them and insert them into Elasticsearch. The custom `logstash_writer` user that is used to authenticate against Elasticsearch is created in [this Bash script](../elasticsearch/setup.sh) which is run when the Elasticsearch container is started. The default password is set in an [.env](../.env) file* and can be overridden by setting an environment variable:
 
    export LOGSTASH_WRITER_PASSWORD=secret?
    docker-compose -f ../docker-compose.yml --env-file ../.env up --build
 
***Note:** The `.env` file is provided for the sole purpose of being able to quickly get this *sample* solution up and running. In general, you should *not* store any secrets in a git repository.
