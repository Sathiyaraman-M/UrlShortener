#!/bin/bash
# Marker file location
MARKER_FILE="/docker-entrypoint-initdb.d/.script_ran"

# Wait for SQL Server to start up
TRIES=60
DBSTATUS=1
ERRCODE=1
i=0

while [[ $DBSTATUS -ne 0 ]] && [[ $i -lt $TRIES ]]; do
	i=$((i+1))
	DBSTATUS=$(/opt/mssql-tools/bin/sqlcmd -h -1 -t 1 -S sql-server -U SA -P $SQL_SERVER_SA_PASSWORD -Q "SET NOCOUNT ON; Select COALESCE(SUM(state), 0) from sys.databases") || DBSTATUS=1

	sleep 1s
done

if [ $DBSTATUS -ne 0 ]; then
	echo "SQL Server took more than $TRIES seconds to start up or one or more databases are not in an ONLINE state"
	exit 1
fi

if [ ! -f "$MARKER_FILE" ]; then
    echo "Running the initial setup script..."

    # Run the SQL script using environment variables
    /opt/mssql-tools/bin/sqlcmd -S sql-server -U SA -P $SQL_SERVER_SA_PASSWORD -d master -i /docker-entrypoint-initdb.d/create_db.sql
    /opt/mssql-tools/bin/sqlcmd -S sql-server -U SA -P $SQL_SERVER_SA_PASSWORD -d UrlShortener -i /docker-entrypoint-initdb.d/script.sql

    # Create the marker file to indicate that the script has been run
    touch $MARKER_FILE

    echo "Initial setup script has been run."
else
    echo "Initial setup script has already been run. Skipping."
fi
