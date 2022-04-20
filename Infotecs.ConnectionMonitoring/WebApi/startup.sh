#!/bin/bash
service nginx start
dotnet /app/WebApi.dll
