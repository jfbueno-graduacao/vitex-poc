FROM mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04

COPY setup_database.sh setup_database.sh
COPY entrypoint.sh entrypoint.sh

COPY schema-create.sql schema-create.sql
COPY seed-people.sql seed-people.sql

CMD /bin/bash ./entrypoint.sh