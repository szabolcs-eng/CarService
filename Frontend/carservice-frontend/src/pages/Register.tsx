import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import api from '../api';
import axios from 'axios';

export default function Register() {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (password !== confirmPassword) {
      setError('The two passwords do not match!');
      return;
    }

    try {
      await api.post('/Auth/register', {
        username: username,
        email: email,
        password: password
      });

      setSuccess('Registration successful! Redirecting to login...');
      setTimeout(() => {
        navigate('/login');
      }, 2000);

    } catch (err: unknown) {
      if (axios.isAxiosError(err) && err.response?.data) {
        setError(
          typeof err.response.data === "string"
            ? err.response.data
            : "An error occurred.",
        );
      } else {
        setError("Network error occurred. Please check the backend!");
      }
    }
  };

  return (
    <div className="min-h-screen bg-slate-950 flex flex-col justify-center py-12 sm:px-6 lg:px-8 text-slate-100">
      <div className="sm:mx-auto sm:w-full sm:max-w-md text-center">
        <h2 className="mt-4 text-3xl font-extrabold tracking-tight bg-gradient-to-r from-blue-400 to-indigo-400 bg-clip-text text-transparent">
          Create your Account
        </h2>
        <p className="mt-2 text-sm text-slate-400">
          Join CarService and start tracking your vehicle history today
        </p>
      </div>

      <div className="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div className="bg-slate-900/80 backdrop-blur-md py-8 px-6 shadow-2xl rounded-2xl border border-slate-800 sm:px-10">
          
          {error && (
            <div className="mb-4 bg-red-950/50 border border-red-500/50 text-red-300 px-4 py-3 rounded-xl text-sm flex items-center gap-2">
              <span>⚠️</span>
              <span>{error}</span>
            </div>
          )}

          {success && (
            <div className="mb-4 bg-emerald-950/50 border border-emerald-500/50 text-emerald-300 px-4 py-3 rounded-xl text-sm flex items-center gap-2 animate-pulse">
              <span>🎉</span>
              <span>{success}</span>
            </div>
          )}

          <form className="space-y-4" onSubmit={handleSubmit}>
            <div>
              <label className="block text-xs font-medium text-slate-300 uppercase tracking-wider">Username</label>
              <input 
                type="text" required placeholder="AutoMaster" value={username} onChange={(e) => setUsername(e.target.value)}
                className="mt-1 block w-full px-4 py-2.5 bg-slate-950 border border-slate-800 rounded-xl placeholder-slate-600 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 transition duration-200"
              />
            </div>

            <div>
              <label className="block text-xs font-medium text-slate-300 uppercase tracking-wider">Email Address</label>
              <input 
                type="email" required placeholder="name@example.com" value={email} onChange={(e) => setEmail(e.target.value)}
                className="mt-1 block w-full px-4 py-2.5 bg-slate-950 border border-slate-800 rounded-xl placeholder-slate-600 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 transition duration-200"
              />
            </div>

            <div>
              <label className="block text-xs font-medium text-slate-300 uppercase tracking-wider">Password</label>
              <input 
                type="password" required placeholder="••••••••" value={password} onChange={(e) => setPassword(e.target.value)}
                className="mt-1 block w-full px-4 py-2.5 bg-slate-950 border border-slate-800 rounded-xl placeholder-slate-600 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 transition duration-200"
              />
            </div>

            <div>
              <label className="block text-xs font-medium text-slate-300 uppercase tracking-wider">Confirm Password</label>
              <input 
                type="password" required placeholder="••••••••" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)}
                className="mt-1 block w-full px-4 py-2.5 bg-slate-950 border border-slate-800 rounded-xl placeholder-slate-600 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 transition duration-200"
              />
            </div>

            <div className="pt-2">
              <button 
                type="submit" 
                className="w-full py-3 px-4 rounded-xl shadow-lg text-sm font-semibold text-white bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-500 hover:to-indigo-500 focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all duration-200"
              >
                Create Account
              </button>
            </div>
          </form>

          <div className="mt-6 text-center border-t border-slate-800/80 pt-4">
            <span className="text-sm text-slate-400">Already have an account? </span>
            <Link to="/login" className="text-sm font-medium text-blue-400 hover:text-blue-300 transition duration-150">
              Sign in!
            </Link>
          </div>

        </div>
      </div>
    </div>
  );
}