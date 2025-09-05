import api from "./api";

export const roleService = {
  async getAllRoles() {
    return await api.get("/admin/roles"); // ✅ /api/admin prefiks bilan
  },

  async getRoleById(roleId: number) {
    return await api.get(`/admin/roles/${roleId}`);
  },

  async createRole(data: { roleName: string; roleDescription: string }) {
    return await api.post("/admin/roles", data);
  },

  async deleteRole(roleId: number) {
    return await api.delete(`/admin/roles/${roleId}`);
  },

  async updateUserRole(userId: number, userRole: string) {
    return await api.patch(`/admin/roles/${userId}?userRole=${userRole}`);
  },
};
