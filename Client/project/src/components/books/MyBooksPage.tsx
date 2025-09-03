import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { bookService } from "../../services/bookService";
import { Book } from "../../types";
import {
  BookOpen,
  Plus,
  Edit,
  Trash2,
  Eye,
  Calendar,
  FileText,
} from "lucide-react";

const MyBooksPage: React.FC = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadUserBooks();
  }, []);

  const loadUserBooks = async () => {
    try {
      const response = await bookService.getUserBooks();
      setBooks(response.data);
    } catch (error) {
      console.error("Failed to load user books:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteBook = async (bookId: number) => {
    if (window.confirm("Are you sure you want to delete this book?")) {
      try {
        await bookService.deleteBook(bookId);
        setBooks(books.filter((book) => book.bookId !== bookId));
      } catch (error) {
        console.error("Failed to delete book:", error);
      }
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-96">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-amber-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">My Books</h1>
          <p className="text-gray-600 mt-2">Manage your uploaded books</p>
        </div>

        <Link
          to="/add-book"
          className="flex items-center space-x-2 bg-amber-600 text-white px-6 py-3 rounded-lg hover:bg-amber-700 transition-colors"
        >
          <Plus className="w-5 h-5" />
          <span>Add New Book</span>
        </Link>
      </div>

      {books.length > 0 ? (
        <div className="grid grid-cols-1 gap-6">
          {books.map((book) => (
            <div
              key={book.bookId}
              className="bg-white rounded-xl shadow-sm border border-amber-100 hover:shadow-md transition-shadow"
            >
              <div className="p-6 flex items-center space-x-6">
                <img
                  src={
                    book.thumbnaliUrl ||
                    "https://images.pexels.com/photos/159711/books-bookstore-book-reading-159711.jpeg?auto=compress&cs=tinysrgb&w=200"
                  }
                  alt={book.title}
                  className="w-20 h-28 object-cover rounded-lg shadow-sm"
                />

                <div className="flex-1 min-w-0">
                  <h3 className="text-xl font-semibold text-gray-900 truncate">
                    {book.title}
                  </h3>
                  <p className="text-gray-600 mt-1">by {book.author}</p>

                  <div className="flex items-center space-x-4 mt-2 text-sm text-gray-500">
                    <div className="flex items-center space-x-1">
                      <Calendar className="w-4 h-4" />
                      <span>{new Date(book.published).getFullYear()}</span>
                    </div>
                    <div className="flex items-center space-x-1">
                      <FileText className="w-4 h-4" />
                      <span>{book.pages} pages</span>
                    </div>
                  </div>

                  <p className="text-gray-700 mt-3 line-clamp-2">
                    {book.description}
                  </p>
                </div>

                <div className="flex items-center space-x-2">
                  {/* View details */}
                  <Link
                    to={`/books/${book.bookId}`}
                    className="p-2 text-amber-600 hover:bg-amber-50 rounded-lg transition-colors"
                    title="View Details"
                  >
                    <Eye className="w-5 h-5" />
                  </Link>

                  {/* Edit book → Update page ochiladi */}
                  <Link
                    to={`/update-book/${book.bookId}`}
                    className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                    title="Edit Book"
                  >
                    <Edit className="w-5 h-5" />
                  </Link>

                  {/* Delete */}
                  <button
                    onClick={() => handleDeleteBook(book.bookId)}
                    className="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                    title="Delete Book"
                  >
                    <Trash2 className="w-5 h-5" />
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="text-center py-16">
          <BookOpen className="mx-auto h-16 w-16 text-gray-400" />
          <h3 className="mt-4 text-lg font-medium text-gray-900">
            No books uploaded
          </h3>
          <p className="mt-2 text-gray-500">
            Get started by uploading your first book.
          </p>
          <Link
            to="/add-book"
            className="mt-6 inline-flex items-center space-x-2 bg-amber-600 text-white px-6 py-3 rounded-lg hover:bg-amber-700 transition-colors"
          >
            <Plus className="w-5 h-5" />
            <span>Upload Your First Book</span>
          </Link>
        </div>
      )}
    </div>
  );
};

export default MyBooksPage;
