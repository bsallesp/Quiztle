@echo off
echo Building Docker Image for Quiztle Flask PDF Scanner...
docker build -f C:\Users\bsall\source\repos\Quiztle\Quiztle.Flask.PDFScanner\Dockerfile -t quiztle-flask-pdfscanner .
echo Build complete.

echo Publishing Docker Image to Repository...
docker push brunosallesdev/quiztle-flask-pdfscanner:latest
echo Publish complete.

pause
