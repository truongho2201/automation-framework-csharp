FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ENV NUGET_PACKAGES=/home/app/.nuget/packages

COPY global.json ./
COPY Directory.Build.props Directory.Packages.props ./
COPY tests/SeleniumTests/SeleniumTests.csproj tests/SeleniumTests/
RUN dotnet restore tests/SeleniumTests/SeleniumTests.csproj

COPY tests/SeleniumTests/ tests/SeleniumTests/
RUN dotnet build tests/SeleniumTests/SeleniumTests.csproj --configuration Release --no-restore

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /src
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/* && groupadd -r app && useradd -r -g app -d /home/app -m app

ENV NUGET_PACKAGES=/home/app/.nuget/packages
COPY --from=build --chown=app:app /home/app/.nuget/ /home/app/.nuget/
COPY --from=build --chown=app:app /src/ ./

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
USER app
ENTRYPOINT ["sh", "-c", "until curl --fail --silent http://localhost:4444/status > /dev/null; do sleep 2; done; exec dotnet test tests/SeleniumTests/SeleniumTests.csproj --configuration Release --no-build --no-restore"]