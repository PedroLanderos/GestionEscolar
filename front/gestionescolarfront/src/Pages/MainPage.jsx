import React, { useState, useContext, useEffect } from "react";
import axios from "axios";
import "./CSS/MainPage.css";
import RegisterRequest from "./RegisterRequest";
import PasswordResetRequests from "./PasswordResetRequests";
import Register from "./Register";
import AddTeacher from "./AddTeacher";
import UsersTable from "./UsersTable";
import AddSubject from "./AddSubject";
import Subjects from "./Subjects";
import AssignSubject from "./AssignSubject";
import AddSchedule from "./AddSchedule";
import Schedules from "./Schedules";
import Schedule from "./Schedule";
import AssignSchedule from "./AssignSchedule";
import User from "./User";
import ShowSchedule from "./ShowSchedule";
import CreateReport from "./CreateReport";
import AddSanction from "./AddSanction";
import AddSchoolYear from "./AddSchoolYear";
import TeacherClassesTable from "./TeacherClassesTable";
import AttendanceRegister from "./SetAttendance";
import SetGrades from "./SetGrades";
import ShowGrades from "./ShowGrades";
import ShowAttendance from "./ShowAttendance";
import ShowReport from "./ShowReports";
import ShowAssignments from "./ShowAssignments";
import ShowSanctions from "./ShowSanctions";
import ShowAbsences from "./ShowAbsences";
import Workshops from "./Workshops";
import ShowAllReports from "./ShowAllReports";
import { AuthContext } from "../Context/AuthContext";
import { useNavigate } from "react-router-dom";

const CICLO_API = "http://localhost:5004/api";

