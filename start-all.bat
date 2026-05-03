@echo off
title LogStreamX SYSTEM STARTER (LOCAL + CLOUD KAFKA)

echo =========================================
echo 🚀 STARTING LOGSTREAMX SYSTEM
echo =========================================

echo.
echo 📦 Mode:
echo - API: LOCAL (.NET)
echo - Worker: LOCAL (.NET Background Service)
echo - DB: LOCAL (SQL Server)
echo - Kafka: CONFLUENT CLOUD ☁️
echo.

REM ================================
REM START API
REM ================================
echo =========================================
echo 🚀 STARTING API
echo =========================================

start "LogStreamX API" cmd /k ^
"cd /d C:\Users\hp\OneDrive\Desktop\Yashasvi\NetPractice\LogStreamX\LogStreamX.API && dotnet run"

echo ⏳ Waiting for API to boot...
timeout /t 8 >nul

REM ================================
REM START WORKER
REM ================================
echo =========================================
echo ⚙️ STARTING WORKER (Kafka Consumer)
echo =========================================

start "LogStreamX Worker" cmd /k ^
"cd /d C:\Users\hp\OneDrive\Desktop\Yashasvi\NetPractice\LogStreamX\LogStreamX.Worker && dotnet run"

timeout /t 3 >nul

REM ================================
REM OPEN BROWSER
REM ================================
echo =========================================
echo 🌐 OPENING DASHBOARD
echo =========================================

start http://localhost:5000/index.html
start http://localhost:5000/swagger

REM ================================
REM INFO
REM ================================
echo.
echo =========================================
echo 🔗 SYSTEM ENDPOINTS
echo =========================================
echo Swagger UI : http://localhost:5000/swagger
echo API Logs   : http://localhost:5000/api/logs
echo UI         : http://localhost:5000/index.html

echo.
echo =========================================
echo 🧪 TEST FLOW
echo =========================================
echo 1. Open Swagger
echo 2. POST /api/LogPush
echo 3. Check Worker console → "Saved log"
echo 4. Refresh UI

echo.
echo =========================================
echo ⚠️ IMPORTANT NOTES
echo =========================================
echo - Worker MUST show "Kafka Consumer Started"
echo - If logs not visible → check Worker console
echo - This setup is LOCAL only
echo - Render deployment uses separate services

echo.
echo =========================================
echo ✅ SYSTEM READY 🚀
echo =========================================

pause