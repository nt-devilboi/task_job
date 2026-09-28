FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8090
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TestJobSys/TestJobSys.csproj", "TestJobSys/"]
RUN dotnet restore "TestJobSys/TestJobSys.csproj"
COPY . .
WORKDIR "/src/TestJobSys"
RUN dotnet build "./TestJobSys.csproj" -c $BUILD_CONFIGURATION -o /app/build
WORKDIR /app/build
ENTRYPOINT ["dotnet", "TestJobSys.dll"]



