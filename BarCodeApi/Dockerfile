FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["BarCodeApi/BarCodeApi.csproj", "BarCodeApi/"]
RUN dotnet restore "BarCodeApi/BarCodeApi.csproj"
COPY . .
WORKDIR "/src/BarCodeApi"
RUN dotnet build "BarCodeApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BarCodeApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BarCodeApi.dll"]