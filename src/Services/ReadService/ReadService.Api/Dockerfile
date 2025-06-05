# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 2400
EXPOSE 2401


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Development

WORKDIR /src

COPY ["Services/ReadService/ReadService.Api/ReadService.Api.csproj", "Services/ReadService/ReadService.Api/"]
COPY ["Services/ReadService/ReadService.Application/ReadService.Application.csproj", "Services/ReadService/ReadService.Application/"]
COPY ["Services/ReadService/ReadService.Infrastructure/ReadService.Infrastructure.csproj", "Services/ReadService/ReadService.Infrastructure/"]
COPY ["Services/ReadService/ReadService.Domain/ReadService.Domain.csproj", "Services/ReadService/ReadService.Domain/"]

RUN dotnet restore "./Services/ReadService/ReadService.Api/ReadService.Api.csproj"

COPY . .

WORKDIR "/src/Services/ReadService/ReadService.Api"

RUN dotnet build "./ReadService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Development

RUN dotnet publish "./ReadService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ReadService.Api.dll"]