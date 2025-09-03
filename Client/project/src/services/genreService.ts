import api from './api';

export const genreService = {
  async getAllGenres() {
    return await api.get('/genres');
  },

  async getGenreById(genreId: number) {
    return await api.get(`/genres/${genreId}`);
  },

  async createGenre(data: { genreName: string; genreDescription: string }) {
    return await api.post('/admin/genres', data);
  },

  async updateGenre(data: { genreId: number; genreName: string; genreDescription: string }) {
    return await api.patch(`/admin/genres/${data.genreId}`, data);
  },

  async deleteGenre(genreId: number) {
    return await api.delete(`/admin/genres/${genreId}`);
  },

  async searchGenres(keyword: string) {
    return await api.get(`/genres/search?keyword=${encodeURIComponent(keyword)}`);
  },
};