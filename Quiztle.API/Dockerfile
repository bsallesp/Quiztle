#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Quiztle.API/Quiztle.API.csproj", "Quiztle.API/"]
COPY ["Quiztle.CoreBusiness/Quiztle.CoreBusiness.csproj", "Quiztle.CoreBusiness/"]
COPY ["Quiztle.DataContext/Quiztle.DataContext.csproj", "Quiztle.DataContext/"]
RUN dotnet restore "./Quiztle.API/Quiztle.API.csproj"
COPY . .
WORKDIR "/src/Quiztle.API"
RUN dotnet build "./Quiztle.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Quiztle.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quiztle.API.dll"] 