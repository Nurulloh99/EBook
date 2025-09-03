import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { bookService } from '../../services/bookService';
import { genreService } from '../../services/genreService';
import { languageService } from '../../services/languageService';
import { BookUpdateDto, Genre, Language } from '../../types';
import { BookOpen, AlertCircle, CheckCircle } from 'lucide-react';

const UpdateBookPage: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const [error, setError] = useState<string>('');
  const [success, setSuccess] = useState<string>('');
  const [loading, setLoading] = useState(false);
  const [initialLoading, setInitialLoading] = useState(true);
  const [genres, setGenres] = useState<Genre[]>([]);
  const [languages, setLanguages] = useState<Language[]>([]);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<BookUpdateDto>();

  useEffect(() => {
    if (id) {
      loadBook(+id);
      loadGenresAndLanguages();
    }
  }, [id]);

  const loadBook = async (bookId: number) => {
    try {
      const res = await bookService.getBookById(bookId);
      reset({
        bookId: res.data.bookId,
        title: res.data.title,
        author: res.data.author,
        description: res.data.description,
        published: res.data.published,
        pages: res.data.pages,
        genreId: res.data.genreId,
        languageId: res.data.languageId,
      });
    } catch (err) {
      setError('Failed to load book details');
    } finally {
      setInitialLoading(false);
    }
  };

  const loadGenresAndLanguages = async () => {
    try {
      const [genresRes, languagesRes] = await Promise.all([
        genreService.getAllGenres(),
        languageService.getAllLanguages(),
      ]);
      setGenres(genresRes.data);
      setLanguages(languagesRes.data);
    } catch (err) {
      console.error('Failed to load genres/languages', err);
    }
  };

  const onSubmit = async (data: BookUpdateDto) => {
    setLoading(true);
    setError('');
    try {
      await bookService.updateBook(data.bookId, data);
      setSuccess('Book updated successfully!');
      setTimeout(() => {
        navigate('/books');
      }, 2000);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to update book');
    } finally {
      setLoading(false);
    }
  };

  if (initialLoading) {
    return <p className="text-center mt-10">Loading book data...</p>;
  }

  return (
    <div className="max-w-4xl mx-auto space-y-8">
      <div className="text-center">
        <BookOpen className="w-12 h-12 text-amber-600 mx-auto" />
        <h1 className="mt-4 text-3xl font-bold text-gray-900">Update Book</h1>
        <p className="mt-2 text-gray-600">Edit the details of your book</p>
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
          <input type="hidden" {...register('bookId')} />

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700">
                Book Title *
              </label>
              <input
                {...register('title', { required: 'Title is required' })}
                type="text"
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg 
                  focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
              />
              {errors.title && (
                <p className="mt-1 text-sm text-red-600">{errors.title.message}</p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">
                Author *
              </label>
              <input
                {...register('author', { required: 'Author is required' })}
                type="text"
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg 
                  focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
              />
              {errors.author && (
                <p className="mt-1 text-sm text-red-600">{errors.author.message}</p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">
                Publication Date *
              </label>
              <input
                {...register('published', { required: 'Publication date is required' })}
                type="date"
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg 
                  focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
              />
              {errors.published && (
                <p className="mt-1 text-sm text-red-600">{errors.published.message}</p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">
                Number of Pages *
              </label>
              <input
                {...register('pages', {
                  required: 'Number of pages is required',
                  min: { value: 1, message: 'Pages must be at least 1' },
                })}
                type="number"
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg 
                  focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
              />
              {errors.pages && (
                <p className="mt-1 text-sm text-red-600">{errors.pages.message}</p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">
                Genre *
              </label>
              <select
                {...register('genreId', { required: 'Genre is required' })}
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg 
                  focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
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
              <label className="block text-sm font-medium text-gray-700">
                Language *
              </label>
              <select
                {...register('languageId', { required: 'Language is required' })}
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg 
                  focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
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
            <label className="block text-sm font-medium text-gray-700">
              Description *
            </label>
            <textarea
              {...register('description', { required: 'Description is required' })}
              rows={4}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg 
                focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent resize-none"
            />
            {errors.description && (
              <p className="mt-1 text-sm text-red-600">{errors.description.message}</p>
            )}
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
              className="px-6 py-2 bg-amber-600 text-white rounded-lg hover:bg-amber-700 
                focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-amber-500 
                disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
            >
              {loading ? 'Updating...' : 'Update Book'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default UpdateBookPage;
