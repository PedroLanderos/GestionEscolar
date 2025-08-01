# 📚 School Management System

This project is a school management system developed with a microservices architecture. The solution consists of a .NET 8 backend and a React frontend. Each microservice follows the Clean Architecture pattern, and the entire system can be orchestrated and run using Docker.

---

## 🧱 Project Structure

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

## ⚙️ Technologies Used

### Backend (.NET 8)
- Independent microservices
- Clean Architecture:
  - Domain
  - Application
  - Infrastructure
  - Presentation
- Authentication with JWT
- Testing:
  - Unit tests
  - Stress tests
  - Smoke tests

### Frontend (React)
- Modern user interface
- Communication with microservices via REST API

### Postman
- Collection available to test the microservices endpoints

### Docker
- The entire system is dockerized
- Uses `docker-compose` to easily launch the whole environment

---

## 🚀 How to Run the Project

1. Clone this repository
2. In the root directory of the project, run the following command:

```bash
docker-compose up --build
```

3. The frontend will be available at: [http://localhost:3000](http://localhost:3000)
4. Each microservice is exposed on its own port configured in the `docker-compose.yml` file

---

## 🧪 Testing

This system features a comprehensive testing approach:

- ✅ Unit tests to ensure the internal logic of the services
- ✅ Stress tests to measure performance under load
- ✅ Smoke tests for quick system validation
- ✅ Postman collection for manual and automated testing

