#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Quiztle.Blazor/Quiztle.Blazor/Quiztle.Blazor.csproj", "Quiztle.Blazor/Quiztle.Blazor/"]
COPY ["Quiztle.API/Quiztle.API.csproj", "Quiztle.API/"]
COPY ["Quiztle.CoreBusiness/Quiztle.CoreBusiness.csproj", "Quiztle.CoreBusiness/"]
COPY ["Quiztle.DataContext/Quiztle.DataContext.csproj", "Quiztle.DataContext/"]
COPY ["Quiztle.Blazor/Quiztle.Blazor.Client/Quiztle.Blazor.Client.csproj", "Quiztle.Blazor/Quiztle.Blazor.Client/"]
RUN dotnet restore "./Quiztle.Blazor/Quiztle.Blazor/Quiztle.Blazor.csproj"
COPY . .
WORKDIR "/src/Quiztle.Blazor/Quiztle.Blazor"
RUN dotnet build "./Quiztle.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Quiztle.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quiztle.Blazor.dll"]