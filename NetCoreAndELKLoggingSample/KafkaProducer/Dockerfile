FROM mcr.microsoft.com/dotnet/core/runtime:3.1-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["KafkaProducer/KafkaProducer.csproj", "KafkaProducer/KafkaProducer.csproj"]
COPY ["AvroTypes/AvroTypes.csproj", "AvroTypes/AvroTypes.csproj"]
RUN dotnet restore "KafkaProducer/KafkaProducer.csproj"
COPY . .
WORKDIR "/src/KafkaProducer"
RUN dotnet build "KafkaProducer.csproj" -c Release --no-restore

FROM build AS publish
WORKDIR "/src/KafkaProducer"
RUN dotnet publish "KafkaProducer.csproj" -c Release --no-restore --no-build -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KafkaProducer.dll"]