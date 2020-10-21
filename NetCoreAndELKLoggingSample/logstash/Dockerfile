FROM docker.elastic.co/logstash/logstash:7.9.2

# Remove the example config from the base image
RUN rm -f /usr/share/logstash/pipeline/logstash.conf

# Add the Kafka pipeline
COPY ["logstash/kafka-pipeline.conf", "/usr/share/logstash/pipeline/kafka.conf"]

# Install Avro Schema Registry Codec (https://github.com/revpoint/logstash-codec-avro_schema_registry)
RUN /usr/share/logstash/bin/logstash-plugin install logstash-codec-avro_schema_registry