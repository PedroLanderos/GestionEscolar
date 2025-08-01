# 📚 Sistema de Gestión Escolar

Este proyecto es un sistema de gestión escolar desarrollado con una arquitectura de microservicios. La solución está compuesta por un backend en .NET 8 y un frontend en React. Cada microservicio sigue el patrón de Clean Architecture, y todo el sistema puede ser ejecutado de manera orquestada mediante Docker.

---

## 🧱 Estructura del Proyecto

```plaintext
back/
│
├── Proyecto.AuthenticationApi.Solution
├── Proyecto.ClassroomApi.Solution
├── Proyecto.NotificationApi.Solution
├── Proyecto.ScheduleApi.Solution
├── Proyecto.SharedLibrarySolution
└── Proyecto.SubjectsApi.Solution
```

---

## ⚙️ Tecnologías Utilizadas

### Backend (.NET 8)
- Microservicios independientes
- Arquitectura limpia (Clean Architecture):
  - Domain
  - Application
  - Infrastructure
  - Presentation
- Autenticación con JWT
- Pruebas:
  - Pruebas unitarias
  - Pruebas de estrés
  - Pruebas de humo

### Frontend (React)
- Interfaz de usuario moderna
- Comunicación con microservicios mediante API REST

### Postman
- Colección disponible para probar los endpoints de los microservicios

### Docker
- Todo el sistema está dockerizado
- Uso de `docker-compose` para levantar todo el entorno de forma sencilla

---

## 🚀 Cómo Ejecutar el Proyecto

1. Clona este repositorio
2. En la raíz del proyecto, ejecuta el siguiente comando:

```bash
docker-compose up --build
```

3. El frontend estará disponible en: [http://localhost:3000](http://localhost:3000)
4. Cada microservicio se expone en su propio puerto configurado en el archivo `docker-compose.yml`

---

## 🧪 Pruebas

Este sistema cuenta con un enfoque integral de pruebas:

- ✅ Pruebas unitarias para asegurar la lógica interna de los servicios
- ✅ Pruebas de estrés para medir el rendimiento bajo carga
- ✅ Pruebas de humo para validación rápida del sistema
- ✅ Colección de Postman para pruebas manuales y automatizadas


