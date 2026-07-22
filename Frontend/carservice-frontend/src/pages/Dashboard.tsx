import { useEffect, useState, useCallback, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api";

interface Vehicle {
  id: number;
  licensePlate: string;
  brand: string;
  model: string;
  year: number;
  technicalInspectionExpiry?: string;
}

export default function Dashboard() {
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const [brand, setBrand] = useState("");
  const [model, setModel] = useState("");
  const [licensePlate, setLicensePlate] = useState("");
  const [year, setYear] = useState<number>(new Date().getFullYear());
  const [expiryDate, setExpiryDate] = useState("");

  const getUserIdFromToken = (): number | null => {
    const token = localStorage.getItem("token");
    if (!token) return null;
    try {
      const base64Url = token.split(".")[1];
      const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
      const payload = JSON.parse(window.atob(base64));
      const userId =
        payload[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        ] || payload["nameid"];
      return userId ? parseInt(userId) : null;
    } catch (e) {
      return null;
    }
  };

  const userId = useMemo(() => getUserIdFromToken(), []);

  const fetchVehicles = useCallback(async () => {
    if (!userId) return;
    try {
      const response = await api.get(`/Vehicle/user-vehicles/${userId}`);
      setVehicles(response.data.data || response.data);
    } catch (err) {
      setError("Vehicle list fetch failed. Please check the backend!");
    }
  }, [userId]);

  useEffect(() => {
    if (!localStorage.getItem("token") || !userId) {
      navigate("/login");
    } else {
      fetchVehicles();
    }
  }, [userId, navigate, fetchVehicles]);

  const handleAddVehicle = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    if (!userId) return;

    try {
      await api.post(`/Vehicle/add`, {
        userId: userId,
        licensePlate: licensePlate,
        brand: brand,
        model: model,
        year: year,
        technicalInspectionExpiry: expiryDate ? expiryDate : null,
      });

      setBrand("");
      setModel("");
      setLicensePlate("");
      setYear(new Date().getFullYear());
      setExpiryDate("");
      fetchVehicles();
    } catch (err) {
      setError("Failed to add the vehicle. Please check the backend!");
    }
  };

  const getDaysUntilExpiry = (expiryDate?: string) => {
    if (!expiryDate) return null;
    const today = new Date();
    const expiry = new Date(expiryDate);
    const diffTime = expiry.getTime() - today.getTime();
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100 pb-12">
      <nav className="border-b border-slate-800 bg-slate-900/50 backdrop-blur-lg sticky top-0 z-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
          <div className="flex items-center gap-3">
            <span className="text-2xl">🚗</span>
            <span className="font-bold text-xl tracking-tight bg-gradient-to-r from-white to-slate-400 bg-clip-text text-transparent">
              CarService Dashboard
            </span>
          </div>
          <button
            onClick={handleLogout}
            className="px-4 py-2 rounded-xl text-xs font-semibold bg-red-500/10 text-red-400 border border-red-500/20 hover:bg-red-500 hover:text-white transition-all duration-200"
          >
            Sign Out
          </button>
        </div>
      </nav>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        {error && (
          <div className="mb-6 bg-red-950/50 border border-red-500/50 text-red-300 px-4 py-3 rounded-xl text-sm">
            ⚠️ {error}
          </div>
        )}

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          <div className="lg:col-span-1">
            <div className="bg-slate-900/80 border border-slate-800 rounded-2xl p-6 shadow-xl sticky top-24">
              <h4 className="text-lg font-bold text-slate-200 mb-4 flex items-center gap-2">
                <span>➕</span> Add New Vehicle
              </h4>
              <form onSubmit={handleAddVehicle} className="space-y-4">
                <div>
                  <label className="block text-xs font-medium text-slate-400 uppercase tracking-wider mb-1">
                    Brand
                  </label>
                  <input
                    type="text"
                    required
                    placeholder="e.g., Toyota"
                    value={brand}
                    onChange={(e) => setBrand(e.target.value)}
                    className="w-full px-3 py-2.5 bg-slate-950 border border-slate-800 rounded-xl text-sm text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  />
                </div>
                <div>
                  <label className="block text-xs font-medium text-slate-400 uppercase tracking-wider mb-1">
                    Model
                  </label>
                  <input
                    type="text"
                    required
                    placeholder="e.g., Corolla"
                    value={model}
                    onChange={(e) => setModel(e.target.value)}
                    className="w-full px-3 py-2.5 bg-slate-950 border border-slate-800 rounded-xl text-sm text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  />
                </div>
                <div>
                  <label className="block text-xs font-medium text-slate-400 uppercase tracking-wider mb-1">
                    License Plate
                  </label>
                  <input
                    type="text"
                    required
                    placeholder="e.g., ABC-123"
                    value={licensePlate}
                    onChange={(e) => setLicensePlate(e.target.value)}
                    className="w-full px-3 py-2.5 bg-slate-950 border border-slate-800 rounded-xl text-sm text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500 uppercase"
                  />
                </div>
                <div>
                  <label className="block text-xs font-medium text-slate-400 uppercase tracking-wider mb-1">
                    Manufacturing Year
                  </label>
                  <input
                    type="number"
                    required
                    min={1900}
                    value={year}
                    onChange={(e) => setYear(parseInt(e.target.value))}
                    className="w-full px-3 py-2.5 bg-slate-950 border border-slate-800 rounded-xl text-sm text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  />
                </div>
                <div>
                  <label className="block text-xs font-medium text-slate-400 uppercase tracking-wider mb-1">
                    Technical Inspection Expiry
                  </label>
                  <input
                    type="date"
                    value={expiryDate}
                    onChange={(e) => setExpiryDate(e.target.value)}
                    className="w-full px-3 py-2.5 bg-slate-950 border border-slate-800 rounded-xl text-sm text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  />
                </div>
                <button
                  type="submit"
                  className="w-full py-3 mt-2 rounded-xl text-sm font-semibold text-white bg-gradient-to-r from-emerald-600 to-teal-600 hover:from-emerald-500 hover:to-teal-500 shadow-lg shadow-emerald-900/20 transition-all duration-200"
                >
                  Save to Profile
                </button>
              </form>
            </div>
          </div>

          <div className="lg:col-span-2">
            <div className="flex items-center justify-between mb-6">
              <h3 className="text-xl font-bold text-slate-200">
                My Garage ({vehicles.length})
              </h3>
            </div>


            {vehicles.length === 0 ? (
              <div className="bg-slate-900/50 border border-slate-800/80 rounded-2xl p-12 text-center text-slate-400">
                <span className="text-5xl block mb-3">🛠️</span>
                <p className="text-lg font-medium text-slate-300">
                  Your garage is empty
                </p>
                <p className="text-sm mt-1">
                  Use the panel on the left to add your first vehicle!
                </p>
              </div>
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {vehicles.map((vehicle) => {
                  const daysLeft = getDaysUntilExpiry(
                    vehicle.technicalInspectionExpiry,
                  );
                  const isExpiringSoon = daysLeft !== null && daysLeft <= 30;

                  return (
                    <div
                      key={vehicle.id}
                      className="group bg-slate-900/60 border border-slate-800 rounded-2xl p-5 hover:bg-slate-900 hover:border-slate-700/80 transition-all duration-300 flex flex-col justify-between shadow-lg hover:shadow-xl hover:-translate-y-1"
                    >
                      <div>
                        <div className="flex justify-between items-start mb-3">
                          <div className="flex items-center gap-2">
                            <span className="px-3 py-1 bg-slate-800 border border-slate-700 rounded-lg text-xs font-mono font-bold tracking-widest text-blue-400 uppercase shadow-inner">
                              {vehicle.licensePlate}
                            </span>
                            {isExpiringSoon && (
                              <span
                                title={
                                  daysLeft < 0
                                    ? "The technical inspection has expired!"
                                    : `The technical inspection will expire in ${daysLeft} days!`
                                }
                                className="flex h-3 w-3 relative cursor-help"
                              >
                                <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-red-400 opacity-75"></span>
                                <span className="relative inline-flex rounded-full h-3 w-3 bg-red-500"></span>
                              </span>
                            )}
                          </div>
                          <span className="text-xs font-semibold px-2.5 py-1 bg-slate-950 text-slate-400 rounded-full border border-slate-800">
                            {vehicle.year}
                          </span>
                        </div>
                        <h5 className="text-lg font-bold text-white group-hover:text-blue-400 transition-colors duration-200">
                          {vehicle.brand} {vehicle.model}
                        </h5>
                      </div>

                      <div className="mt-6 pt-4 border-t border-slate-800/80 flex flex-wrap items-center justify-between gap-2">
                        <span className="text-xs text-slate-500 font-medium">
                          Service & Fuel History
                        </span>
                        <button
                          onClick={() =>
                            navigate(`/vehicle/${vehicle.id}`, {
                              state: {
                                expiryDate: vehicle.technicalInspectionExpiry,
                              },
                            })
                          }
                          className="px-4 py-2 rounded-xl bg-blue-600/10 text-blue-400 border border-blue-500/20 text-xs font-semibold group-hover:bg-blue-600 group-hover:text-white transition-all duration-200 flex items-center gap-1 shrink-0"
                        >
                          <span>Open Logs</span>
                          <span>➔</span>
                        </button>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
