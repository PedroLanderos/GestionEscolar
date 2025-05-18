import React, { useState, useContext, useEffect } from "react";
import "./CSS/MainPage.css";
import RegisterRequest from "./RegisterRequest";
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
import { AuthContext } from "../Context/AuthContext";
import { useNavigate } from "react-router-dom";

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

  const [activeSection, setActiveSection] = useState(isAdmin ? "Administrador" : "Alumnos");
  const [activeSubOption, setActiveSubOption] = useState(null);
  const [registerData, setRegisterData] = useState(null);
  const [userType, setUserType] = useState(null);
  const [selectedSchedule, setSelectedSchedule] = useState(null);
  const [assignSchedule, setAssignSchedule] = useState(null);
  const [editingSubject, setEditingSubject] = useState(null);
  const [selectedUserId, setSelectedUserId] = useState(null);
  const [userMode, setUserMode] = useState(null);

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
  };

  const getGreeting = () => {
    const hour = new Date().getHours();
    if (hour < 12) return "BUENOS DÍAS";
    if (hour < 19) return "BUENAS TARDES";
    return "BUENAS NOCHES";
  };

  const renderSidebarContent = () => {
    if (activeSection === "Administrador" && isAdmin) {
      return (
        <ul>
          <li onClick={() => { resetState(); setActiveSubOption("Solicitudes"); }}>Solicitudes de registro</li>
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
        </ul>
      );
    }

    return (
      <ul>
        <li onClick={() => { resetState(); setActiveSubOption("DatosPersonales"); }}>Datos Personales</li>
        {(isTeacher || isStudent) && (
          <li onClick={() => { resetState(); setActiveSubOption("Horario"); }}>Horario</li>
        )}
        {isTeacher && (
          <li onClick={() => { resetState(); setActiveSubOption("CrearReporte"); }}>Crear Reporte</li>
        )}
        <li>Talleres</li>
        <li>Asistencias</li>
        <li>Reportes</li>
        <li>Sanciones</li>
        <li>Datos Académicos</li>
        <li className="submenu">- Kárdex</li>
      </ul>
    );
  };

  const renderMainContent = () => {
    if (activeSection === "Administrador" && isAdmin) {
      if (selectedSchedule) return <Schedule schedule={selectedSchedule} onBack={() => setSelectedSchedule(null)} />;
      if (assignSchedule) return <AssignSchedule schedule={assignSchedule} onClose={() => setAssignSchedule(null)} />;
      if (editingSubject) return <AddSubject subject={editingSubject} onSuccess={() => { setEditingSubject(null); setActiveSubOption("Materias"); }} />;
      if (registerData) return <Register data={registerData} />;
      if (activeSubOption === "Solicitudes") return <RegisterRequest onRegisterClick={setRegisterData} />;
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
    }

    if (activeSubOption === "DatosPersonales") return <User id={auth.user?.id} mode="view" onBack={resetState} />;
    if (activeSubOption === "Horario") return <ShowSchedule />;
    if (activeSubOption === "CrearReporte") return <CreateReport onBack={resetState} />;

    return (
      <>
        <h2>MENÚ PRINCIPAL DE {activeSection.toUpperCase()}</h2>
        <h3>{getGreeting()}</h3>
        <h3>{auth.user?.name?.toUpperCase()}</h3>
        <p className="notice">
          <strong>AVISO:</strong> Recuerda que la credencial del IPN te reconoce como alumno y es la forma de identificarte dentro del Instituto.
        </p>
        <div className="alert">
          <h4>ALERTA</h4>
          <p>Ningún empleado del Instituto está facultado para solicitar datos personales por redes sociales. <strong>¡DENUNCIA!</strong></p>
        </div>
      </>
    );
  };

  return (
    <>
      <div className="top-nav">
        <ul>
          <li className={`nav-item ${activeSection === "Inicio" ? "active-orange" : ""}`} onClick={() => { resetState(); setActiveSection("Inicio"); }}>Inicio</li>
          <li className={`nav-item ${activeSection === "Alumnos" ? "active-orange" : ""}`} onClick={() => { resetState(); setActiveSection("Alumnos"); }}>Alumnos</li>
          {isAdmin && (
            <li className={`nav-item ${activeSection === "Administrador" ? "active-orange" : ""}`} onClick={() => { resetState(); setActiveSection("Administrador"); }}>Administrador</li>
          )}
        </ul>
      </div>

      <div className="main-layout">
        <aside className="sidebar-left">{renderSidebarContent()}</aside>
        <main className="main-content">{renderMainContent()}</main>
        <aside className="sidebar-right">
          <h4>Accesos rápidos</h4>
          <ul>
            <li><a href="#">Agenda escolar</a></li>
            <li><a href="#">Horarios de clase</a></li>
            <li><a href="#">Horarios de ETS</a></li>
          </ul>
        </aside>
      </div>
    </>
  );
};

export default MainPage;
