import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { AlertCircle, CheckCircle, Shield, Trash2, PlusCircle } from "lucide-react";
import { roleService } from "../../services/roleService";

interface Role {
  roleId: number;
  roleName: string;
  roleDescription: string;
}

interface RoleFormData {
  roleName: string;
  roleDescription: string;
}

const RoleManagerPage: React.FC = () => {
  const [roles, setRoles] = useState<Role[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<RoleFormData>();

  useEffect(() => {
    loadRoles();
  }, []);

  const loadRoles = async () => {
    try {
      const res = await roleService.getAllRoles();
      setRoles(res.data);
    } catch (err) {
      console.error(err);
    }
  };

  const onSubmit = async (data: RoleFormData) => {
    setLoading(true);
    setError("");
    try {
      await roleService.createRole(data);
      setSuccess("Role created successfully!");
      reset();
      loadRoles();
    } catch (err: any) {
      setError(err.response?.data?.message || "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this role?")) return;
    try {
      await roleService.deleteRole(id);
      setRoles((prev) => prev.filter((r) => r.roleId !== id));
    } catch (err: any) {
      setError(err.response?.data?.message || "Failed to delete role");
    }
  };

  return (
    <div className="max-w-4xl mx-auto space-y-8">
      <div className="text-center">
        <Shield className="w-12 h-12 text-amber-600 mx-auto" />
        <h1 className="mt-4 text-3xl font-bold text-gray-900">Manage Roles</h1>
        <p className="mt-2 text-gray-600">Add or delete user roles</p>
      </div>

      {/* ✅ Errors & Success */}
      <div className="bg-white rounded-xl shadow-lg border border-amber-100 p-8">
        {error && (
          <div className="mb-6 bg-red-50 border border-red-200 rounded-lg p-4 flex items-center space-x-2 text-red-700">
            <AlertCircle className="w-5 h-5" />
            <span>{error}</span>
          </div>
        )}
        {success && (
          <div className="mb-6 bg-green-50 border border-green-200 rounded-lg p-4 flex items-center space-x-2 text-green-700">
            <CheckCircle className="w-5 h-5" />
            <span>{success}</span>
          </div>
        )}

        {/* ✅ Role Form */}
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-700">
              Role Name *
            </label>
            <input
              {...register("roleName", { required: "Role name is required" })}
              type="text"
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
              placeholder="Enter role name"
            />
            {errors.roleName && (
              <p className="mt-1 text-sm text-red-600">{errors.roleName.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">
              Description *
            </label>
            <textarea
              {...register("roleDescription", {
                required: "Description is required",
              })}
              rows={3}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent resize-none"
              placeholder="Enter role description"
            />
            {errors.roleDescription && (
              <p className="mt-1 text-sm text-red-600">
                {errors.roleDescription.message}
              </p>
            )}
          </div>

          <div className="flex justify-end pt-6">
            <button
              type="submit"
              disabled={loading}
              className="px-6 py-2 bg-amber-600 text-white rounded-lg hover:bg-amber-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-amber-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors flex items-center space-x-2"
            >
              {loading ? (
                <span>Saving...</span>
              ) : (
                <>
                  <PlusCircle className="w-4 h-4" />
                  <span>Add Role</span>
                </>
              )}
            </button>
          </div>
        </form>
      </div>

      {/* ✅ Role List */}
      <div className="bg-white rounded-xl shadow-md border border-gray-200 p-6">
        <h2 className="text-lg font-semibold mb-4">Existing Roles</h2>
        <ul className="space-y-3">
          {roles.map((role) => (
            <li
              key={role.roleId}
              className="flex justify-between items-center border p-3 rounded-lg"
            >
              <div>
                <p className="font-semibold">{role.roleName}</p>
                <p className="text-sm text-gray-500">{role.roleDescription}</p>
              </div>
              <button
                onClick={() => handleDelete(role.roleId)}
                className="px-3 py-1 bg-red-100 text-red-700 rounded-lg hover:bg-red-200 flex items-center space-x-1"
              >
                <Trash2 className="w-4 h-4" />
                <span>Delete</span>
              </button>
            </li>
          ))}
        </ul>
        {roles.length === 0 && (
          <p className="text-gray-500 text-center">No roles found.</p>
        )}
      </div>
    </div>
  );
};

export default RoleManagerPage;
