import api from './api';
import { BookCreateData } from '../types';

export const bookService = {
  async getAllBooks(skip?: number, take?: number) {
    const params = new URLSearchParams();
    if (skip !== undefined) params.append('skip', skip.toString());
    if (take !== undefined) params.append('take', take.toString());
    
    return await api.get(`/books?${params.toString()}`);
  },

  async getBookById(bookId: number) {
    return await api.get(`/books/${bookId}`);
  },

  async createBook(data: BookCreateData) {
  const formData = new FormData();

  formData.append("Author", data.author);
  formData.append("Title", data.title);
  formData.append("Description", data.description);
  formData.append("Published", data.published); // string bo‘lishi kerak, masalan "2025-09-02"
  formData.append("Pages", data.pages.toString());
  formData.append("LanguageId", data.languageId.toString());
  formData.append("GenreId", data.genreId.toString());

  formData.append("book", data.book);
  formData.append("image", data.thumbnali);

  return await api.post("/admin/books", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
},


  async updateBook(bookId: number, data: any) {
  return await api.put(`/admin/books/${bookId}`, data, {
    headers: {
      "Content-Type": "application/json",
    },
  });
},


  async deleteBook(bookId: number) {
    return await api.delete(`/admin/books/${bookId}`);
  },

  async getBooksByLanguage(languageId: number) {
    return await api.get(`/books/by-language/${languageId}`);
  },

  async getBooksByGenre(genreId: number) {
    return await api.get(`/books/by-genre/${genreId}`);
  },

  async searchBooks(keyword: string) {
    return await api.get(`/books/search?keyword=${encodeURIComponent(keyword)}`);
  },

  async getUserBooks() {
    return await api.get('/admin/users/me/books');
  },
};