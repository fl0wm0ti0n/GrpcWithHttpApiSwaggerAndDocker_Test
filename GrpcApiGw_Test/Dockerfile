#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GrpcApiGw_Test/GrpcApiGw_Test.csproj", "GrpcApiGw_Test/"]
RUN dotnet restore "GrpcApiGw_Test/GrpcApiGw_Test.csproj"
COPY . .
WORKDIR "/src/GrpcApiGw_Test"
RUN dotnet build "GrpcApiGw_Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcApiGw_Test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcApiGw_Test.dll"]
