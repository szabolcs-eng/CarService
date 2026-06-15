import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

// TypeScript interfész a járművek típusbiztonságához
interface Vehicle {
  id: number;
  licensePlate: string;
  brand: string;
  model: string;
  year: number;
}

export default function Dashboard() {
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  // Űrlap állapotok az új autó hozzáadásához
  const [brand, setBrand] = useState('');
  const [model, setModel] = useState('');
  const [licensePlate, setLicensePlate] = useState('');
  const [year, setYear] = useState<number>(new Date().getFullYear());

  // Segédfüggvény a userId kinyerésére a JWT tokenből
  const getUserIdFromToken = (): number | null => {
    const token = localStorage.getItem('token');
    if (!token) return null;
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const payload = JSON.parse(window.atob(base64));
      // A .NET standard ClaimTypes.NameIdentifier kulcsa vagy a 'nameid' keresése
      const userId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || payload["nameid"];
      return userId ? parseInt(userId) : null;
    } catch (e) {
      return null;
    }
  };

  const userId = getUserIdFromToken();
  // ⚠️ FONTOS: Írd át a portot arra, amin a C# backend fut!
  const API_BASE_URL = 'https://localhost:7196/api/Vehicle';

  // Autók betöltése az API-ból
  const fetchVehicles = async () => {
    if (!userId) return;
    try {
      const response = await axios.get(`${API_BASE_URL}/user-vehicles/${userId}`);
      setVehicles(response.data);
    } catch (err) {
      setError('Nem sikerült betölteni a járműveket.');
    }
  };

  // Ez a hook fut le az oldal betöltődésekor
  useEffect(() => {
    if (!localStorage.getItem('token') || !userId) {
      // Ha nincs token, vagy érvénytelen, visszadobjuk a loginra
      navigate('/login');
    } else {
      fetchVehicles();
    }
  }, []);

  // Új jármű beküldése
  const handleAddVehicle = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (!userId) return;

    try {
      await axios.post(`${API_BASE_URL}/add`, {
        userId: userId,
        licensePlate: licensePlate,
        brand: brand,
        model: model,
        year: year
      });

      // Űrlap kiürítése sikeres mentés után
      setBrand('');
      setModel('');
      setLicensePlate('');
      setYear(new Date().getFullYear());

      // Lista frissítése
      fetchVehicles();
    } catch (err) {
      setError('Nem sikerült hozzáadni a járművet.');
    }
  };

  // Kijelentkezés
  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  return (
    <div>
      {/* Bootstrap Navigációs sáv */}
      <nav className="navbar navbar-dark bg-primary shadow-sm mb-4">
        <div className="container">
          <span className="navbar-brand mb-0 h1">🚗 CarService</span>
          <button className="btn btn-outline-light btn-sm bg-danger" onClick={handleLogout}>
            Kijelentkezés
          </button>
        </div>
      </nav>

      <div className="container">
        {error && <div className="alert alert-danger">{error}</div>}

        <div className="row">
          {/* BAL OLDAL: Új autó hozzáadása form */}
          <div className="col-md-4 mb-4">
            <div className="card shadow-sm">
              <div className="card-body">
                <h4 className="card-title text-primary mb-3">Új jármű rögzítése</h4>
                <form onSubmit={handleAddVehicle}>
                  <div className="mb-2">
                    <label className="form-label small">Márka</label>
                    <input type="text" className="form-control form-control-sm" placeholder="Pl. Ford" value={brand} onChange={(e) => setBrand(e.target.value)} required />
                  </div>
                  <div className="mb-2">
                    <label className="form-label small">Modell</label>
                    <input type="text" className="form-control form-control-sm" placeholder="Pl. Focus" value={model} onChange={(e) => setModel(e.target.value)} required />
                  </div>
                  <div className="mb-2">
                    <label className="form-label small">Rendszám</label>
                    <input type="text" className="form-control form-control-sm" placeholder="Pl. ABC-123" value={licensePlate} onChange={(e) => setLicensePlate(e.target.value)} required />
                  </div>
                  <div className="mb-3">
                    <label className="form-label small">Évjárat</label>
                    <input type="number" className="form-control form-control-sm" value={year} onChange={(e) => setYear(parseInt(e.target.value))} required min={1900}/>
                  </div>
                  <button type="submit" className="btn btn-success btn-sm w-100">Mentés a profilhoz</button>
                </form>
              </div>
            </div>
          </div>

          {/* JOBB OLDAL: Járművek listája */}
          <div className="col-md-8">
            <h3 className="mb-3">Saját járművek</h3>
            {vehicles.length === 0 ? (
              <div className="alert alert-info">Még nincs rögzített járműved. Adj hozzá egyet a bal oldali panelen!</div>
            ) : (
              <div className="row row-cols-1 row-cols-md-2 g-3">
                {vehicles.map((vehicle) => (
                  <div className="col" key={vehicle.id}>
                    <div className="card h-100 border-start border-primary border-4 shadow-sm">
                      <div className="card-body d-flex flex-column justify-content-between">
                        <div>
                          <div className="badge bg-secondary mb-2">{vehicle.licensePlate}</div>
                          <h5 className="card-title mb-1">{vehicle.brand} {vehicle.model}</h5>
                          <p className="text-muted small">Évjárat: {vehicle.year}</p>
                        </div>
                        <button 
                          className="btn btn-outline-primary btn-sm mt-3 w-100"
                          onClick={() => navigate(`/vehicle/${vehicle.id}`)}
                        >
                          Részletek és Naplók ➔
                        </button>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}