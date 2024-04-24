FROM postgres:latest

RUN apt-get update && apt-get install -y postgresql-client

COPY Backups/Postgresql/BrunoTheBotDB.sql /docker-entrypoint-initdb.d/

RUN docker exec -t your-db-container pg_dumpall -c -U postgre > /docker-entrypoint-initdb.d/dump_$(date +%Y-%m-%d_%H_%M_%S).sql

EXPOSE 5432
