#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaxCalculatorBlazorServer/TaxCalculatorBlazorServer.csproj", "TaxCalculatorBlazorServer/"]
COPY ["TaxCalculatorLibary/TaxCalculatorLibary.csproj", "TaxCalculatorLibary/"]
RUN dotnet restore "./TaxCalculatorBlazorServer/./TaxCalculatorBlazorServer.csproj"
COPY . .
WORKDIR "/src/TaxCalculatorBlazorServer"
RUN dotnet build "./TaxCalculatorBlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaxCalculatorBlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["TaxCalculatorBlazorServer/wwwroot", "wwwroot"]
ENTRYPOINT ["dotnet", "TaxCalculatorBlazorServer.dll"]

ENV \
    # For docker desktop because it switches to port 8080 by default !
    ASPNETCORE_URLS=http://+:80;https://+443 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true