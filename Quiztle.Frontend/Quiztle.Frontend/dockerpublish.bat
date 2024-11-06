@echo off
REM Navigate to the root project directory
cd C:\Users\bsall\source\repos\bsallesp\BrunoTheBot.Blazor

REM Build the Docker image
echo Building Docker image...
docker build -f Quiztle.Frontend/Quiztle.Frontend/Dockerfile -t quiztle-frontend .

REM Check if the build was successful
if %ERRORLEVEL% neq 0 (
    echo Error while building the image.
    exit /b %ERRORLEVEL%
)

REM Log in to Docker Hub (remove this line if already logged in)
echo Logging in to Docker Hub...
docker login

REM Check if the login was successful
if %ERRORLEVEL% neq 0 (
    echo Error while logging in to Docker Hub.
    exit /b %ERRORLEVEL%
)

REM Tag the image
echo Tagging image...
docker tag quiztle-frontend brunosallesdev/quiztlefrontend:latest

REM Push the image to Docker Hub
echo Pushing image to Docker Hub...
docker push brunosallesdev/quiztlefrontend:latest

REM Check if the push was successful
if %ERRORLEVEL% neq 0 (
    echo Error while pushing the image to Docker Hub.
    exit /b %ERRORLEVEL%
)

echo Operation completed successfully.

REM Pause to prevent window from closing immediately
pause
