# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 2200
EXPOSE 2201


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Development

WORKDIR /src

COPY ["Services/SalesService/SalesService.Api/SalesService.Api.csproj", "Services/SalesService/SalesService.Api/"]
COPY ["Services/SalesService/SalesService.Application/SalesService.Application.csproj", "Services/SalesService/SalesService.Application/"]
COPY ["Services/SalesService/SalesService.Infrastructure/SalesService.Infrastructure.csproj", "Services/SalesService/SalesService.Infrastructure/"]
COPY ["Services/SalesService/SalesService.Domain/SalesService.Domain.csproj", "Services/SalesService/SalesService.Domain/"]
COPY ["Services/SalesService/SalesService.Contracts/SalesService.Contracts.csproj", "Services/SalesService/SalesService.Contracts/"]

RUN dotnet restore "./Services/SalesService/SalesService.Api/SalesService.Api.csproj"

COPY . .

WORKDIR "/src/Services/SalesService/SalesService.Api"

RUN dotnet build "./SalesService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Development

RUN dotnet publish "./SalesService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SalesService.Api.dll"]