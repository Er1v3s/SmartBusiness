# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/mssql/server:2019-latest
RUN apt-get update && apt-get install -y mssql-tools unixodbc-dev
ENV PATH="$PATH:/opt/mssql-tools/bin"

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 2100
EXPOSE 2101


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Development

WORKDIR /src

COPY ["Services/AccountService/AccountService.Api/AccountService.Api.csproj", "Services/AccountService/AccountService.Api/"]
COPY ["Services/AccountService/AccountService.Application/AccountService.Application.csproj", "Services/AccountService/AccountService.Application/"]
COPY ["Services/AccountService/AccountService.Infrastructure/AccountService.Infrastructure.csproj", "Services/AccountService/AccountService.Infrastructure/"]
COPY ["Services/AccountService/AccountService.Domain/AccountService.Domain.csproj", "Services/AccountService/AccountService.Domain/"]
COPY ["Services/AccountService/AccountService.Contracts/AccountService.Contracts.csproj", "Services/AccountService/AccountService.Contracts/"]

RUN dotnet restore "./Services/AccountService/AccountService.Api/AccountService.Api.csproj"

COPY . .

WORKDIR "/src/Services/AccountService/AccountService.Api"

RUN dotnet build "./AccountService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Development

RUN dotnet publish "./AccountService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AccountService.Api.dll"]