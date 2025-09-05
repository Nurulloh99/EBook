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
import UpdateBookPage from './components/books/UpdateBookPage';
import GenreManagerPage from './components/genre/GenrePage';
import LanguageManagerPage from './components/language/LanguageManagerPage';
import RoleManagerPage from './components/role/RoleManagerPage'; // ✅ yangi qo‘shildi
import UserManagerPage from './components/user/UserManagerPage'; // ✅ yangi qo‘shildi

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

          {/* Genre Manager */}
          <Route
            path="/genres"
            element={
              <ProtectedRoute>
                <Layout>
                  <GenreManagerPage />
                </Layout>
              </ProtectedRoute>
            }
          />

          {/* Language Manager */}
          <Route
            path="/languages"
            element={
              <ProtectedRoute>
                <Layout>
                  <LanguageManagerPage />
                </Layout>
              </ProtectedRoute>
            }
          />

          {/* Role Manager — hamma foydalanuvchi uchun */}
          <Route
            path="/roles"
            element={
              <ProtectedRoute>
                <Layout>
                  <RoleManagerPage />
                </Layout>
              </ProtectedRoute>
            }
          />

          {/* User Manager — Admin va SuperAdmin uchun */}
          <Route
            path="/users"
            element={
              <ProtectedRoute roles={['Admin', 'SuperAdmin']}>
                <Layout>
                  <UserManagerPage />
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
