import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../api';

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

  const fetchLogs = async () => {
    try {
      const [fuelRes, serviceRes] = await Promise.all([
        api.get(`/FuelLog/vehicle/${id}`),
        api.get(`/ServiceLog/vehicle/${id}`)
      ]);
      setFuelLogs(fuelRes.data.data || fuelRes.data || []);
      setServiceLogs(serviceRes.data.data || serviceRes.data || []);
    } catch (err) {
      setError('Error fetching logs. Please check the backend!');
    }
  };

  const fetchAverageConsumption = async () => {
    try {
      const response = await api.get(`/FuelLog/vehicle/${id}/average-consumption`);
      setAvgConsumption(response.data);
      setAvgError(''); 
    } catch (err: any) {
      setAvgConsumption(null);
      if (err.response && err.response.data && typeof err.response.data === 'string') {
        setAvgError(err.response.data);
      } else {
        setAvgError('Error fetching average consumption. Please check the backend!');
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
      await api.post('/FuelLog/add', {
        vehicleId: Number(id),
        date: fuelDate,
        carKmCount: Number(fuelKm),
        fuelAmount: Number(fuelAmount),
        fuelCost: Number(fuelCost)
      });
      setFuelKm(''); setFuelAmount(''); setFuelCost('');
      fetchLogs();
      fetchAverageConsumption();
    } catch (err) {
      setError('Error adding fuel log. Please check the backend!');
    }
  };

  const handleAddService = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post('/ServiceLog/add', {
        vehicleId: Number(id),
        date: serviceDate,
        carKmCount: Number(serviceKm),
        serviceDescription: serviceDesc,
        serviceCost: Number(serviceCost)
      });
      setServiceKm(''); setServiceDesc(''); setServiceCost('');
      fetchLogs();
    } catch (err) {
      setError('Error adding service log. Please check the backend!');
    }
  };

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100 pb-16">
      {/* Navbar */}
      <nav className="border-b border-slate-800 bg-slate-900/50 backdrop-blur-lg sticky top-0 z-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
          <button 
            onClick={() => navigate('/dashboard')}
            className="flex items-center gap-2 px-3 py-1.5 rounded-xl bg-slate-800/80 hover:bg-slate-800 text-slate-300 hover:text-white text-xs font-semibold border border-slate-700/50 transition-all duration-200"
          >
            <span>⬅</span>
            <span>Back to Garage</span>
          </button>
          <span className="font-bold text-lg tracking-tight bg-gradient-to-r from-white to-slate-400 bg-clip-text text-transparent">
            Vehicle Telemetry & Logs
          </span>
          <div className="w-20"></div> {/* Spacer for center alignment */}
        </div>
      </nav>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        {error && (
          <div className="mb-6 bg-red-950/50 border border-red-500/50 text-red-300 px-4 py-3 rounded-xl text-sm">
            ⚠️ {error}
          </div>
        )}

        {/* Top Bento Banner: Average Consumption */}
        <div className="mb-8 bg-gradient-to-r from-slate-900 via-slate-900 to-emerald-950/30 border border-emerald-500/30 rounded-3xl p-6 shadow-2xl relative overflow-hidden">
          <div className="absolute -right-10 -bottom-10 w-40 h-40 bg-emerald-500/10 rounded-full blur-3xl pointer-events-none"></div>
          
          <div className="flex flex-col md:flex-row md:items-center justify-between gap-6 relative z-10">
            <div>
              <span className="px-3 py-1 rounded-full bg-emerald-500/10 text-emerald-400 border border-emerald-500/20 text-xs font-mono uppercase tracking-widest">
                Analytics Engine
              </span>
              <h3 className="text-2xl font-extrabold text-white mt-2">Average Fuel Consumption</h3>
              <p className="text-sm text-slate-400">Calculated based on odometer differences between full refills</p>
            </div>

            {avgConsumption ? (
              <div className="grid grid-cols-3 gap-4 sm:gap-8 bg-slate-950/60 border border-slate-800/80 px-6 py-4 rounded-2xl backdrop-blur-md">
                <div className="text-center">
                  <span className="block text-xs text-slate-400 uppercase font-medium">Distance</span>
                  <span className="text-lg sm:text-xl font-bold text-slate-200">{avgConsumption.totalDistanceKm} <small className="text-xs text-slate-500">km</small></span>
                </div>
                <div className="text-center border-x border-slate-800/80 px-2 sm:px-4">
                  <span className="block text-xs text-slate-400 uppercase font-medium">Fuel Used</span>
                  <span className="text-lg sm:text-xl font-bold text-slate-200">{avgConsumption.totalFuelUsedLiters} <small className="text-xs text-slate-500">L</small></span>
                </div>
                <div className="text-center">
                  <span className="block text-xs text-emerald-400 uppercase font-bold">Average</span>
                  <span className="text-xl sm:text-2xl font-extrabold text-emerald-400">{avgConsumption.averageConsumption} <small className="text-xs font-normal text-emerald-500">L/100km</small></span>
                </div>
              </div>
            ) : (
              <div className="bg-slate-950/40 border border-slate-800/80 px-6 py-4 rounded-2xl flex items-center gap-3 text-sm text-slate-400">
                <span className="text-xl">ℹ️</span>
                <span>{avgError || "At least 2 fuel logs are required for calculation."}</span>
              </div>
            )}
          </div>
        </div>

        {/* 2-Column Bento Grid: Fuel & Service */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
          
          {/* LEFT COLUMN: FUEL LOGS */}
          <div className="space-y-6">
            <div className="flex items-center gap-2 border-b border-slate-800 pb-3">
              <span className="text-2xl">⛽</span>
              <h4 className="text-lg font-bold text-emerald-400">Fuel Refill Logs</h4>
            </div>

            {/* Add Fuel Form */}
            <div className="bg-slate-900/60 border border-slate-800 rounded-2xl p-5 shadow-lg">
              <h5 className="text-xs font-bold text-slate-300 uppercase tracking-wider mb-3">Add Refill Record</h5>
              <form onSubmit={handleAddFuel} className="grid grid-cols-2 gap-3">
                <div className="col-span-2 sm:col-span-1">
                  <input type="date" required value={fuelDate} onChange={e => setFuelDate(e.target.value)} 
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-emerald-500" />
                </div>
                <div className="col-span-2 sm:col-span-1">
                  <input type="number" required placeholder="Odometer (km)" value={fuelKm} onChange={e => setFuelKm(Number(e.target.value))} 
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-emerald-500" />
                </div>
                <div className="col-span-1">
                  <input type="number" step="0.1" required placeholder="Liters (L)" value={fuelAmount} onChange={e => setFuelAmount(Number(e.target.value))} 
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-emerald-500" />
                </div>
                <div className="col-span-1">
                  <input type="number" required placeholder="Cost (HUF)" value={fuelCost} onChange={e => setFuelCost(Number(e.target.value))} 
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-emerald-500" />
                </div>
                <div className="col-span-2">
                  <button type="submit" className="w-full py-2.5 rounded-xl text-xs font-bold text-white bg-gradient-to-r from-emerald-600 to-teal-600 hover:from-emerald-500 hover:to-teal-500 shadow-md shadow-emerald-900/20 transition-all">
                    + Add Fuel Record
                  </button>
                </div>
              </form>
            </div>

            {/* Fuel List */}
            <div className="space-y-3">
              {fuelLogs.length === 0 ? (
                <p className="text-center text-sm text-slate-500 py-6">No fuel records logged yet.</p>
              ) : (
                fuelLogs?.map(log => (
                  <div key={log.id} className="bg-slate-900/40 border border-slate-800/80 rounded-xl p-4 flex items-center justify-between hover:border-slate-700/80 transition-colors">
                    <div>
                      <div className="flex items-center gap-2 mb-1">
                        <span className="text-xs font-bold text-slate-200">{new Date(log.date).toLocaleDateString()}</span>
                        <span className="text-[10px] font-mono px-2 py-0.5 bg-slate-800 text-slate-400 rounded-md border border-slate-700/50">{log.carKmCount} km</span>
                      </div>
                      <span className="text-sm font-semibold text-emerald-400">{log.fuelAmount} Liters</span>
                    </div>
                    <div className="text-right">
                      <span className="text-sm font-bold text-slate-200">{log.fuelCost.toLocaleString()}</span>
                      <span className="text-xs text-slate-500 block">HUF</span>
                    </div>
                  </div>
                ))
              )}
            </div>
          </div>

          {/* RIGHT COLUMN: SERVICE LOGS */}
          <div className="space-y-6">
            <div className="flex items-center gap-2 border-b border-slate-800 pb-3">
              <span className="text-2xl">🔧</span>
              <h4 className="text-lg font-bold text-amber-400">Maintenance & Service Logs</h4>
            </div>

            {/* Add Service Form */}
            <div className="bg-slate-900/60 border border-slate-800 rounded-2xl p-5 shadow-lg">
              <h5 className="text-xs font-bold text-slate-300 uppercase tracking-wider mb-3">Add Service Record</h5>
              <form onSubmit={handleAddService} className="grid grid-cols-2 gap-3">
                <div className="col-span-2 sm:col-span-1">
                  <input type="date" required value={serviceDate} onChange={e => setServiceDate(e.target.value)} 
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-amber-500" />
                </div>
                <div className="col-span-2 sm:col-span-1">
                  <input type="number" required placeholder="Odometer (km)" value={serviceKm} onChange={e => setServiceKm(Number(e.target.value))} 
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-amber-500" />
                </div>
                <div className="col-span-2">
                  <input type="text" required placeholder="Description (e.g., Oil Change & Brake Pads)" value={serviceDesc} onChange={e => setServiceDesc(e.target.value)} 
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-amber-500" />
                </div>
                <div className="col-span-2">
                  <input type="number" required placeholder="Total Cost (HUF)" value={serviceCost} onChange={e => setServiceCost(Number(e.target.value))} 
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-amber-500" />
                </div>
                <div className="col-span-2">
                  <button type="submit" className="w-full py-2.5 rounded-xl text-xs font-bold text-white bg-gradient-to-r from-amber-600 to-orange-600 hover:from-amber-500 hover:to-orange-500 shadow-md shadow-amber-900/20 transition-all">
                    + Add Maintenance Record
                  </button>
                </div>
              </form>
            </div>

            {/* Service List */}
            <div className="space-y-3">
              {serviceLogs.length === 0 ? (
                <p className="text-center text-sm text-slate-500 py-6">No service records logged yet.</p>
              ) : (
                serviceLogs?.map(log => (
                  <div key={log.id} className="bg-slate-900/40 border border-slate-800/80 rounded-xl p-4 flex items-center justify-between hover:border-slate-700/80 transition-colors">
                    <div>
                      <div className="flex items-center gap-2 mb-1">
                        <span className="text-xs font-bold text-slate-200">{new Date(log.date).toLocaleDateString()}</span>
                        <span className="text-[10px] font-mono px-2 py-0.5 bg-slate-800 text-slate-400 rounded-md border border-slate-700/50">{log.carKmCount} km</span>
                      </div>
                      <span className="text-sm font-medium text-slate-300 block">{log.serviceDescription}</span>
                    </div>
                    <div className="text-right shrink-0 ml-4">
                      <span className="text-sm font-bold text-amber-400">{log.serviceCost.toLocaleString()}</span>
                      <span className="text-xs text-slate-500 block">HUF</span>
                    </div>
                  </div>
                ))
              )}
            </div>
          </div>

        </div>
      </div>
    </div>
  );
}