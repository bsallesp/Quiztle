@echo off
echo Building Docker Image for quiztle-postgresql...
docker build -f C:\Users\bsall\source\repos\Quiztle\Quiztle.Postgresql -t brunosallesdev/quiztle-postgresql:latest .
echo Build complete.
pause
