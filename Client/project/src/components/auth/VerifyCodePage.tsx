import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { authService } from '../../services/authService';
import { BookOpen, Mail, AlertCircle, CheckCircle } from 'lucide-react';

interface VerifyFormData {
  code: string;
}

const VerifyCodePage: React.FC = () => {
  const navigate = useNavigate();
  const [error, setError] = useState<string>('');
  const [loading, setLoading] = useState(false);
  const [email, setEmail] = useState<string>('');
  
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<VerifyFormData>();

  useEffect(() => {
    const pendingEmail = localStorage.getItem('pendingEmail');
    if (!pendingEmail) {
      navigate('/signup');
      return;
    }
    setEmail(pendingEmail);
    
    // Send verification code
    const sendCode = async () => {
      try {
        await authService.sendCode(pendingEmail);
      } catch (err) {
        console.error('Failed to send verification code:', err);
      }
    };
    sendCode();
  }, [navigate]);

  const onSubmit = async (data: VerifyFormData) => {
    setLoading(true);
    setError('');
    
    try {
      const response = await authService.confirmCode(email, data.code);
      localStorage.removeItem('pendingEmail');
      
      if (response.data.accessToken) {
        const authTokens = {
          accessToken: response.data.accessToken,
          refreshToken: response.data.refreshToken,
        };
        localStorage.setItem('authTokens', JSON.stringify(authTokens));
        localStorage.setItem('user', JSON.stringify(response.data.user));
      }
      
      navigate('/books');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Invalid verification code');
    } finally {
      setLoading(false);
    }
  };

  const resendCode = async () => {
    try {
      await authService.sendCode(email);
      setError('');
    } catch (err: any) {
      setError('Failed to resend code');
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-amber-50 to-orange-50 flex items-center justify-center px-4">
      <div className="max-w-md w-full space-y-8">
        <div className="text-center">
          <div className="flex justify-center">
            <BookOpen className="w-12 h-12 text-amber-600" />
          </div>
          <h2 className="mt-6 text-3xl font-bold text-gray-900">Verify your email</h2>
          <p className="mt-2 text-sm text-gray-600">
            We've sent a verification code to{' '}
            <span className="font-medium text-amber-600">{email}</span>
          </p>
        </div>

        <div className="bg-white py-8 px-6 shadow-xl rounded-xl">
          {error && (
            <div className="mb-4 bg-red-50 border border-red-200 rounded-lg p-4 flex items-center space-x-2 text-red-700">
              <AlertCircle className="w-5 h-5" />
              <span>{error}</span>
            </div>
          )}

          <form className="space-y-6" onSubmit={handleSubmit(onSubmit)}>
            <div>
              <label htmlFor="code" className="block text-sm font-medium text-gray-700">
                Verification Code
              </label>
              <div className="mt-1 relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Mail className="h-5 w-5 text-gray-400" />
                </div>
                <input
                  {...register('code', {
                    required: 'Verification code is required',
                    pattern: {
                      value: /^\d{6}$/,
                      message: 'Code must be 6 digits',
                    },
                  })}
                  type="text"
                  maxLength={6}
                  className="block w-full pl-10 pr-3 py-3 text-center text-2xl tracking-widest border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
                  placeholder="123456"
                />
              </div>
              {errors.code && (
                <p className="mt-1 text-sm text-red-600">{errors.code.message}</p>
              )}
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full flex justify-center py-3 px-4 border border-transparent rounded-lg shadow-sm text-sm font-medium text-white bg-amber-600 hover:bg-amber-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-amber-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
            >
              {loading ? 'Verifying...' : 'Verify email'}
            </button>

            <div className="text-center">
              <button
                type="button"
                onClick={resendCode}
                className="text-sm text-amber-600 hover:text-amber-500"
              >
                Didn't receive the code? Resend
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default VerifyCodePage;