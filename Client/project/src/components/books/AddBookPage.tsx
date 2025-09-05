import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { bookService } from '../../services/bookService';
import { genreService } from '../../services/genreService';
import { languageService } from '../../services/languageService';
import { BookCreateData, Genre, Language } from '../../types';
import { Upload, BookOpen, AlertCircle, CheckCircle } from 'lucide-react';

const AddBookPage: React.FC = () => {
  const navigate = useNavigate();
  const [error, setError] = useState<string>('');
  const [success, setSuccess] = useState<string>('');
  const [loading, setLoading] = useState(false);
  const [genres, setGenres] = useState<Genre[]>([]);
  const [languages, setLanguages] = useState<Language[]>([]);
  const [bookFile, setBookFile] = useState<File | null>(null);
  const [thumbnailFile, setThumbnailFile] = useState<File | null>(null);
  
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Omit<BookCreateData, 'book' | 'thumbnali'>>();

  useEffect(() => {
    loadGenresAndLanguages();
  }, []);

  const loadGenresAndLanguages = async () => {
    try {
      const [genresRes, languagesRes] = await Promise.all([
        genreService.getAllGenres(),
        languageService.getAllLanguages(),
      ]);
      setGenres(genresRes.data);
      setLanguages(languagesRes.data);
    } catch (error) {
      console.error('Failed to load genres and languages:', error);
    }
  };

  const onSubmit = async (data: Omit<BookCreateData, 'book' | 'thumbnali'>) => {
    if (!bookFile) {
      setError('Please select a book file');
      return;
    }
    if (!thumbnailFile) {
      setError('Please select a thumbnail image');
      return;
    }

    setLoading(true);
    setError('');
    
    try {
      const bookData: BookCreateData = {
        ...data,
        book: bookFile,
        thumbnali: thumbnailFile,
      };
      
      await bookService.createBook(bookData);
      setSuccess('Book uploaded successfully!');
      setTimeout(() => {
        navigate('/books');
      }, 2000);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to upload book');
    } finally {
      setLoading(false);
    }
  };

 const handleBookFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  const file = e.target.files?.[0];
  if (file) {
    // Ruxsat berilgan kitob formatlari
    const allowedExtensions = ['pdf', 'epub', 'mobi', 'txt', 'doc', 'docx', 'rtf', 'odt'];

    // Fayl nomidan kengaytmani olish
    const fileExtension = file.name.split('.').pop()?.toLowerCase();

    if (!fileExtension || !allowedExtensions.includes(fileExtension)) {
      setError(`Book file must be one of: ${allowedExtensions.join(', ').toUpperCase()}`);
      return;
    }

    setBookFile(file);
    setError('');
  }
};


  const handleThumbnailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        setError('Thumbnail must be an image file');
        return;
      }
      setThumbnailFile(file);
      setError('');
    }
  };

  return (
    <div className="max-w-4xl mx-auto space-y-8">
      <div className="text-center">
        <BookOpen className="w-12 h-12 text-amber-600 mx-auto" />
        <h1 className="mt-4 text-3xl font-bold text-gray-900">Add New Book</h1>
        <p className="mt-2 text-gray-600">Share your book with the community</p>
      </div>

      <div className="bg-white rounded-xl shadow-lg border border-amber-100 p-8">
        {error && (
          <div className="mb-6 bg-red-50 border border-red-200 rounded-lg p-4 flex items-center space-x-2 text-red-700">
            <AlertCircle className="w-5 h-5" />
            <span>{error}</span>
          </div>
        )}

        {success && (
          <div className="mb-6 bg-green-50 border border-green-200 rounded-lg p-4 flex items-center space-x-2 text-green-700">
            <CheckCircle className="w-5 h-5" />
            <span>{success}</span>
          </div>
        )}

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label htmlFor="title" className="block text-sm font-medium text-gray-700">
                Book Title *
              </label>
              <input
                {...register('title', { required: 'Title is required' })}
                type="text"
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
                placeholder="Enter book title"
              />
              {errors.title && (
                <p className="mt-1 text-sm text-red-600">{errors.title.message}</p>
              )}
            </div>

            <div>
              <label htmlFor="author" className="block text-sm font-medium text-gray-700">
                Author *
              </label>
              <input
                {...register('author', { required: 'Author is required' })}
                type="text"
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
                placeholder="Enter author name"
              />
              {errors.author && (
                <p className="mt-1 text-sm text-red-600">{errors.author.message}</p>
              )}
            </div>

            <div>
              <label htmlFor="published" className="block text-sm font-medium text-gray-700">
                Publication Date *
              </label>
              <input
                {...register('published', { required: 'Publication date is required' })}
                type="date"
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
              />
              {errors.published && (
                <p className="mt-1 text-sm text-red-600">{errors.published.message}</p>
              )}
            </div>

            <div>
              <label htmlFor="pages" className="block text-sm font-medium text-gray-700">
                Number of Pages *
              </label>
              <input
                {...register('pages', {
                  required: 'Number of pages is required',
                  min: { value: 1, message: 'Pages must be at least 1' },
                })}
                type="number"
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
                placeholder="Enter number of pages"
              />
              {errors.pages && (
                <p className="mt-1 text-sm text-red-600">{errors.pages.message}</p>
              )}
            </div>

            <div>
              <label htmlFor="genreId" className="block text-sm font-medium text-gray-700">
                Genre *
              </label>
              <select
                {...register('genreId', { required: 'Genre is required' })}
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
              >
                <option value="">Select a genre</option>
                {genres.map((genre) => (
                  <option key={genre.genreId} value={genre.genreId}>
                    {genre.genreName}
                  </option>
                ))}
              </select>
              {errors.genreId && (
                <p className="mt-1 text-sm text-red-600">{errors.genreId.message}</p>
              )}
            </div>

            <div>
              <label htmlFor="languageId" className="block text-sm font-medium text-gray-700">
                Language *
              </label>
              <select
                {...register('languageId', { required: 'Language is required' })}
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
              >
                <option value="">Select a language</option>
                {languages.map((language) => (
                  <option key={language.languageId} value={language.languageId}>
                    {language.languageName}
                  </option>
                ))}
              </select>
              {errors.languageId && (
                <p className="mt-1 text-sm text-red-600">{errors.languageId.message}</p>
              )}
            </div>
          </div>

          <div>
            <label htmlFor="description" className="block text-sm font-medium text-gray-700">
              Description *
            </label>
            <textarea
              {...register('description', { required: 'Description is required' })}
              rows={4}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent resize-none"
              placeholder="Enter book description"
            />
            {errors.description && (
              <p className="mt-1 text-sm text-red-600">{errors.description.message}</p>
            )}
          </div>

          {/* File Upload Section */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Book File  *
              </label>
              <div className="border-2 border-dashed border-gray-300 rounded-lg p-6 text-center hover:border-amber-400 transition-colors">
                <input
                  type="file"
                  accept=".pdf,.epub,.mobi,.txt,.doc,.docx,.rtf,.odt"
                  onChange={handleBookFileChange}
                  className="hidden"
                  id="book-upload"
                />
                <label
                  htmlFor="book-upload"
                  className="cursor-pointer flex flex-col items-center space-y-2"
                >
                  <Upload className="w-8 h-8 text-gray-400" />
                  <span className="text-sm text-gray-600">
                    {bookFile ? bookFile.name : 'Click to upload book file'}
                  </span>
                </label>
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Thumbnail Image *
              </label>
              <div className="border-2 border-dashed border-gray-300 rounded-lg p-6 text-center hover:border-amber-400 transition-colors">
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleThumbnailChange}
                  className="hidden"
                  id="thumbnail-upload"
                />
                <label
                  htmlFor="thumbnail-upload"
                  className="cursor-pointer flex flex-col items-center space-y-2"
                >
                  <Upload className="w-8 h-8 text-gray-400" />
                  <span className="text-sm text-gray-600">
                    {thumbnailFile ? thumbnailFile.name : 'Click to upload image'}
                  </span>
                </label>
              </div>
            </div>
          </div>

          <div className="flex justify-end space-x-4 pt-6">
            <button
              type="button"
              onClick={() => navigate('/books')}
              className="px-6 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200 transition-colors"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={loading}
              className="px-6 py-2 bg-amber-600 text-white rounded-lg hover:bg-amber-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-amber-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
            >
              {loading ? 'Uploading...' : 'Upload Book'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddBookPage;