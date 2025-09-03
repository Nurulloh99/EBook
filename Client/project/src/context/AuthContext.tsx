import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { User, AuthTokens } from '../types';
import { authService } from '../services/authService';

interface AuthContextType {
  user: User | null;
  tokens: AuthTokens | null;
  login: (userName: string, password: string) => Promise<void>;
  logout: () => void;
  signup: (data: any) => Promise<void>;
  isAuthenticated: boolean;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [tokens, setTokens] = useState<AuthTokens | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
  const initAuth = () => {
    try {
      const savedTokens = localStorage.getItem('authTokens');
      const savedUser = localStorage.getItem('user');
      
      if (savedTokens) {
        setTokens(JSON.parse(savedTokens));
      }
      if (savedUser) {
        setUser(JSON.parse(savedUser));
      }
    } catch (e) {
      console.error("Auth data in localStorage is invalid:", e);
      localStorage.removeItem("authTokens");
      localStorage.removeItem("user");
    } finally {
      setLoading(false);
    }
  };

  initAuth();
}, []);


  const login = async (userName: string, password: string) => {
    try {
      const response = await authService.login({ userName, password });
      const { accessToken, refreshToken, userData } = response.data;
      
      const authTokens = { accessToken, refreshToken };
      setTokens(authTokens);
      setUser(userData);
      
      localStorage.setItem('authTokens', JSON.stringify(authTokens));
      localStorage.setItem('user', JSON.stringify(userData));
    } catch (error) {
      throw error;
    }
  };

  const logout = async () => {
    try {
      if (tokens?.refreshToken) {
        await authService.logout(tokens.refreshToken);
      }
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      setUser(null);
      setTokens(null);
      localStorage.removeItem('authTokens');
      localStorage.removeItem('user');
    }
  };

  const signup = async (data: any) => {
    const response = await authService.signup(data);
    localStorage.setItem('pendingEmail', data.email);
    return response;
  };

  const value = {
    user,
    tokens,
    login,
    logout,
    signup,
    isAuthenticated: !!tokens,
    loading,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};