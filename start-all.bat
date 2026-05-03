@echo off
title LogStreamX SYSTEM STARTER (CLOUD MODE)

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
echo USING CONFLUENT CLOUD KAFKA ☁️
echo ================================
echo Topic: logs-topic
echo Broker: pkc-921jm...

echo ================================
echo ALL SERVICES STARTED 🚀
echo ================================

pause