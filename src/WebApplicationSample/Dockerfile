FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["WebApplicationSample/WebApplicationSample.csproj", "WebApplicationSample/"]
RUN dotnet restore "WebApplicationSample/WebApplicationSample.csproj"
COPY . .
WORKDIR "/src/WebApplicationSample"
RUN dotnet build "WebApplicationSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplicationSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplicationSample.dll"]