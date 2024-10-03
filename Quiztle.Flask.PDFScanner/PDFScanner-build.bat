@echo off
echo Building Docker Image for Quiztle Flask PDF Scanner...
docker build -f C:\Users\bsall\source\repos\Quiztle\Quiztle.Flask.PDFScanner\Dockerfile -t brunosallesdev/quiztle-flask-pdfscanner:latest .
echo Build complete.
pause
