import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';

interface FuelLog {
  id: number;
  date: string;
  carKmCount: number;
  fuelAmount: number;
  fuelCost: number;
}

interface ServiceLog {
  id: number;
  date: string;
  carKmCount: number;
  serviceDescription: string;
  serviceCost: number;
}

// Új interfész az átlagfogyasztásnak
interface AvgConsumptionData {
  totalDistanceKm: number;
  totalFuelUsedLiters: number;
  averageConsumption: number;
}

export default function VehicleDetails() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [fuelLogs, setFuelLogs] = useState<FuelLog[]>([]);
  const [serviceLogs, setServiceLogs] = useState<ServiceLog[]>([]);
  const [error, setError] = useState('');

  // Átlagfogyasztás állapotai
  const [avgConsumption, setAvgConsumption] = useState<AvgConsumptionData | null>(null);
  const [avgError, setAvgError] = useState('');

  const [fuelDate, setFuelDate] = useState(new Date().toISOString().split('T')[0]);
  const [fuelKm, setFuelKm] = useState<number | ''>('');
  const [fuelAmount, setFuelAmount] = useState<number | ''>('');
  const [fuelCost, setFuelCost] = useState<number | ''>('');

  const [serviceDate, setServiceDate] = useState(new Date().toISOString().split('T')[0]);
  const [serviceKm, setServiceKm] = useState<number | ''>('');
  const [serviceDesc, setServiceDesc] = useState('');
  const [serviceCost, setServiceCost] = useState<number | ''>('');

  const API_BASE_URL = 'https://localhost:7196/api';

  const fetchLogs = async () => {
    try {
      const [fuelRes, serviceRes] = await Promise.all([
        axios.get(`${API_BASE_URL}/FuelLog/vehicle/${id}`),
        axios.get(`${API_BASE_URL}/ServiceLog/vehicle/${id}`)
      ]);
      setFuelLogs(fuelRes.data);
      setServiceLogs(serviceRes.data);
    } catch (err) {
      setError('Hiba történt a naplók betöltésekor.');
    }
  };

  // Külön függvény az átlagfogyasztás lekérésére
  const fetchAverageConsumption = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/FuelLog/vehicle/${id}/average-consumption`);
      setAvgConsumption(response.data);
      setAvgError(''); // Ha sikeres, töröljük a hibaüzenetet
    } catch (err: any) {
      setAvgConsumption(null);
      // Ha a backend pl. BadRequest-et dob (< 2 tankolás), kiírjuk a felhasználónak
      if (err.response && err.response.data && typeof err.response.data === 'string') {
        setAvgError(err.response.data);
      } else {
        setAvgError('Nem sikerült betölteni az átlagfogyasztást.');
      }
    }
  };

  useEffect(() => {
    if (!localStorage.getItem('token')) {
      navigate('/login');
    } else {
      fetchLogs();
      fetchAverageConsumption();
    }
  }, [id]);

  const handleAddFuel = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post(`${API_BASE_URL}/FuelLog/add`, {
        vehicleId: Number(id),
        date: fuelDate,
        carKmCount: Number(fuelKm),
        fuelAmount: Number(fuelAmount),
        fuelCost: Number(fuelCost)
      });
      setFuelKm(''); setFuelAmount(''); setFuelCost('');
      
      // Frissítjük a listát ÉS az átlagfogyasztást is!
      fetchLogs();
      fetchAverageConsumption();
    } catch (err) {
      setError('Nem sikerült menteni a tankolást.');
    }
  };

  const handleAddService = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post(`${API_BASE_URL}/ServiceLog/add`, {
        vehicleId: Number(id),
        date: serviceDate,
        carKmCount: Number(serviceKm),
        serviceDescription: serviceDesc,
        serviceCost: Number(serviceCost)
      });
      setServiceKm(''); setServiceDesc(''); setServiceCost('');
      fetchLogs();
    } catch (err) {
      setError('Nem sikerült menteni a szervizt.');
    }
  };

  return (
    <div>
      <nav className="navbar navbar-dark bg-secondary shadow-sm mb-4">
        <div className="container">
          <button className="btn btn-outline-light btn-sm" onClick={() => navigate('/dashboard')}>
            ⬅ Vissza a Dashboardra
          </button>
          <span className="navbar-brand mb-0 h1">Jármű Részletek</span>
        </div>
      </nav>

      <div className="container">
        {error && <div className="alert alert-danger">{error}</div>}

        <div className="row">
          {/* BAL OSZLOP: Tankolások */}
          <div className="col-md-6 mb-4">
            <h4 className="text-success border-bottom pb-2 mb-3">⛽ Tankolási Napló</h4>
            
            {/* ÚJ RÉSZ: Átlagfogyasztás kártya */}
            <div className="card shadow-sm border-success mb-3 bg-light">
              <div className="card-body py-2 text-center">
                <h6 className="text-success fw-bold mb-1">📊 Átlagfogyasztás statisztika</h6>
                {avgConsumption ? (
                  <div className="d-flex justify-content-around mt-2">
                    <div><small className="text-muted d-block">Távolság</small><strong>{avgConsumption.totalDistanceKm} km</strong></div>
                    <div><small className="text-muted d-block">Fogyasztás</small><strong>{avgConsumption.totalFuelUsedLiters} L</strong></div>
                    <div><small className="text-muted d-block">Átlag</small><strong className="text-danger fs-5">{avgConsumption.averageConsumption} L/100km</strong></div>
                  </div>
                ) : (
                  <p className="text-muted mb-0 mt-1 small">
                    <i className="bi bi-info-circle"></i> {avgError || "Legalább 2 tankolás szükséges a számításhoz."}
                  </p>
                )}
              </div>
            </div>

            <div className="card shadow-sm mb-4">
              <div className="card-body">
                <form onSubmit={handleAddFuel} className="row g-2">
                  <div className="col-6"><input type="date" className="form-control form-control-sm" value={fuelDate} onChange={e => setFuelDate(e.target.value)} required /></div>
                  <div className="col-6"><input type="number" className="form-control form-control-sm" placeholder="Km óra állás" value={fuelKm} onChange={e => setFuelKm(Number(e.target.value))} required /></div>
                  <div className="col-6"><input type="number" step="0.1" className="form-control form-control-sm" placeholder="Mennyiség (Liter)" value={fuelAmount} onChange={e => setFuelAmount(Number(e.target.value))} required /></div>
                  <div className="col-6"><input type="number" className="form-control form-control-sm" placeholder="Költség (Ft)" value={fuelCost} onChange={e => setFuelCost(Number(e.target.value))} required /></div>
                  <div className="col-12"><button type="submit" className="btn btn-success btn-sm w-100 mt-2">Tankolás Rögzítése</button></div>
                </form>
              </div>
            </div>

            <ul className="list-group shadow-sm">
              {fuelLogs.map(log => (
                <li key={log.id} className="list-group-item d-flex justify-content-between align-items-center">
                  <div>
                    <strong>{new Date(log.date).toLocaleDateString()}</strong> - {log.carKmCount} km
                    <br/><small className="text-muted">{log.fuelAmount} L | {log.fuelCost} Ft</small>
                  </div>
                </li>
              ))}
            </ul>
          </div>

          {/* JOBB OSZLOP: Szervizek */}
          <div className="col-md-6 mb-4">
            <h4 className="text-warning border-bottom pb-2 mb-3">🔧 Szerviz Napló</h4>
            
            <div className="card shadow-sm mb-4">
              <div className="card-body">
                <form onSubmit={handleAddService} className="row g-2">
                  <div className="col-6"><input type="date" className="form-control form-control-sm" value={serviceDate} onChange={e => setServiceDate(e.target.value)} required /></div>
                  <div className="col-6"><input type="number" className="form-control form-control-sm" placeholder="Km óra állás" value={serviceKm} onChange={e => setServiceKm(Number(e.target.value))} required /></div>
                  <div className="col-12"><input type="text" className="form-control form-control-sm" placeholder="Szerviz leírása (pl. Olajcsere)" value={serviceDesc} onChange={e => setServiceDesc(e.target.value)} required /></div>
                  <div className="col-12"><input type="number" className="form-control form-control-sm" placeholder="Költség (Ft)" value={serviceCost} onChange={e => setServiceCost(Number(e.target.value))} required /></div>
                  <div className="col-12"><button type="submit" className="btn btn-warning btn-sm w-100 mt-2">Szerviz Rögzítése</button></div>
                </form>
              </div>
            </div>

            <ul className="list-group shadow-sm">
              {serviceLogs.map(log => (
                <li key={log.id} className="list-group-item d-flex justify-content-between align-items-center">
                  <div>
                    <strong>{new Date(log.date).toLocaleDateString()}</strong> - {log.carKmCount} km
                    <br/><span>{log.serviceDescription}</span>
                    <br/><small className="text-muted">{log.serviceCost} Ft</small>
                  </div>
                </li>
              ))}
            </ul>
          </div>

        </div>
      </div>
    </div>
  );
}