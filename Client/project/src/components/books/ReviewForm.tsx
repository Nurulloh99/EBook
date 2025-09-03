import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { reviewService } from '../../services/reviewService';
import { Star, AlertCircle } from 'lucide-react';

interface ReviewFormProps {
  bookId: number;
  onReviewAdded: () => void;
}

interface ReviewFormData {
  content: string;
  rating: number;
}

const ReviewForm: React.FC<ReviewFormProps> = ({ bookId, onReviewAdded }) => {
  const [error, setError] = useState<string>('');
  const [loading, setLoading] = useState(false);
  const [selectedRating, setSelectedRating] = useState<number>(0);
  
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ReviewFormData>();

  const onSubmit = async (data: ReviewFormData) => {
    if (selectedRating === 0) {
      setError('Please select a rating');
      return;
    }

    setLoading(true);
    setError('');
    
    try {
      await reviewService.createReview({
        content: data.content,
        rating: selectedRating,
        bookId,
      });
      reset();
      setSelectedRating(0);
      onReviewAdded();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to submit review');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      {error && (
        <div className="bg-red-50 border border-red-200 rounded-lg p-4 flex items-center space-x-2 text-red-700">
          <AlertCircle className="w-5 h-5" />
          <span>{error}</span>
        </div>
      )}

      <div>
        <label className="block text-sm font-medium text-gray-700 mb-2">
          Rating
        </label>
        <div className="flex items-center space-x-1">
          {[1, 2, 3, 4, 5].map((star) => (
            <button
              key={star}
              type="button"
              onClick={() => setSelectedRating(star)}
              className="p-1 hover:scale-110 transition-transform"
            >
              <Star
                className={`w-6 h-6 ${
                  star <= selectedRating
                    ? 'text-yellow-400 fill-current'
                    : 'text-gray-300 hover:text-yellow-300'
                }`}
              />
            </button>
          ))}
          <span className="ml-2 text-gray-600">
            {selectedRating > 0 && `${selectedRating}/5`}
          </span>
        </div>
      </div>

      <div>
        <label htmlFor="content" className="block text-sm font-medium text-gray-700">
          Review
        </label>
        <textarea
          {...register('content', { required: 'Review content is required' })}
          rows={4}
          className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent resize-none"
          placeholder="Share your thoughts about this book..."
        />
        {errors.content && (
          <p className="mt-1 text-sm text-red-600">{errors.content.message}</p>
        )}
      </div>

      <div className="flex justify-end space-x-3">
        <button
          type="button"
          onClick={() => {
            reset();
            setSelectedRating(0);
          }}
          className="px-4 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200 transition-colors"
        >
          Cancel
        </button>
        <button
          type="submit"
          disabled={loading}
          className="px-6 py-2 bg-amber-600 text-white rounded-lg hover:bg-amber-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-amber-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
        >
          {loading ? 'Submitting...' : 'Submit Review'}
        </button>
      </div>
    </form>
  );
};

export default ReviewForm;





