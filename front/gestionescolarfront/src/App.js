import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import LoginSignup from "./Pages/LoginSignup";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/login' element={<LoginSignup/>} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
