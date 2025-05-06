# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto
COPY back/Proyecto.ScheduleApi.Solution/ScheduleApi.Presentation/ScheduleApi.Presentation.csproj ScheduleApi.Presentation/
COPY back/Proyecto.ScheduleApi.Solution/ScheduleApi.Application/ScheduleApi.Application.csproj ScheduleApi.Application/
COPY back/Proyecto.ScheduleApi.Solution/ScheduleApi.Domain/ScheduleApi.Domain.csproj ScheduleApi.Domain/
COPY back/Proyecto.ScheduleApi.Solution/ScheduleApi.Infrastructure/ScheduleApi.Infrastructure.csproj ScheduleApi.Infrastructure/
COPY back/Proyecto.SharedLibrarySolution/Proyecto.SharedLibrary/Proyecto.SharedLibrary.csproj Proyecto.SharedLibrary/

# Restaurar dependencias
RUN dotnet restore ScheduleApi.Presentation/ScheduleApi.Presentation.csproj

# Copiar el código fuente completo
COPY back/Proyecto.ScheduleApi.Solution/ ScheduleApi.Solution/
COPY back/Proyecto.SharedLibrarySolution/ Proyecto.SharedLibrarySolution/

# Cambiar al directorio del proyecto principal
WORKDIR /src/ScheduleApi.Solution/ScheduleApi.Presentation

# Publicar la aplicación
RUN dotnet publish ScheduleApi.Presentation.csproj -c Release -o /app/publish

# Etapa final: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Establecer la variable de entorno para usar el puerto 5000
ENV ASPNETCORE_URLS=http://+:5000

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "ScheduleApi.Presentation.dll"]
