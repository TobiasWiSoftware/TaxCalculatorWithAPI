#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaxCalculatorAPI/TaxCalculatorAPI.csproj", "TaxCalculatorAPI/"]
COPY ["TaxCalculatorLibary/TaxCalculatorLibary.csproj", "TaxCalculatorLibary/"]
RUN dotnet restore "./TaxCalculatorAPI/./TaxCalculatorAPI.csproj"
COPY . .
WORKDIR "/src/TaxCalculatorAPI"
RUN dotnet build "./TaxCalculatorAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaxCalculatorAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaxCalculatorAPI.dll"]

RUN apt-get update && apt-get install -y nano
