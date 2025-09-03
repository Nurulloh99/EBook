import api from './api';

export const reviewService = {
  async createReview(data: { content: string; rating: number; bookId: number }) {
    return await api.post('/reviews', data);
  },

  async getReviewById(reviewId: number) {
    return await api.get(`/reviews/${reviewId}`);
  },

  async updateReview(data: { reviewId: number; content: string; rating: number }) {
    return await api.patch(`/reviews/${data.reviewId}`, data);
  },

  async deleteReview(reviewId: number) {
    return await api.delete(`/reviews/${reviewId}`);
  },

  async getReviewsByBook(bookId: number) {
    return await api.get(`/reviews/by-book/${bookId}`);
  },

  async getUserReviews() {
    return await api.get('/reviews/by-user/me');
  },
};