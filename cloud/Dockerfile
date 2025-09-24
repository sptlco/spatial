FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore
RUN dotnet publish Server/Spatial.Cloud.csproj -c Release -o /app/dist

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/dist ./

EXPOSE 80
EXPOSE 9010

ENTRYPOINT ["dotnet", "Spatial.Cloud.dll"]
