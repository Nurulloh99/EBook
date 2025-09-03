import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { BookOpen, Star, Users, Upload, Search } from 'lucide-react';

const HomePage: React.FC = () => {
  const { isAuthenticated } = useAuth();

  return (
    <div className="space-y-16">
      {/* Hero Section */}
      <section className="text-center py-16">
        <div className="max-w-4xl mx-auto">
          <div className="flex justify-center mb-8">
            <div className="bg-amber-100 p-6 rounded-full">
              <BookOpen className="w-16 h-16 text-amber-600" />
            </div>
          </div>
          
          <h1 className="text-5xl md:text-6xl font-bold text-gray-900 mb-6 leading-tight">
            Your Digital
            <span className="text-amber-600 block">Book Literacy</span>
          </h1>
          
          <p className="text-xl text-gray-600 mb-8 max-w-2xl mx-auto leading-relaxed">
            Discover, share, and manage your book collection in one beautiful platform. 
            Connect with fellow readers and explore new worlds through literature.
          </p>
          
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            {isAuthenticated ? (
              <>
                <Link
                  to="/books"
                  className="bg-amber-600 text-white px-8 py-4 rounded-lg hover:bg-amber-700 transition-colors text-lg font-medium"
                >
                  Browse Books
                </Link>
                <Link
                  to="/add-book"
                  className="bg-emerald-600 text-white px-8 py-4 rounded-lg hover:bg-emerald-700 transition-colors text-lg font-medium"
                >
                  Upload Your Book
                </Link>
              </>
            ) : (
              <>
                <Link
                  to="/signup"
                  className="bg-amber-600 text-white px-8 py-4 rounded-lg hover:bg-amber-700 transition-colors text-lg font-medium"
                >
                  Get Started
                </Link>
                <Link
                  to="/login"
                  className="border border-amber-600 text-amber-600 px-8 py-4 rounded-lg hover:bg-amber-50 transition-colors text-lg font-medium"
                >
                  Sign In
                </Link>
              </>
            )}
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="py-16">
        <div className="max-w-6xl mx-auto">
          <h2 className="text-3xl font-bold text-center text-gray-900 mb-12">
            Everything you need for book management
          </h2>
          
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
            <FeatureCard
              icon={<Upload className="w-8 h-8" />}
              title="Upload & Share"
              description="Easily upload your books and share them with the community"
            />
            <FeatureCard
              icon={<Search className="w-8 h-8" />}
              title="Discover Books"
              description="Search and filter books by genre, language, and more"
            />
            <FeatureCard
              icon={<Star className="w-8 h-8" />}
              title="Rate & Review"
              description="Share your thoughts and help others discover great reads"
            />
            <FeatureCard
              icon={<Users className="w-8 h-8" />}
              title="Join Community"
              description="Connect with fellow readers and book enthusiasts"
            />
          </div>
        </div>
      </section>

      {/* CTA Section */}
      {!isAuthenticated && (
        <section className="bg-gradient-to-r from-amber-600 to-orange-600 rounded-2xl p-12 text-center text-white">
          <h2 className="text-3xl font-bold mb-4">Ready to start your reading journey?</h2>
          <p className="text-xl mb-8 opacity-90">
            Join thousands of readers sharing and discovering amazing books
          </p>
          <Link
            to="/signup"
            className="bg-white text-amber-600 px-8 py-4 rounded-lg hover:bg-gray-100 transition-colors text-lg font-medium inline-flex items-center space-x-2"
          >
            <BookOpen className="w-5 h-5" />
            <span>Start Reading Today</span>
          </Link>
        </section>
      )}
    </div>
  );
};

interface FeatureCardProps {
  icon: React.ReactNode;
  title: string;
  description: string;
}

const FeatureCard: React.FC<FeatureCardProps> = ({ icon, title, description }) => {
  return (
    <div className="bg-white p-6 rounded-xl shadow-sm border border-amber-100 text-center hover:shadow-md transition-shadow">
      <div className="text-amber-600 mb-4 flex justify-center">{icon}</div>
      <h3 className="text-lg font-semibold text-gray-900 mb-2">{title}</h3>
      <p className="text-gray-600">{description}</p>
    </div>
  );
};

export default HomePage;