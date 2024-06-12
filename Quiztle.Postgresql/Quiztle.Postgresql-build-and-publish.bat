@echo off
echo Building Docker Image for quiztle-postgresql...
docker build -f C:\Users\bsall\source\repos\Quiztle\Quiztle.Postgresql\Dockerfile -t brunosallesdev/quiztle-postgresql:latest .
echo Build complete.

echo Publishing Docker Image to Repository...
docker push brunosallesdev/quiztle-postgresql:latest
echo Publish complete.

pause