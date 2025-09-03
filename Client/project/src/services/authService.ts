import api from './api';
import { LoginCredentials, SignUpData } from '../types';

export const authService = {
  async login(credentials: LoginCredentials) {
    return await api.post('/auth/login', credentials);
  },

  async signup(data: SignUpData) {
    return await api.post('/auth/signup', data);
  },

  async sendCode(email: string) {
    return await api.post(`/auth/send-code?email=${encodeURIComponent(email)}`);
  },

  async confirmCode(email: string, code: string) {
    return await api.post(`/auth/confirm-code?email=${encodeURIComponent(email)}&code=${code}`);
  },

  async forgotPassword(email: string, newPassword: string, confirmCode: string) {
    return await api.post(
      `/auth/forgot-password?email=${encodeURIComponent(email)}&newPassword=${encodeURIComponent(newPassword)}&confirmCode=${confirmCode}`
    );
  },

  async refreshToken(refreshToken: string) {
    return await api.post('/auth/refresh-token', { refreshToken });
  },

  async logout(refreshToken: string) {
    return await api.delete(`/auth/logout?refreshToken=${refreshToken}`);
  },
};