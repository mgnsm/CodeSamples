This solution folder contains the definition for the Elasticsearch [index](index-template.json), an index lifecycle management (ILM) (or retention) [policy](retention-policy.json), custom [reader](reader-role.json) and [writer](writer-role.json) roles and a [Bash script](setup.sh) that uses [cURL](https://curl.haxx.se/) to set up everything. This includes creating two custom users (`log_reader` and `logstash_writer`) and setting the passwords for the built-in `elastic` and `kibana_system` users.
 
The [docker-compose.yml](../docker-compose.yml) file runs the script in the background when the Elasticsearch container is started.
 
**Note:** The passwords for the built-in `elastic` and `kibana_system` and custom `log_reader` and `logstash_writer` users are defined in an [.env](../.env) file which is provided for the sole purpose of being able to quickly get this sample solution up and running. In general, you should *not* store any secrets in a git repository. 

You can override the default sample credentials by setting the corresponding environment variables in the shell before you start the Docker container:

    export ELASTIC_PASSWORD=secret1
    export KIBANA_SYSTEM_PASSWORD=secret2
    export ELASTIC_READ_ONLY_USER_PASSWORD=secret3
    export LOGSTASH_WRITER_PASSWORD=secret4
    docker-compose -f ../docker-compose.yml up elasticsearch

Alternatively, you could run the script manually and pass the passwords as parameters to it:

    docker run -d -p 9200:9200 -e "discovery.type=single-node" -e "xpack.security.enabled=true" -e "ELASTIC_PASSWORD=secret1" docker.elastic.co/elasticsearch/elasticsearch:7.9.2
    sh setup.sh localhost:9200 elastic secret1 secret2 secret3 secret4
