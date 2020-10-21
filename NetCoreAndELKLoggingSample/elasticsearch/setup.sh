#!/bin/bash

if [ $# -eq 6 ]; then
	elasticsearchHost=$1
	elasticUser=$2
	elasticPassword=$3
	kibanaPassword=$4
	readerPassword=$5
	writerPassword=$6
else
	echo "Usage: <elasticsearchhost:port> <script-user> <script-password> <kibana-system-password> <read-only-user-password> <logstash-writer-user-password>"
	exit 1
fi

absolutPath="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

# Wait until Elasticsearch is up and running and ready to serve HTTP requests
maxAttempts=50
counter=1
user="$elasticUser:$elasticPassword"
until $(curl --user "$user" --silent --head --fail --output /dev/null "$elasticsearchHost"); do
	if [ $counter -eq $maxAttempts ];then
      echo "Failed to connect to $elasticsearchHost after $maxAttempts attempts."
      exit 1
    fi
	counter=$((counter+1))
	sleep 3
done

contentType='Content-Type: application/json'
# Create the index template
echo 'Creating the index template...'
curl --request PUT $elasticsearchHost/_template/log_template \
  --user "$user" \
  --data @"$absolutPath/index-template.json" \
  --header "$contentType" \
  --write-out '\n' \
  --silent
  
# Create the retention policy
echo 'Creating the index lifecycle management policy...'
curl --request PUT $elasticsearchHost/_ilm/policy/retention_policy \
  --user "$user" \
  --data @"$absolutPath/retention-policy.json" \
  --header "$contentType" \
  --write-out '\n' \
  --silent

# Set the password for the built-in 'kibana_system' user
echo 'Setting password for the kibana_system built-in user...'
curl --request POST $elasticsearchHost/_security/user/kibana_system/_password \
  --user "$user" \
  --header "$contentType" \
  --write-out '\n' \
  --silent \
  --data-binary @- << EOF
  {
    "password": "$kibanaPassword"
  }
EOF

# Create a reader role
echo 'Creating a read role...'
readerName='log_reader'
curl --request POST $elasticsearchHost/_security/role/$readerName \
  --user "$user" \
  --data @"$absolutPath/reader-role.json" \
  --header "$contentType" \
  --write-out '\n' \
  --silent

# Create a reader user
echo 'Creating a read user...'
curl --request POST $elasticsearchHost/_security/user/$readerName \
  --user "$user" \
  --header "$contentType" \
  --write-out '\n' \
  --silent \
  --data-binary @- << EOF
  {
    "password": "$readerPassword",
	"roles": ["$readerName"],
	"full_name": "Read-only user"
  }
EOF
 
# Create a writer role
echo 'Creating a writer role...'
writerName='logstash_writer'
curl --request POST $elasticsearchHost/_security/role/$writerName \
  --user "$user" \
  --data @"$absolutPath/writer-role.json" \
  --header "$contentType" \
  --write-out '\n' \
  --silent

# Create a writer (Logstash) user
echo 'Creating a writer user...'
curl --request POST $elasticsearchHost/_security/user/$writerName \
  --user "$user" \
  --header "$contentType" \
  --write-out '\n' \
  --silent \
  --data-binary @- << EOF
  {
    "password": "$writerPassword",
	"roles": ["$writerName"],
	"full_name": "Internal Logstash User"
  }
EOF

exit 0