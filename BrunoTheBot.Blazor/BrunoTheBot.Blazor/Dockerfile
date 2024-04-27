#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 443
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BrunoTheBot.Blazor/BrunoTheBot.Blazor/BrunoTheBot.Blazor.csproj", "BrunoTheBot.Blazor/BrunoTheBot.Blazor/"]
COPY ["BrunoTheBot.API/BrunoTheBot.API.csproj", "BrunoTheBot.API/"]
COPY ["BrunoTheBot.CoreBusiness/BrunoTheBot.CoreBusiness.csproj", "BrunoTheBot.CoreBusiness/"]
COPY ["BrunoTheBot.DataContext/BrunoTheBot.DataContext.csproj", "BrunoTheBot.DataContext/"]
COPY ["BrunoTheBot.Blazor/BrunoTheBot.Blazor.Client/BrunoTheBot.Blazor.Client.csproj", "BrunoTheBot.Blazor/BrunoTheBot.Blazor.Client/"]
RUN dotnet restore "./BrunoTheBot.Blazor/BrunoTheBot.Blazor/BrunoTheBot.Blazor.csproj"
COPY . .
WORKDIR "/src/BrunoTheBot.Blazor/BrunoTheBot.Blazor"
RUN dotnet build "./BrunoTheBot.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BrunoTheBot.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrunoTheBot.Blazor.dll"]