import api from './api';

export const languageService = {
  async getAllLanguages() {
    return await api.get('/languages');
  },

  async getLanguageById(languageId: number) {
    return await api.get(`/languages/${languageId}`);
  },

  async createLanguage(data: { languageName: string }) {
    return await api.post('/admin/languages', data);
  },

  async deleteLanguage(languageId: number) {
    return await api.delete(`/admin/languages/${languageId}`);
  },
  async updateLanguage(data: { languageId: number; languageName: string }) {
    return await api.put(`/admin/languages/${data.languageId}`, data);
  },
};