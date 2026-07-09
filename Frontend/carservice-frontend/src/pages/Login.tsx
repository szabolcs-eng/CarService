import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import api from '../api';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(''); 
  const navigate = useNavigate(); 

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(''); 

    try {
      const response = await api.post('/Auth/login', {
        email: email,
        password: password
      });

      const token = response.data;
      
      localStorage.setItem('token', token);

      alert('Login is successful! Redirecting to dashboard...');
      
      navigate('/dashboard');

    } catch (err: any) {
      if (err.response && err.response.data) {
        setError(err.response.data);
      } else {
        setError('Network error occurred. Please check the backend!');
      }
    }
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-5">
          <div className="card shadow-sm mt-5">
            <div className="card-body p-4">
              <h2 className="text-center mb-4 text-primary">Login</h2>
              
              {error && <div className="alert alert-danger">{error}</div>}

              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label className="form-label">Email</label>
                  <input 
                    type="email" 
                    className="form-control" 
                    placeholder="example@email.com"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required 
                  />
                </div>
                <div className="mb-4">
                  <label className="form-label">Password</label>
                  <input 
                    type="password" 
                    className="form-control" 
                    placeholder="********"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required 
                  />
                </div>
                <button type="submit" className="btn btn-primary w-100 py-2">
                  Login
                </button>
                <div className="text-center mt-3">
                    <span className="text-muted">Don't have an account? </span>
                    <Link to="/register" className="text-decoration-none">Register now!</Link>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}