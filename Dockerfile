#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Exchange/Exchange.csproj", "Exchange/"]
RUN dotnet restore "Exchange/Exchange.csproj"
COPY . .
WORKDIR "/src/Exchange"
RUN dotnet build "Exchange.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exchange.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange.dll"]