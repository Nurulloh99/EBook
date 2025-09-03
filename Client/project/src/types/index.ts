export interface User {
  userId: number;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phoneNumber: string;
  roleId: number;
  roleName: string;
}

// types.ts
export interface BookUpdateDto {
  bookId: number;
  title: string;
  author: string;
  description: string;
  published: string; // API JSON string qaytaradi
  pages: number;
  genreId: number;
  languageId: number;
}


export interface Book {
  bookId: number;
  title: string;
  author: string;
  description: string;
  published: string;
  pages: number;
  bookUrl: string;
  thumbnaliUrl: string;
  reviews?: Review[];
}

export interface BookCreateData {
  title: string;
  author: string;
  description: string;
  published: string;
  pages: number;
  languageId: number;
  genreId: number;
  book: File;
  thumbnali: File;
}

export interface Review {
  reviewId: number;
  content: string;
  rating: number;
}

export interface Genre {
  genreId: number;
  genreName: string;
  genreDescription: string;
}

export interface Language {
  languageId: number;
  languageName: string;
}

export interface AuthTokens {
  accessToken: string;
  refreshToken: string;
}

export interface LoginCredentials {
  userName: string;
  password: string;
}

export interface SignUpData {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
  phoneNumber: string;
}