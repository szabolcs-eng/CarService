import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
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

    // Kliensoldali ellenőrzés: egyeznek-e a jelszavak?
    if (password !== confirmPassword) {
      setError('A két jelszó nem egyezik meg!');
      return;
    }

    try {
      // ⚠️ FONTOS: A portot itt is egyeztesd a backended portjával (pl. 7196)!
      await axios.post('https://localhost:7196/api/Auth/register', {
        username: username,
        email: email,
        password: password
      });

      setSuccess('Sikeres regisztráció! Átirányítás a bejelentkezéshez...');
      
      // 2 másodperc múlva átirányítjuk a Login oldalra
      setTimeout(() => {
        navigate('/login');
      }, 2000);

    } catch (err: any) {
      // Itt kapjuk el, ha pl. a backend azt mondja: "Email already exists." vagy "Username already exists."
      if (err.response && err.response.data) {
        setError(err.response.data);
      } else {
        setError('Hálózati hiba történt. Kérlek ellenőrizd a backendet!');
      }
    }
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-5">
          <div className="card shadow-sm mt-4">
            <div className="card-body p-4">
              <h2 className="text-center mb-4 text-primary">Regisztráció</h2>
              
              {error && <div className="alert alert-danger">{error}</div>}
              {success && <div className="alert alert-success">{success}</div>}

              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label className="form-label">Felhasználónév</label>
                  <input 
                    type="text" 
                    className="form-control" 
                    placeholder="AutoMester99"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required 
                  />
                </div>

                <div className="mb-3">
                  <label className="form-label">Email cím</label>
                  <input 
                    type="email" 
                    className="form-control" 
                    placeholder="pelda@email.com"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required 
                  />
                </div>

                <div className="mb-3">
                  <label className="form-label">Jelszó</label>
                  <input 
                    type="password" 
                    className="form-control" 
                    placeholder="********"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required 
                  />
                </div>

                <div className="mb-4">
                  <label className="form-label">Jelszó megerősítése</label>
                  <input 
                    type="password" 
                    className="form-control" 
                    placeholder="********"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required 
                  />
                </div>

                <button type="submit" className="btn btn-primary w-100 py-2 mb-3">
                  Fiók létrehozása
                </button>

                <div className="text-center">
                  <span className="text-muted">Már van fiókod? </span>
                  <Link to="/login" className="text-decoration-none">Jelentkezz be!</Link>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}