// components/user/UserManagerPage.tsx
import React, { useEffect, useState } from "react";
import { userService } from "../../services/userService";
import { AlertCircle, CheckCircle, Trash2, User } from "lucide-react";

interface AppUser {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  roleName: string;
}

const UserManagerPage: React.FC = () => {
  const [users, setUsers] = useState<AppUser[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [roleFilter, setRoleFilter] = useState("User"); // default filter

  useEffect(() => {
    loadUsers();
  }, [roleFilter]);

  const loadUsers = async () => {
    setLoading(true);
    setError("");
    try {
      const res = await userService.getUsersByRole(roleFilter);
      setUsers(res.data.data);
    } catch (err: any) {
      setError(err.response?.data?.message || "Failed to load users");
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (userId: number) => {
    if (!window.confirm("Are you sure you want to delete this user?")) return;
    try {
      await userService.deleteUser(userId);
      setSuccess("User deleted successfully");
      setUsers(users.filter(u => u.userId !== userId));
    } catch (err: any) {
      setError(err.response?.data?.message || "Failed to delete user");
    }
  };

  const handleRoleChange = async (userId: number, newRole: string) => {
    try {
      await userService.updateUserRole(userId, newRole);
      setUsers(prev =>
        prev.map(u => (u.userId === userId ? { ...u, roleName: newRole } : u))
      );
      setSuccess("User role updated successfully");
      setError("");
    } catch (err: any) {
      setError(err.response?.data?.message || "Failed to update role");
      setSuccess("");
    }
  };

  return (
    <div className="max-w-5xl mx-auto space-y-6">
      <div className="text-center">
        <User className="w-12 h-12 text-amber-600 mx-auto" />
        <h1 className="mt-4 text-3xl font-bold text-gray-900">Manage Users</h1>
      </div>

      {/* Role Filter */}
      <div className="flex gap-4">
        {["User", "Admin", "SuperAdmin"].map(role => (
          <button
            key={role}
            onClick={() => setRoleFilter(role)}
            className={`px-4 py-2 rounded-lg border ${
              roleFilter === role
                ? "bg-amber-600 text-white"
                : "bg-white text-gray-700 border-gray-300"
            }`}
          >
            {role}s
          </button>
        ))}
      </div>

      {/* Messages */}
      {error && (
        <div className="bg-red-50 border border-red-200 rounded-lg p-4 flex items-center space-x-2 text-red-700">
          <AlertCircle className="w-5 h-5" />
          <span>{error}</span>
        </div>
      )}
      {success && (
        <div className="bg-green-50 border border-green-200 rounded-lg p-4 flex items-center space-x-2 text-green-700">
          <CheckCircle className="w-5 h-5" />
          <span>{success}</span>
        </div>
      )}

      {/* Users List */}
      <div className="bg-white rounded-xl shadow-md border border-gray-200 p-6">
        <ul className="space-y-3">
          {users.map(user => (
            <li
              key={user.userId}
              className="flex justify-between items-center border p-3 rounded-lg"
            >
              <div>
                <p className="font-semibold">{user.firstName} {user.lastName}</p>
                <p className="text-sm text-gray-500">{user.email}</p>

                {/* Role select */}
                <select
                  value={user.roleName}
                  onChange={(e) => handleRoleChange(user.userId, e.target.value)}
                  className="mt-1 border border-gray-300 rounded-lg px-2 py-1 text-sm"
                >
                  <option value="User">User</option>
                  <option value="Admin">Admin</option>
                  <option value="SuperAdmin">SuperAdmin</option>
                </select>
              </div>

              <div className="flex gap-2">
                <button
                  onClick={() => handleDelete(user.userId)}
                  className="px-3 py-1 bg-red-100 text-red-700 rounded-lg hover:bg-red-200 flex items-center space-x-1"
                >
                  <Trash2 className="w-4 h-4" />
                  <span>Delete</span>
                </button>
              </div>
            </li>
          ))}
        </ul>
        {users.length === 0 && (
          <p className="text-gray-500 text-center">No users found.</p>
        )}
      </div>
    </div>
  );
};

export default UserManagerPage;
