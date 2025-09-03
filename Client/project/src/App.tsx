import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import Layout from './components/layout/Layout';
import ProtectedRoute from './components/common/ProtectedRoute';
import HomePage from './components/common/HomePage';
import LoginPage from './components/auth/LoginPage';
import SignUpPage from './components/auth/SignUpPage';
import VerifyCodePage from './components/auth/VerifyCodePage';
import ForgotPasswordPage from './components/auth/ForgotPasswordPage';
import BookListPage from './components/books/BookListPage';
import BookDetailPage from './components/books/BookDetailPage';
import AddBookPage from './components/books/AddBookPage';
import MyBooksPage from './components/books/MyBooksPage';
import UpdateBookPage from './components/books/UpdateBookPage'; // << IMPORTANT

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          {/* Public Routes */}
          <Route path="/" element={<Layout><HomePage /></Layout>} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/signup" element={<SignUpPage />} />
          <Route path="/verify-code" element={<VerifyCodePage />} />
          <Route path="/forgot-password" element={<ForgotPasswordPage />} />
          
          {/* Protected Routes */}
          <Route
            path="/books"
            element={
              <ProtectedRoute>
                <Layout>
                  <BookListPage />
                </Layout>
              </ProtectedRoute>
            }
          />
          <Route
            path="/books/:id"
            element={
              <ProtectedRoute>
                <Layout>
                  <BookDetailPage />
                </Layout>
              </ProtectedRoute>
            }
          />
          <Route
            path="/add-book"
            element={
              <ProtectedRoute>
                <Layout>
                  <AddBookPage />
                </Layout>
              </ProtectedRoute>
            }
          />
          <Route
            path="/my-books"
            element={
              <ProtectedRoute>
                <Layout>
                  <MyBooksPage />
                </Layout>
              </ProtectedRoute>
            }
          />
          <Route
            path="/update-book/:id"
            element={
              <ProtectedRoute>
                <Layout>
                  <UpdateBookPage />
                </Layout>
              </ProtectedRoute>
            }
          />
          
          {/* Redirect unknown routes */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
