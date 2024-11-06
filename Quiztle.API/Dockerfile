# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Log para verificação do caminho atual
RUN echo "Diretório de trabalho atual: $(pwd)"

# Usando caminhos relativos
COPY ["Quiztle.API/Quiztle.API.csproj", "Quiztle.API/"]
RUN echo "Arquivo Quiztle.API.csproj copiado com sucesso para /src/Quiztle.API"

COPY ["Quiztle.CoreBusiness/Quiztle.CoreBusiness.csproj", "Quiztle.CoreBusiness/"]
RUN echo "Arquivo Quiztle.CoreBusiness.csproj copiado com sucesso para /src/Quiztle.CoreBusiness"

COPY ["Quiztle.DataContext/Quiztle.DataContext.csproj", "Quiztle.DataContext/"]
RUN echo "Arquivo Quiztle.DataContext.csproj copiado com sucesso para /src/Quiztle.DataContext"

# Restauração de pacotes
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

# Comandos Docker para build, tag, login e push para Docker Hub
# Exemplo de uso:
# Va para a pasta C:\Users\bsall\source\repos\bsallesp\BrunoTheBot.Blazor> 
# docker build -f Quiztle.API/Dockerfile -t quiztle-api .
# docker tag quiztle-api brunosallesdev/quiztleapi:latest
# docker push brunosallesdev/quiztleapi:latest
