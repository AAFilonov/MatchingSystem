FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestStand/TestStand.csproj", "TestStand/"]
RUN dotnet restore "TestStand/TestStand.csproj"
COPY . .
WORKDIR "/src/TestStand"
RUN dotnet build "TestStand.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestStand.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestStand.dll"]
