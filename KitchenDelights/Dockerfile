#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /home/kitchendelights
EXPOSE 5050
EXPOSE 5051

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KitchenDelights/KitchenDelights.csproj", "KitchenDelights/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["Data/Data.csproj", "Data/"]
RUN dotnet restore "./KitchenDelights/./KitchenDelights.csproj"
COPY . .
WORKDIR "/src/KitchenDelights"
RUN dotnet build "./KitchenDelights.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KitchenDelights.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/kitchendelights
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KitchenDelights.dll"]