const MainPage = () => {
  const { auth } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (!auth.isAuthenticated) {
      navigate("/login");
    }
  }, [auth.isAuthenticated, navigate]);

  const isAdmin = auth.user?.role === "Administrador";
  const isTeacher = auth.user?.role === "Docente";
  const isStudent = auth.user?.role === "Alumno";
  const isTutor = auth.user?.role === "Padre";

  const [activeSection, setActiveSection] = useState(isAdmin ? "Administrador" : "Alumnos");
  const [activeSubOption, setActiveSubOption] = useState(null);
  const [reportesModo, setReportesModo] = useState(null);
  const [gradesModo, setGradesModo] = useState(null);
  const [registerData, setRegisterData] = useState(null);
  const [userType, setUserType] = useState(null);
  const [selectedSchedule, setSelectedSchedule] = useState(null);
  const [assignSchedule, setAssignSchedule] = useState(null);
  const [editingSubject, setEditingSubject] = useState(null);
  const [selectedUserId, setSelectedUserId] = useState(null);
  const [userMode, setUserMode] = useState(null);
  const [attendanceData, setAttendanceData] = useState(null);
  const [gradesData, setGradesData] = useState(null);
  const [activeCycleData, setActiveCycleData] = useState(null);

  // Saludo personalizado
  const [greeting, setGreeting] = useState("");

  useEffect(() => {
    const hour = new Date().getHours();
    let greet = "Hola";

    if (hour >= 6 && hour < 12) greet = "¡Buenos días";
    else if (hour >= 12 && hour < 19) greet = "¡Buenas tardes";
    else greet = "¡Buenas noches";

    setGreeting(greet);
  }, []);

  const handleUserView = (type) => {
    resetState();
    setActiveSubOption("VerUsuarios");
    setUserType(type);
  };

  const handleUserSelect = (id, mode) => {
    setSelectedUserId(id);
    setUserMode(mode);
  };

  const handleBackToUserList = () => {
    setSelectedUserId(null);
    setUserMode(null);
  };

  const resetState = () => {
    setRegisterData(null);
    setSelectedSchedule(null);
    setAssignSchedule(null);
    setEditingSubject(null);
    setUserType(null);
    setSelectedUserId(null);
    setUserMode(null);
    setActiveSubOption(null);
    setAttendanceData(null);
    setGradesData(null);
    setReportesModo(null);
    setGradesModo(null);
    setActiveCycleData(null);
  };

  const cargarCicloActivo = async () => {
    try {
      const res = await axios.get(`${CICLO_API}/CicloEscolar/actual`);
      if (res.status === 200 && res.data) {
        setActiveCycleData(res.data);
      } else {
        setActiveCycleData(null);
      }
    } catch (error) {
      console.error("Error al cargar ciclo escolar activo:", error);
      setActiveCycleData(null);
    }
    setActiveSubOption("CicloEscolarActivo");
  };

  const renderSidebarContent = () => {
    if (activeSection === "Administrador" && isAdmin) {
      return (
        <ul>
          <li onClick={() => { resetState(); setActiveSubOption("Solicitudes"); }}>Solicitudes de registro</li>
          <li onClick={() => { resetState(); setActiveSubOption("SolicitudesContrasena"); }}>Solicitudes de contraseña</li>
          <li onClick={() => { resetState(); setActiveSubOption("AgregarAlumno"); }}>Agregar alumno</li>
          <li onClick={() => { resetState(); setActiveSubOption("AgregarProfesor"); }}>Agregar profesor</li>
          <li onClick={() => { resetState(); setActiveSubOption("AgregarMateria"); }}>Agregar materia</li>
          <li onClick={() => { resetState(); setActiveSubOption("AsignarMateria"); }}>Asignar materia a docente</li>
          <li onClick={() => { resetState(); setActiveSubOption("Materias"); }}>Materias</li>
          <li onClick={() => { resetState(); setActiveSubOption("AgregarHorario"); }}>Agregar horario</li>
          <li onClick={() => { resetState(); setActiveSubOption("VerHorarios"); }}>Ver horarios</li>
          <li onClick={() => handleUserView("alumnos")}>Alumnos</li>
          <li onClick={() => handleUserView("docentes")}>Docentes</li>
          <li onClick={() => handleUserView("tutores")}>Tutores</li>
          <li onClick={() => handleUserView("administradores")}>Administradores</li>
          <li onClick={() => { resetState(); setActiveSubOption("RegistrarCicloEscolar"); }}>Registrar ciclo escolar</li>
          <li onClick={() => { resetState(); cargarCicloActivo(); }}>Ciclo Escolar Activo</li>
          <li onClick={() => { resetState(); setActiveSubOption("EliminarAsignaciones"); }}>Eliminar asignaciones</li>
          <li onClick={() => { resetState(); setActiveSubOption("VerSanciones"); }}>Ver sanciones</li>
          <li onClick={() => { resetState(); setActiveSubOption("VerInasistencias"); }}>Ver inasistencias</li>
          <li onClick={() => { resetState(); setActiveSubOption("VerReportes"); }}>Ver todos los reportes</li>
        </ul>
      );
    }

    return (
      <ul>
        <li onClick={() => { resetState(); setActiveSubOption("DatosPersonales"); }}>Datos Personales</li>
        {(isTeacher || isStudent || isTutor) && (
          <li onClick={() => { resetState(); setActiveSubOption("Horario"); }}>Horario</li>
        )}
        {isTeacher && (
          <>
            <li onClick={() => { resetState(); setActiveSubOption("CrearReporte"); }}>Crear Reporte</li>
            <li onClick={() => { resetState(); setActiveSubOption("AgregarSancion"); }}>Registrar Sanción</li>
            <li onClick={() => { resetState(); setActiveSubOption("ClasesDelMaestro"); }}>Administrar Clases</li>
          </>
        )}
        {(isStudent || isTutor) && (
          <>
            <li
              onClick={() => {
                resetState();
                setActiveSubOption("Calificaciones");
                setGradesModo(null);
              }}
            >
              Calificaciones
            </li>
            {activeSubOption === "Calificaciones" && (
              <>
                <li className="submenu" onClick={() => setGradesModo("ciclo")}>-- Por ciclo escolar</li>
                <li className="submenu" onClick={() => setGradesModo("historial")}>-- Historial completo</li>
              </>
            )}
            <li onClick={() => { resetState(); setActiveSubOption("Asistencias"); }}>Asistencias</li>
            <li onClick={() => setActiveSubOption("Reportes")}>Reportes</li>
            {activeSubOption === "Reportes" && (
              <>
                <li className="submenu" onClick={() => setReportesModo("ciclo")}>-- Ciclo Escolar</li>
                <li className="submenu" onClick={() => setReportesModo("semana")}>-- Semanal</li>
              </>
            )}
            <li onClick={() => { resetState(); setActiveSubOption("Talleres"); }}>Talleres</li>
          </>
        )}
      </ul>
    );
  };

  const renderMainContent = () => {
    if (activeSection === "Inicio") {
      return (
        <div className="welcome-message" style={{ padding: "1.5rem", fontSize: "1.3rem" }}>
          <h1>{greeting}, {auth.user?.name || "Usuario"}!</h1>
          <p>
            Bienvenido{(isAdmin || isTeacher) ? " al sistema de gestión escolar." : ", esperamos que tengas un excelente día de estudios."}
          </p>
        </div>
      );
    }

    if (gradesModo && (isStudent || isTutor)) {
      return (
        <ShowGrades
          modo={gradesModo}
          onBack={() => {
            setGradesModo(null);
            setActiveSubOption(null);
          }}
        />
      );
    }

    if (reportesModo && (isStudent || isTutor)) {
      return (
        <ShowReport
          modo={reportesModo}
          onBack={() => {
            setReportesModo(null);
            setActiveSubOption(null);
          }}
        />
      );
    }

    if (activeSection === "Administrador" && isAdmin) {
      if (selectedSchedule) return <Schedule schedule={selectedSchedule} onBack={() => setSelectedSchedule(null)} />;
      if (assignSchedule) return <AssignSchedule schedule={assignSchedule} onClose={() => setAssignSchedule(null)} />;
      if (editingSubject) return <AddSubject subject={editingSubject} onSuccess={() => { setEditingSubject(null); setActiveSubOption("Materias"); }} />;
      if (registerData) return <Register data={registerData} />;

      if (activeSubOption === "Solicitudes") return <RegisterRequest onRegisterClick={setRegisterData} />;
      if (activeSubOption === "SolicitudesContrasena") return <PasswordResetRequests />;
      if (activeSubOption === "AgregarAlumno") return <Register />;
      if (activeSubOption === "AgregarProfesor") return <AddTeacher />;
      if (activeSubOption === "VerUsuarios" && userType) {
        if (selectedUserId) return <User id={selectedUserId} mode={userMode} onBack={handleBackToUserList} />;
        return <UsersTable userType={userType} onSelectUser={handleUserSelect} />;
      }
      if (activeSubOption === "AgregarMateria") return <AddSubject />;
      if (activeSubOption === "Materias") return <Subjects onEdit={setEditingSubject} />;
      if (activeSubOption === "AsignarMateria") return <AssignSubject />;
      if (activeSubOption === "AgregarHorario") return <AddSchedule />;
      if (activeSubOption === "VerHorarios") return <Schedules onViewSchedule={setSelectedSchedule} onAssignSchedule={setAssignSchedule} />;
      if (activeSubOption === "RegistrarCicloEscolar") return <AddSchoolYear />;
      if (activeSubOption === "CicloEscolarActivo")
        return (
          <AddSchoolYear
            initialData={activeCycleData}
            onSuccess={() => {
              cargarCicloActivo();
            }}
            onBack={() => {
              resetState();
              setActiveSubOption(null);
            }}
          />
        );
      if (activeSubOption === "EliminarAsignaciones") return <ShowAssignments />;
      if (activeSubOption === "VerSanciones") return <ShowSanctions />;
      if (activeSubOption === "VerInasistencias") return <ShowAbsences />;
      if (activeSubOption === "VerReportes") return <ShowAllReports />;
    }

    if (activeSubOption === "DatosPersonales") return <User id={auth.user?.id} mode="view" onBack={resetState} />;
    if (activeSubOption === "Horario" && (isTeacher || isStudent || isTutor)) return <ShowSchedule />;
    if (activeSubOption === "CrearReporte") return <CreateReport onBack={resetState} />;
    if (activeSubOption === "AgregarSancion") return <AddSanction onBack={resetState} />;

    if (isTeacher) {
      if (attendanceData) {
        return <AttendanceRegister claseProfesor={attendanceData.claseProfesor} horarioId={attendanceData.horarioId} onBack={() => setAttendanceData(null)} />;
      }
      if (gradesData) {
        return (
          <SetGrades
            materiaProfesor={gradesData.claseProfesor}
            horarioId={gradesData.horario}
            onBack={() => setGradesData(null)}
          />
        );
      }
      if (activeSubOption === "ClasesDelMaestro") {
        return (
          <TeacherClassesTable
            teacherId={auth.user?.id}
            onRegistrarAsistencia={(claseProfesor, horarioId) => setAttendanceData({ claseProfesor, horarioId })}
            onRegistrarCalificacion={(claseProfesor, horario) => setGradesData({ claseProfesor, horario })}
          />
        );
      }
    }

    if (isStudent || isTutor) {
      if (activeSubOption === "Asistencias") return <ShowAttendance />;
      if (activeSubOption === "Talleres") return <Workshops />;
    }

    if ((isStudent || isTutor) && activeSubOption === "Calificaciones" && !gradesModo) {
      return <p>Por favor, selecciona una opción de calificaciones en el menú lateral.</p>;
    }

    return null;
  };

  // Accesos rápidos personalizados según rol
  const renderQuickAccess = () => {
    if (isAdmin) {
      // Para administrador NO mostrar accesos rápidos
      return null;
    }

    if (isStudent || isTutor || isTeacher) {
      return (
        <ul>
          <li
            style={{ cursor: "pointer" }}
            onClick={() => {
              resetState();
              setActiveSubOption("Horario");
            }}
          >
            Horario
          </li>
          <li
            style={{ cursor: "pointer" }}
            onClick={() => {
              resetState();
              setActiveSubOption("DatosPersonales");
            }}
          >
            Datos personales
          </li>
          <li
            style={{ cursor: "pointer" }}
            onClick={() => {
              resetState();
              if (isStudent || isTutor) {
                setReportesModo("semana");
                setActiveSubOption("Reportes");
              } else if (isTeacher) {
                setActiveSubOption("CrearReporte");
              }
            }}
          >
            Reportes
          </li>
        </ul>
      );
    }

    return null;
  };

  return (
    <>
      <div className="top-nav">
        <ul>
          <li
            className={`nav-item ${activeSection === "Inicio" ? "active-orange" : ""}`}
            onClick={() => {
              resetState();
              setActiveSection("Inicio");
            }}
          >
            Inicio
          </li>
          <li
            className={`nav-item ${activeSection === "Alumnos" ? "active-orange" : ""}`}
            onClick={() => {
              resetState();
              setActiveSection("Alumnos");
            }}
          >
            Alumnos
          </li>
          {isAdmin && (
            <li
              className={`nav-item ${activeSection === "Administrador" ? "active-orange" : ""}`}
              onClick={() => {
                resetState();
                setActiveSection("Administrador");
              }}
            >
              Administrador
            </li>
          )}
        </ul>
      </div>

      <div className="main-layout">
        <aside className="sidebar-left">{renderSidebarContent()}</aside>
        <main className="main-content">{renderMainContent()}</main>
        <aside className="sidebar-right">
          <h4>Accesos rápidos</h4>
          {renderQuickAccess()}
        </aside>
      </div>
    </>
  );
};

export default MainPage;
