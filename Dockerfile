FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine AS runtime
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /src
COPY MyLibrary/MyLibrary.csproj ./dist/
WORKDIR /app/dist
RUN dotnet restore

WORKDIR /app/
COPY dist/. ./dist/
WORKDIR /app/dist
RUN dotnet publish MyLibrary.csproj -c Release -o out

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/dist/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
