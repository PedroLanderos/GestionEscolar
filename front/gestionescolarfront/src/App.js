import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import LoginSignup from "./Pages/LoginSignup";
import MainPage from './Pages/MainPage';
import Register from './Pages/Register';
import Subjects from './Pages/Subjects';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/login' element={<LoginSignup/>} />
        <Route path='/' element={<LoginSignup/>} />
        <Route path='/MenuPrincipal' element={< MainPage/>}/>
        <Route path="/RegistrarAlumno" element={<Register />} />
        <Route path="/Materias" element={<Subjects />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
