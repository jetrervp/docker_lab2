FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyApi.dll"]