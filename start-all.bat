@echo off
title LogStreamX FULL SYSTEM STARTER

echo ================================
echo STARTING ZOOKEEPER
echo ================================

start cmd /k "cd /d C:\kafka\kafka_2.13-3.9.2 && bin\windows\zookeeper-server-start.bat config\zookeeper.properties"

timeout /t 10

echo ================================
echo STARTING KAFKA BROKER
echo ================================

start cmd /k "cd /d C:\kafka\kafka_2.13-3.9.2 && bin\windows\kafka-server-start.bat config\server.properties"

timeout /t 10

echo ================================
echo STARTING API
echo ================================

start cmd /k "cd /d C:\Users\hp\OneDrive\Desktop\Yashasvi\NetPractice\LogStreamX\LogStreamX.API && dotnet run"

timeout /t 5

echo ================================
echo STARTING WORKER
echo ================================

start cmd /k "cd /d C:\Users\hp\OneDrive\Desktop\Yashasvi\NetPractice\LogStreamX\LogStreamX.Worker && dotnet run"

echo ================================
echo ALL SERVICES STARTED 🚀
echo ================================

pause