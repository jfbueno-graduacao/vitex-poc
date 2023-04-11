#!/usr/bin/env bash

# Wait for database to startup 
test-connection() {
    (/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $MSSQL_SA_PASSWORD -Q 'Select 1' &> /dev/null)
    echo "$?"
}

while [ $(test-connection) -ne 0 ]; do
    sleep 2s
    echo 'Waiting 2 second and trying again'    
done

echo 'Database is up and running'

/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $MSSQL_SA_PASSWORD -i schema-create.sql

/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $MSSQL_SA_PASSWORD -i seed-people.sql