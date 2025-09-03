import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { bookService } from '../../services/bookService';
import { reviewService } from '../../services/reviewService';
import { Book, Review } from '../../types';
import { ArrowLeft, Calendar, User, FileText, Star, MessageSquare, Download } from 'lucide-react';
import ReviewForm from './ReviewForm';

const BookDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [book, setBook] = useState<Book | null>(null);
  const [reviews, setReviews] = useState<Review[]>([]);
  const [loading, setLoading] = useState(true);
  const [showReviewForm, setShowReviewForm] = useState(false);

  useEffect(() => {
    if (id) {
      loadBookDetails();
      loadReviews();
    }
  }, [id]);

  const loadBookDetails = async () => {
    try {
      const response = await bookService.getBookById(Number(id));
      setBook(response.data);
    } catch (error) {
      console.error('Failed to load book details:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadReviews = async () => {
    try {
      const response = await reviewService.getReviewsByBook(Number(id));
      setReviews(response.data);
    } catch (error) {
      console.error('Failed to load reviews:', error);
    }
  };

  const handleReviewAdded = () => {
    setShowReviewForm(false);
    loadReviews();
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-96">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-amber-600"></div>
      </div>
    );
  }

  if (!book) {
    return (
      <div className="text-center py-12">
        <h2 className="text-2xl font-bold text-gray-900">Book not found</h2>
        <Link to="/books" className="mt-4 text-amber-600 hover:text-amber-700">
          ← Back to books
        </Link>
      </div>
    );
  }

  const averageRating = reviews.length > 0 
    ? reviews.reduce((sum, review) => sum + review.rating, 0) / reviews.length 
    : 0;

  return (
    <div className="space-y-8">
      {/* Back Button */}
      <Link
        to="/books"
        className="inline-flex items-center space-x-2 text-amber-600 hover:text-amber-700 transition-colors"
      >
        <ArrowLeft className="w-4 h-4" />
        <span>Back to books</span>
      </Link>

      {/* Book Details */}
      <div className="bg-white rounded-xl shadow-lg border border-amber-100 overflow-hidden">
        <div className="md:flex">
          <div className="md:w-1/3">
            <img
              src={book.thumbnaliUrl || 'https://images.pexels.com/photos/159711/books-bookstore-book-reading-159711.jpeg?auto=compress&cs=tinysrgb&w=600'}
              alt={book.title}
              className="w-full h-64 md:h-full object-cover"
            />
          </div>
          
          <div className="md:w-2/3 p-8">
            <div className="space-y-4">
              <div>
                <h1 className="text-3xl font-bold text-gray-900">{book.title}</h1>
                <div className="flex items-center space-x-4 mt-2 text-gray-600">
                  <div className="flex items-center space-x-1">
                    <User className="w-4 h-4" />
                    <span>{book.author}</span>
                  </div>
                  <div className="flex items-center space-x-1">
                    <Calendar className="w-4 h-4" />
                    <span>{new Date(book.published).getFullYear()}</span>
                  </div>
                  <div className="flex items-center space-x-1">
                    <FileText className="w-4 h-4" />
                    <span>{book.pages} pages</span>
                  </div>
                </div>
              </div>

              {/* Rating */}
              <div className="flex items-center space-x-4">
                <div className="flex items-center space-x-1">
                  {[1, 2, 3, 4, 5].map((star) => (
                    <Star
                      key={star}
                      className={`w-5 h-5 ${
                        star <= averageRating
                          ? 'text-yellow-400 fill-current'
                          : 'text-gray-300'
                      }`}
                    />
                  ))}
                  <span className="text-gray-600 ml-2">
                    {averageRating.toFixed(1)} ({reviews.length} reviews)
                  </span>
                </div>
              </div>

              {/* Description */}
              <div>
                <h3 className="text-lg font-semibold text-gray-900 mb-2">Description</h3>
                <p className="text-gray-700 leading-relaxed">{book.description}</p>
              </div>

              {/* Action Buttons */}
              <div className="flex flex-wrap gap-3 pt-4">
                {book.bookUrl && (
                  <a
                    href={book.bookUrl}
                    target="_blank"
                    rel="noopener noreferrer"
                    className="flex items-center space-x-2 bg-emerald-600 text-white px-6 py-2 rounded-lg hover:bg-emerald-700 transition-colors"
                  >
                    <Download className="w-4 h-4" />
                    <span>Read Book</span>
                  </a>
                )}
                <button
                  onClick={() => setShowReviewForm(!showReviewForm)}
                  className="flex items-center space-x-2 bg-amber-600 text-white px-6 py-2 rounded-lg hover:bg-amber-700 transition-colors"
                >
                  <MessageSquare className="w-4 h-4" />
                  <span>Write Review</span>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Review Form */}
      {showReviewForm && (
        <div className="bg-white rounded-xl shadow-lg border border-amber-100 p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Write a Review</h3>
          <ReviewForm bookId={book.bookId} onReviewAdded={handleReviewAdded} />
        </div>
      )}

      {/* Reviews */}
      <div className="bg-white rounded-xl shadow-lg border border-amber-100 p-6">
        <h3 className="text-lg font-semibold text-gray-900 mb-6">
          Reviews ({reviews.length})
        </h3>
        
        {reviews.length > 0 ? (
          <div className="space-y-6">
            {reviews.map((review) => (
              <div key={review.reviewId} className="border-b border-gray-100 last:border-b-0 pb-6 last:pb-0">
                <div className="flex items-center space-x-2 mb-2">
                  <div className="flex items-center">
                    {[1, 2, 3, 4, 5].map((star) => (
                      <Star
                        key={star}
                        className={`w-4 h-4 ${
                          star <= review.rating
                            ? 'text-yellow-400 fill-current'
                            : 'text-gray-300'
                        }`}
                      />
                    ))}
                  </div>
                  <span className="text-sm text-gray-600">{review.rating}/5</span>
                </div>
                <p className="text-gray-700">{review.content}</p>
              </div>
            ))}
          </div>
        ) : (
          <p className="text-gray-500 text-center py-8">
            No reviews yet. Be the first to review this book!
          </p>
        )}
      </div>
    </div>
  );
};

export default BookDetailPage;