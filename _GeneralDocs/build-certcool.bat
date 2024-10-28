@echo off
setlocal

REM Define os caminhos dos projetos
set API_PROJECT=C:\Users\bsall\source\repos\bsallesp\BrunoTheBot.Blazor\Quiztle.API\Quiztle.API.csproj
set FRONTEND_PROJECT=C:\Users\bsall\source\repos\bsallesp\BrunoTheBot.Blazor\Quiztle.Frontend\Quiztle.Frontend\Quiztle.Frontend.csproj

REM Compila o projeto API
echo Building Quiztle.API...
dotnet build "%API_PROJECT%"
if %errorlevel% neq 0 (
    echo Build failed for Quiztle.API.
    exit /b %errorlevel%
)

REM Compila o projeto Frontend
echo Building Quiztle.Frontend...
dotnet build "%FRONTEND_PROJECT%"
if %errorlevel% neq 0 (
    echo Build failed for Quiztle.Frontend.
    exit /b %errorlevel%
)

echo Build completed successfully.

endlocal
pause
