import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { bookService } from '../../services/bookService';
import { genreService } from '../../services/genreService';
import { languageService } from '../../services/languageService';
import { Book, Genre, Language } from '../../types';
import { Search,  Grid, List, Calendar, User, Eye, BookOpen } from 'lucide-react';


const BookListPage: React.FC = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [genres, setGenres] = useState<Genre[]>([]);
  const [languages, setLanguages] = useState<Language[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedGenre, setSelectedGenre] = useState<number | null>(null);
  const [selectedLanguage, setSelectedLanguage] = useState<number | null>(null);
  const [viewMode, setViewMode] = useState<'grid' | 'list'>('grid');
  const [currentPage, setCurrentPage] = useState(1);
  const booksPerPage = 12;

  useEffect(() => {
    loadInitialData();
  }, []);

  useEffect(() => {
    if (searchTerm) {
      searchBooks();
    } else if (selectedGenre) {
      loadBooksByGenre();
    } else if (selectedLanguage) {
      loadBooksByLanguage();
    } else {
      loadBooks();
    }
  }, [searchTerm, selectedGenre, selectedLanguage, currentPage]);

  const loadInitialData = async () => {
    try {
      const [booksRes, genresRes, languagesRes] = await Promise.all([
        bookService.getAllBooks(0, booksPerPage),
        genreService.getAllGenres(),
        languageService.getAllLanguages(),
      ]);
      
      setBooks(booksRes.data);
      setGenres(genresRes.data);
      setLanguages(languagesRes.data);
    } catch (error) {
      console.error('Failed to load data:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadBooks = async () => {
    try {
      const skip = (currentPage - 1) * booksPerPage;
      const response = await bookService.getAllBooks(skip, booksPerPage);
      setBooks(response.data.items);
    } catch (error) {
      console.error('Failed to load books:', error);
    }
  };

  const searchBooks = async () => {
    try {
      const response = await bookService.searchBooks(searchTerm);
      setBooks(response.data);
    } catch (error) {
      console.error('Failed to search books:', error);
    }
  };

  const loadBooksByGenre = async () => {
    if (!selectedGenre) return;
    try {
      const response = await bookService.getBooksByGenre(selectedGenre);
      setBooks(response.data);
    } catch (error) {
      console.error('Failed to load books by genre:', error);
    }
  };

  const loadBooksByLanguage = async () => {
    if (!selectedLanguage) return;
    try {
      const response = await bookService.getBooksByLanguage(selectedLanguage);
      setBooks(response.data);
    } catch (error) {
      console.error('Failed to load books by language:', error);
    }
  };

  const clearFilters = () => {
    setSearchTerm('');
    setSelectedGenre(null);
    setSelectedLanguage(null);
    setCurrentPage(1);
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
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Book Library</h1>
          <p className="text-gray-600 mt-2">Discover your next great read</p>
        </div>
        
        <div className="flex items-center space-x-2">
          <button
            onClick={() => setViewMode('grid')}
            className={`p-2 rounded-lg transition-colors ${
              viewMode === 'grid' ? 'bg-amber-100 text-amber-600' : 'text-gray-400 hover:text-gray-600'
            }`}
          >
            <Grid className="w-5 h-5" />
          </button>
          <button
            onClick={() => setViewMode('list')}
            className={`p-2 rounded-lg transition-colors ${
              viewMode === 'list' ? 'bg-amber-100 text-amber-600' : 'text-gray-400 hover:text-gray-600'
            }`}
          >
            <List className="w-5 h-5" />
          </button>
        </div>
      </div>

      {/* Search and Filters */}
      <div className="bg-white p-6 rounded-xl shadow-sm border border-amber-100">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div className="md:col-span-2">
            <div className="relative">
              <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <Search className="h-5 w-5 text-gray-400" />
              </div>
              <input
                type="text"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
                placeholder="Search books by title..."
              />
            </div>
          </div>

          <div>
            <select
              value={selectedGenre || ''}
              onChange={(e) => setSelectedGenre(e.target.value ? Number(e.target.value) : null)}
              className="block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
            >
              <option value="">All Genres</option>
              {genres.map((genre) => (
                <option key={genre.genreId} value={genre.genreId}>
                  {genre.genreName}
                </option>
              ))}
            </select>
          </div>

          <div>
            <select
              value={selectedLanguage || ''}
              onChange={(e) => setSelectedLanguage(e.target.value ? Number(e.target.value) : null)}
              className="block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
            >
              <option value="">All Languages</option>
              {languages.map((language) => (
                <option key={language.languageId} value={language.languageId}>
                  {language.languageName}
                </option>
              ))}
            </select>
          </div>
        </div>

        {(searchTerm || selectedGenre || selectedLanguage) && (
          <div className="mt-4 flex items-center justify-between">
            <p className="text-sm text-gray-600">
              {books.length} books found
            </p>
            <button
              onClick={clearFilters}
              className="text-sm text-amber-600 hover:text-amber-700"
            >
              Clear filters
            </button>
          </div>
        )}
      </div>

      {/* Books Display */}
      {books.length > 0 ? (
        <div
          className={
            viewMode === 'grid'
              ? 'grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6'
              : 'space-y-4'
          }
        >
          {books.map((book) => (
            <BookCard key={book.bookId} book={book} viewMode={viewMode} />
          ))}
        </div>
      ) : (
        <div className="text-center py-12">
          <BookOpen className="mx-auto h-12 w-12 text-gray-400" />
          <h3 className="mt-2 text-sm font-medium text-gray-900">No books found</h3>
          <p className="mt-1 text-sm text-gray-500">Try adjusting your search or filters.</p>
        </div>
      )}
    </div>
  );
};

interface BookCardProps {
  book: Book;
  viewMode: 'grid' | 'list';
}

const BookCard: React.FC<BookCardProps> = ({ book, viewMode }) => {
  if (viewMode === 'list') {
    return (
      <div className="bg-white rounded-xl shadow-sm border border-amber-100 hover:shadow-md transition-shadow">
        <div className="p-6 flex items-center space-x-4">
          <img
            src={book.thumbnaliUrl || 'https://images.pexels.com/photos/159711/books-bookstore-book-reading-159711.jpeg?auto=compress&cs=tinysrgb&w=200'}
            alt={book.title}
            className="w-16 h-20 object-cover rounded-lg"
          />
          <div className="flex-1 min-w-0">
            <h3 className="text-lg font-semibold text-gray-900 truncate">{book.title}</h3>
            <p className="text-gray-600 flex items-center space-x-1">
              <User className="w-4 h-4" />
              <span>{book.author}</span>
            </p>
            <p className="text-gray-500 flex items-center space-x-1 mt-1">
              <Calendar className="w-4 h-4" />
              <span>{new Date(book.published).getFullYear()}</span>
              <span>•</span>
              <span>{book.pages} pages</span>
            </p>
          </div>
          <div className="flex items-center space-x-2">
            <Link
              to={`/books/${book.bookId}`}
              className="flex items-center space-x-1 bg-amber-600 text-white px-4 py-2 rounded-lg hover:bg-amber-700 transition-colors"
            >
              <Eye className="w-4 h-4" />
              <span>View</span>
            </Link>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-xl shadow-sm border border-amber-100 hover:shadow-lg transition-all duration-300 group overflow-hidden">
      <div className="aspect-[3/4] overflow-hidden">
        <img
          src={book.thumbnaliUrl || 'https://images.pexels.com/photos/159711/books-bookstore-book-reading-159711.jpeg?auto=compress&cs=tinysrgb&w=400'}
          alt={book.title}
          className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
        />
      </div>
      <div className="p-4">
        <h3 className="font-semibold text-gray-900 line-clamp-2 group-hover:text-amber-600 transition-colors">
          {book.title}
        </h3>
        <p className="text-gray-600 text-sm mt-1 flex items-center space-x-1">
          <User className="w-3 h-3" />
          <span>{book.author}</span>
        </p>
        <p className="text-gray-500 text-xs mt-1 flex items-center space-x-1">
          <Calendar className="w-3 h-3" />
          <span>{new Date(book.published).getFullYear()}</span>
          <span>•</span>
          <span>{book.pages} pages</span>
        </p>
        
        <Link
          to={`/books/${book.bookId}`}
          className="mt-3 w-full flex items-center justify-center space-x-1 bg-amber-600 text-white px-4 py-2 rounded-lg hover:bg-amber-700 transition-colors text-sm"
        >
          <Eye className="w-4 h-4" />
          <span>View Details</span>
        </Link>
      </div>
    </div>
  );
};

export default BookListPage;