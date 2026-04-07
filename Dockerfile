FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY CVAnalyzer/ CVAnalyzer/
COPY CVAnalyzer.Business/ CVAnalyzer.Business/
COPY CVAnalyzer.Data/ CVAnalyzer.Data/
COPY CVAnalyzer.DbLayer/ CVAnalyzer.DbLayer/
COPY CVAnalyzer.Mappers/ CVAnalyzer.Mappers/
COPY CVAnalyzer.Models/ CVAnalyzer.Models/

RUN dotnet restore

WORKDIR /src/CVAnalyzer
RUN dotnet build "CVAnalyzer.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/CVAnalyzer
RUN dotnet publish "CVAnalyzer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CVAnalyzer.dll"]