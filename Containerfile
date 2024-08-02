FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore

WORKDIR /source
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
RUN apt-get update && apt-get install -y wget
RUN wget https://github.com/nats-io/natscli/releases/download/v0.1.5/nats-0.1.5-amd64.deb
RUN dpkg -i nats-0.1.5-amd64.deb
WORKDIR /app
COPY --from=build /app ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "BusNoti.dll"]

# podman pull mcr.microsoft.com/dotnet/sdk:8.0
# podman built -t akos_busnoti .
# podman run -it -p 8080:8080 akos_busnoti
