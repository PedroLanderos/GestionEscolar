# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto
COPY back/Proyecto.SubjectsApi.Solution/SubjectsApi.Presentation/SubjectsApi.Presentation.csproj SubjectsApi.Presentation/
COPY back/Proyecto.SubjectsApi.Solution/SubjectsApi.Application/SubjectsApi.Application.csproj SubjectsApi.Application/
COPY back/Proyecto.SubjectsApi.Solution/SubjectsApi.Domain/SubjectsApi.Domain.csproj SubjectsApi.Domain/
COPY back/Proyecto.SubjectsApi.Solution/SubjectsApi.Infrastructure/SubjectsApi.Infrastructure.csproj SubjectsApi.Infrastructure/
COPY back/Proyecto.SharedLibrarySolution/Proyecto.SharedLibrary/Proyecto.SharedLibrary.csproj Proyecto.SharedLibrary/

# Restaurar dependencias
RUN dotnet restore SubjectsApi.Presentation/SubjectsApi.Presentation.csproj

# Copiar el código fuente completo
COPY back/Proyecto.SubjectsApi.Solution/ SubjectsApi.Solution/
COPY back/Proyecto.SharedLibrarySolution/ Proyecto.SharedLibrarySolution/

# Cambiar al directorio del proyecto principal
WORKDIR /src/SubjectsApi.Solution/SubjectsApi.Presentation

# Publicar la aplicación
RUN dotnet publish SubjectsApi.Presentation.csproj -c Release -o /app/publish

# Etapa final: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Establecer la variable de entorno para usar el puerto 5000
ENV ASPNETCORE_URLS=http://+:5000

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "SubjectsApi.Presentation.dll"]
