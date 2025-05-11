import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import LoginSignup from "./Pages/LoginSignup";
import MainPage from './Pages/MainPage';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/login' element={<LoginSignup/>} />
        <Route path='/' element={<LoginSignup/>} />
        <Route path='/MenuPrincipal' element={< MainPage/>}/>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
