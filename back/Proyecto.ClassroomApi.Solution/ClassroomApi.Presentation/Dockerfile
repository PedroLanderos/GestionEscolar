# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto
COPY back/Proyecto.ClassroomApi.Solution/ClassroomApi.Presentation/ClassroomApi.Presentation.csproj ClassroomApi.Presentation/
COPY back/Proyecto.ClassroomApi.Solution/ClassroomApi.Application/ClassroomApi.Application.csproj ClassroomApi.Application/
COPY back/Proyecto.ClassroomApi.Solution/ClassroomApi.Domain/ClassroomApi.Domain.csproj ClassroomApi.Domain/
COPY back/Proyecto.ClassroomApi.Solution/ClassroomApi.Infrastructure/ClassroomApi.Infrastructure.csproj ClassroomApi.Infrastructure/
COPY back/Proyecto.SharedLibrarySolution/Proyecto.SharedLibrary/Proyecto.SharedLibrary.csproj Proyecto.SharedLibrary/

# Restaurar dependencias
RUN dotnet restore ClassroomApi.Presentation/ClassroomApi.Presentation.csproj

# Copiar el código fuente completo
COPY back/Proyecto.ClassroomApi.Solution/ ClassroomApi.Solution/
COPY back/Proyecto.SharedLibrarySolution/ Proyecto.SharedLibrarySolution/

# Cambiar al directorio del proyecto principal
WORKDIR /src/ClassroomApi.Solution/ClassroomApi.Presentation

# Publicar la aplicación
RUN dotnet publish ClassroomApi.Presentation.csproj -c Release -o /app/publish

# Etapa final: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Establecer la variable de entorno para usar el puerto 5000
ENV ASPNETCORE_URLS=http://+:5000

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "ClassroomApi.Presentation.dll"]
