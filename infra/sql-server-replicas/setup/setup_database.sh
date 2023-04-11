#!/usr/bin/env bash

# Wait for database to startup 
sleep 15
./opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P MjJ8U4CZUS3X6y2L -i schema-create.sql

./opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P MjJ8U4CZUS3X6y2L -i seed-people.sql