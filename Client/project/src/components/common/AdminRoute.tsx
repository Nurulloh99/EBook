import { Navigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

const AdminRoute = ({ children }: { children: JSX.Element }) => {
  const { user } = useAuth();
  console.log("🔎 AdminRoute check:", user);

  if (!user || (user.roleName !== "Admin" && user.roleName !== "SuperAdmin")) {
    console.log("❌ Access denied:", user?.roleName);
    return <Navigate to="/" replace />;
  }

  console.log("✅ Access granted:", user.roleName);
  return children;
};

export default AdminRoute;
