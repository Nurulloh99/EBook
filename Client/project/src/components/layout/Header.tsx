import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { BookOpen, User, LogOut, Plus, Tags, Languages, Shield } from 'lucide-react';

const Header: React.FC = () => {
  const { user, logout, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    await logout();
    navigate('/');
  };

  // ✅ Faqat admin va super admin
  const isAdmin = user?.roleName === 'Admin' || user?.roleName === 'SuperAdmin';

  return (
    <header className="bg-white shadow-lg border-b border-amber-100">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <Link
            to="/"
            className="flex items-center space-x-2 text-amber-600 hover:text-amber-700 transition-colors"
          >
            <BookOpen className="w-8 h-8" />
            <span className="text-xl font-bold">BookLiteracy</span>
          </Link>

          {isAuthenticated ? (
            <div className="flex items-center space-x-6">
              <nav className="hidden md:flex items-center space-x-6">
                <Link
                  to="/books"
                  className="text-gray-700 hover:text-amber-600 transition-colors"
                >
                  Browse Books
                </Link>

                {/* ✅ faqat adminlar ko‘radi */}
                {isAdmin && (
                  <>
                    <Link
                      to="/genres"
                      className="flex items-center space-x-1 text-gray-700 hover:text-amber-600 transition-colors"
                    >
                      <Tags className="w-4 h-4" />
                      <span>Genres</span>
                    </Link>

                    <Link
                      to="/languages"
                      className="flex items-center space-x-1 text-gray-700 hover:text-amber-600 transition-colors"
                    >
                      <Languages className="w-4 h-4" />
                      <span>Languages</span>
                    </Link>

                    <Link
                      to="/roles"
                      className="flex items-center space-x-1 text-gray-700 hover:text-amber-600 transition-colors"
                    >
                      <Shield className="w-4 h-4" />
                      <span>Roles</span>
                    </Link>

                    {/* ✅ Users tugmasi */}
                    <Link
                      to="/users"
                      className="flex items-center space-x-1 text-gray-700 hover:text-amber-600 transition-colors"
                    >
                      <User className="w-4 h-4" />
                      <span>Users</span>
                    </Link>

                    <Link
                      to="/my-books"
                      className="text-gray-700 hover:text-amber-600 transition-colors"
                    >
                      My Books
                    </Link>
                    <Link
                      to="/add-book"
                      className="flex items-center space-x-1 bg-amber-600 text-white px-4 py-2 rounded-lg hover:bg-amber-700 transition-colors"
                    >
                      <Plus className="w-4 h-4" />
                      <span>Add Book</span>
                    </Link>
                  </>
                )}
              </nav>

              <div className="flex items-center space-x-4">
                <div className="flex items-center space-x-2 text-gray-700">
                  <User className="w-5 h-5" />
                  <span className="hidden sm:inline">
                    {user?.firstName} {user?.lastName}
                  </span>
                </div>
                <button
                  onClick={handleLogout}
                  className="p-2 text-gray-500 hover:text-red-600 transition-colors"
                  title="Logout"
                >
                  <LogOut className="w-5 h-5" />
                </button>
              </div>
            </div>
          ) : (
            <div className="flex items-center space-x-4">
              <Link
                to="/login"
                className="text-gray-700 hover:text-amber-600 transition-colors"
              >
                Login
              </Link>
              <Link
                to="/signup"
                className="bg-amber-600 text-white px-4 py-2 rounded-lg hover:bg-amber-700 transition-colors"
              >
                Sign Up
              </Link>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;
