#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Quiztle.Frontend/Quiztle.Frontend/Quiztle.Frontend.csproj", "Quiztle.Frontend/Quiztle.Frontend/"]
COPY ["Quiztle.Frontend/Quiztle.Frontend.Client/Quiztle.Frontend.Client.csproj", "Quiztle.Frontend/Quiztle.Frontend.Client/"]
RUN dotnet restore "./Quiztle.Frontend/Quiztle.Frontend/Quiztle.Frontend.csproj"
COPY . .
WORKDIR "/src/Quiztle.Frontend/Quiztle.Frontend"
RUN dotnet build "./Quiztle.Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Quiztle.Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quiztle.Frontend.dll"]