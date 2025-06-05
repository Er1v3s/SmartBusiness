# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 2300
EXPOSE 2301


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Development

WORKDIR /src

COPY ["Services/WriteService/WriteService.Api/WriteService.Api.csproj", "Services/WriteService/WriteService.Api/"]
COPY ["Services/WriteService/WriteService.Application/WriteService.Application.csproj", "Services/WriteService/WriteService.Application/"]
COPY ["Services/WriteService/WriteService.Infrastructure/WriteService.Infrastructure.csproj", "Services/WriteService/WriteService.Infrastructure/"]
COPY ["Services/WriteService/WriteService.Domain/WriteService.Domain.csproj", "Services/WriteService/WriteService.Domain/"]

RUN dotnet restore "./Services/WriteService/WriteService.Api/WriteService.Api.csproj"

COPY . .

WORKDIR "/src/Services/WriteService/WriteService.Api"

RUN dotnet build "./WriteService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Development

RUN dotnet publish "./WriteService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WriteService.Api.dll"]