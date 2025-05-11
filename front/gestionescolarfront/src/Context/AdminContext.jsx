import React, { createContext, useState } from "react";

export const AdminContext = createContext();

export const AdminProvider = ({ children }) => {
  const [selectedRequest, setSelectedRequest] = useState(null);

  return (
    <AdminContext.Provider value={{ selectedRequest, setSelectedRequest }}>
      {children}
    </AdminContext.Provider>
  );
};
