#!/usr/bin/env pwsh
$schaleDbUrl = 'https://beta.schaledb.com/data/cn/students.min.json'
Invoke-WebRequest $schaleDbUrl -OutFile students.json
