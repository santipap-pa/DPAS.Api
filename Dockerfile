FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

COPY --from=build /app/out .

RUN dotnet tool install --global dotnet-ef --version 9.0.6
ENV PATH="$PATH:/root/.dotnet/tools"

EXPOSE 80

# Run migrations and start
RUN echo '#!/bin/bash\n\
echo "Running database migrations..."\n\
dotnet ef database update --no-build\n\
echo "Starting application..."\n\
dotnet DPAS.Api.dll' > /app/start.sh && chmod +x /app/start.sh

ENTRYPOINT ["/app/start.sh"]