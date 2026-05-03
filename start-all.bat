@echo off
title LogStreamX SYSTEM STARTER (LOCAL + CLOUD KAFKA)

echo =========================================
echo 🚀 STARTING LOGSTREAMX SYSTEM
echo =========================================

echo.
echo 📦 Mode:
echo - API: LOCAL
echo - Worker: LOCAL
echo - DB: LOCAL (SQL Server)
echo - Kafka: CONFLUENT CLOUD ☁️
echo.

echo =========================================
echo STARTING API (http://localhost:5000)
echo =========================================

start "API" cmd /k "cd /d C:\Users\hp\OneDrive\Desktop\Yashasvi\NetPractice\LogStreamX\LogStreamX.API && dotnet run"

timeout /t 5 >nul

echo =========================================
echo STARTING WORKER (Kafka Consumer)
echo =========================================

start "Worker" cmd /k "cd /d C:\Users\hp\OneDrive\Desktop\Yashasvi\NetPractice\LogStreamX\LogStreamX.Worker && dotnet run"

timeout /t 3 >nul

echo =========================================
echo 🔗 SYSTEM ENDPOINTS
echo =========================================
echo Swagger: http://localhost:5000/swagger
echo API Logs: http://localhost:5000/api/logs
echo UI: http://localhost:5000/index.html

echo =========================================
echo 🧪 TEST FLOW
echo =========================================
echo 1. Open Swagger
echo 2. POST /api/LogPush
echo 3. Watch Worker console (Saved log)
echo 4. Refresh UI

echo =========================================
echo ⚠️ NOTE
echo =========================================
echo This setup will NOT show logs on:
echo https://logstreamx.onrender.com
echo (Different environment)

echo =========================================
echo ✅ SYSTEM READY 🚀
echo =========================================

pause