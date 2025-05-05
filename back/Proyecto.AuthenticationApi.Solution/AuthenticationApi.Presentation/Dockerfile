# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto
COPY back/Proyecto.AuthenticationApi.Solution/AuthenticationApi.Presentation/AuthenticationApi.Presentation.csproj AuthenticationApi.Presentation/
COPY back/Proyecto.AuthenticationApi.Solution/AuthenticationApi.Application/AuthenticationApi.Application.csproj AuthenticationApi.Application/
COPY back/Proyecto.AuthenticationApi.Solution/AuthenticationApi.Domain/AuthenticationApi.Domain.csproj AuthenticationApi.Domain/
COPY back/Proyecto.AuthenticationApi.Solution/AuthenticationApi.Infrastructure/AuthenticationApi.Infrastructure.csproj AuthenticationApi.Infrastructure/
COPY back/Proyecto.SharedLibrarySolution/Proyecto.SharedLibrary/Proyecto.SharedLibrary.csproj Proyecto.SharedLibrary/

# Restaurar dependencias
RUN dotnet restore AuthenticationApi.Presentation/AuthenticationApi.Presentation.csproj

# Copiar el código fuente completo
COPY back/Proyecto.AuthenticationApi.Solution/ AuthenticationApi.Solution/
COPY back/Proyecto.SharedLibrarySolution/ Proyecto.SharedLibrarySolution/

# Cambiar al directorio del proyecto principal
WORKDIR /src/AuthenticationApi.Solution/AuthenticationApi.Presentation

# Publicar la aplicación
RUN dotnet publish AuthenticationApi.Presentation.csproj -c Release -o /app/publish

# Etapa final: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Establecer la variable de entorno para usar el puerto 5000
ENV ASPNETCORE_URLS=http://+:5000

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "AuthenticationApi.Presentation.dll"]