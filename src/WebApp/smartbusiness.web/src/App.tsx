import React from "react";
import { Route, Routes } from "react-router-dom";
import "./App.css";
// import TableComponent from './components/TableComponent.tsx'

import { HomePage } from "./pages/HomePage";
import { LoginPage } from "./pages/LoginPage";
import { RegisterPage } from "./pages/RegisterPage";

const App: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
    </Routes>
  );
};

// function App() {
//     return (
//       <div>
//         <LoginSignup/>
//       </div>

//       // <div>
//       //     <h1>API TEST</h1>
//       //     <TableComponent />
//       // </div>
//   )
// }

export default App;
