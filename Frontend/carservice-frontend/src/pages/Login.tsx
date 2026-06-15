import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(''); // Hibaüzenetek tárolására
  const navigate = useNavigate(); // Navigáláshoz a sikeres login után

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(''); // Korábbi hibaüzenet törlése

    try {
      // ⚠️ FONTOS: Írd át a portot (számot) arra, amin a C# backend fut (pl. 7196, 5001)!
      const response = await axios.post('https://localhost:7196/api/Auth/login', {
        email: email,
        password: password
      });

      // Sikeres kérés esetén a válasz (response.data) maga a JWT token szövege
      const token = response.data;
      
      // Eltároljuk a böngészőben, hogy később is tudjuk használni
      localStorage.setItem('token', token);

      // Ideiglenes visszajelzés a teszteléshez
      alert('Sikeres bejelentkezés! A token elmentve.');
      
      // Később ide navigálunk:
      // navigate('/dashboard');

    } catch (err: any) {
      // Ha a backend mondjuk 400 BadRequest-et dob (pl. Invalid email or password.)
      if (err.response && err.response.data) {
        setError(err.response.data);
      } else {
        setError('Hálózati hiba történt. Fut a backend szerver?');
      }
    }
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-5">
          <div className="card shadow-sm mt-5">
            <div className="card-body p-4">
              <h2 className="text-center mb-4 text-primary">Bejelentkezés</h2>
              
              {/* Ha van hibaüzenet, itt megjelenítjük egy piros dobozban */}
              {error && <div className="alert alert-danger">{error}</div>}

              <form onSubmit={handleSubmit}>
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
                <div className="mb-4">
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
                <button type="submit" className="btn btn-primary w-100 py-2">
                  Belépés
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}