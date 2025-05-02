# Etapa base: Imagen de .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el archivo del proyecto SharedLibrary
COPY Llaveremos.SharedLibrary.csproj ./

# Restaurar dependencias
RUN dotnet restore

# Copiar todo el código fuente
COPY . ./

# Compilar el proyecto
RUN dotnet build -c Release -o /app/build

# Publicar el proyecto
RUN dotnet publish -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
