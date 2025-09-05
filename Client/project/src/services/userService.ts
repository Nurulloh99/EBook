// services/userService.ts
import api from "./api"; // sizning axios instance

export const userService = {
  async getUsersByRole(role: string) {
    return await api.get(`/admin/users/by-role/${role}`);
  },

  async deleteUser(userId: number) {
    return await api.delete(`/admin/users/${userId}`);
  },

  async updateUserRole(userId: number, userRole: string) {
    return await api.patch(`/admin/roles/${userId}`, null, { params: { userRole } });
  },
};
