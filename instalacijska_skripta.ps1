docker-compose -f ./kafka-ksqldb.yml up
docker exec -it ksqldb-cli ksql http://ksqldb-server:8088 -e "CREATE STREAM temperature (town VARCHAR, temperature DOUBLE) WITH (kafka_topic='weatherTown', value_format='json', partitions=1);"
docker exec -it ksqldb-cli ksql http://ksqldb-server:8088 -e "CREATE STREAM rain (town VARCHAR, rain INT) WITH (kafka_topic='rainTown', value_format='json', partitions=1);"
docker exec -it ksqldb-cli ksql http://ksqldb-server:8088 -e  "CREATE STREAM joined AS SELECT * FROM rain JOIN temperature WITHIN 10 SECONDS ON rain.town = temperature.town;"
