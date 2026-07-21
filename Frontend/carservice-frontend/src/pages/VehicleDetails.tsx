import { useEffect, useState, useCallback } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../api";
import axios from "axios";

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
  const [error, setError] = useState("");

  const [avgConsumption, setAvgConsumption] =
    useState<AvgConsumptionData | null>(null);
  const [avgError, setAvgError] = useState("");

  const [fuelDate, setFuelDate] = useState(
    new Date().toISOString().split("T")[0],
  );
  const [fuelKm, setFuelKm] = useState<number | "">("");
  const [fuelAmount, setFuelAmount] = useState<number | "">("");
  const [fuelCost, setFuelCost] = useState<number | "">("");

  const [serviceDate, setServiceDate] = useState(
    new Date().toISOString().split("T")[0],
  );
  const [serviceKm, setServiceKm] = useState<number | "">("");
  const [serviceDesc, setServiceDesc] = useState("");
  const [serviceCost, setServiceCost] = useState<number | "">("");

  const [editingFuelId, setEditingFuelId] = useState<number | null>(null);
  const [editingServiceId, setEditingServiceId] = useState<number | null>(null);

  const fetchLogs = useCallback(async () => {
    try {
      const [fuelRes, serviceRes] = await Promise.all([
        api.get(`/FuelLog/vehicle/${id}`),
        api.get(`/ServiceLog/vehicle/${id}`),
      ]);
      setFuelLogs(fuelRes.data.data || fuelRes.data || []);
      setServiceLogs(serviceRes.data.data || serviceRes.data || []);
    } catch (err) {
      setError("Error fetching logs. Please check the backend!");
    }
  }, [id]);

  const fetchAverageConsumption = useCallback(async () => {
    try {
      const response = await api.get(
        `/FuelLog/vehicle/${id}/average-consumption`,
      );
      setAvgConsumption(response.data);
      setAvgError("");
    } catch (err: unknown) {
      setAvgConsumption(null);
      if (
        axios.isAxiosError(err) &&
        err.response?.data &&
        typeof err.response.data === "string"
      ) {
        setAvgError(err.response.data);
      } else {
        setAvgError(
          "Error fetching average consumption. Please check the backend!",
        );
      }
    }
  }, [id]);

  useEffect(() => {
    if (!localStorage.getItem("token")) {
      navigate("/login");
    } else {
      fetchLogs();
      fetchAverageConsumption();
    }
  }, [id, navigate, fetchLogs, fetchAverageConsumption]);

  const handleAddFuel = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const payload = {
        vehicleId: Number(id),
        date: fuelDate,
        carKmCount: Number(fuelKm),
        fuelAmount: Number(fuelAmount),
        fuelCost: Number(fuelCost),
      };

      if (editingFuelId !== null) {
        // Frissítés[cite: 6]
        await api.put(`/FuelLog/update/${editingFuelId}`, payload);
        setEditingFuelId(null);
      } else {
        // Új hozzáadás[cite: 6]
        await api.post("/FuelLog/add", payload);
      }

      setFuelKm("");
      setFuelAmount("");
      setFuelCost("");
      fetchLogs();
      fetchAverageConsumption();
    } catch (err) {
      setError(
        "Error occurred while saving fuel log. Please check the backend!",
      );
    }
  };

  const handleDeleteFuel = async (logId: number) => {
    if (!window.confirm("Are you sure you want to delete this fuel log?"))
      return;
    try {
      await api.delete(`/FuelLog/delete/${logId}`);
      fetchLogs();
      fetchAverageConsumption();
    } catch (err) {
      setError(
        "Error occurred while deleting fuel log. Please check the backend!",
      );
    }
  };

  const startEditingFuel = (log: FuelLog) => {
    setEditingFuelId(log.id);
    setFuelDate(log.date.split("T")[0]);
    setFuelKm(log.carKmCount);
    setFuelAmount(log.fuelAmount);
    setFuelCost(log.fuelCost);
  };

  const cancelEditingFuel = () => {
    setEditingFuelId(null);
    setFuelDate(new Date().toISOString().split("T")[0]);
    setFuelKm("");
    setFuelAmount("");
    setFuelCost("");
  };

  const handleAddService = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const payload = {
        vehicleId: Number(id),
        date: serviceDate,
        carKmCount: Number(serviceKm),
        serviceDescription: serviceDesc,
        serviceCost: Number(serviceCost),
      };

      if (editingServiceId !== null) {
        // Frissítés[cite: 7]
        await api.put(`/ServiceLog/update/${editingServiceId}`, payload);
        setEditingServiceId(null);
      } else {
        // Új hozzáadás[cite: 7]
        await api.post("/ServiceLog/add", payload);
      }

      setServiceKm("");
      setServiceDesc("");
      setServiceCost("");
      fetchLogs();
    } catch (err) {
      setError(
        "Error occurred while saving service log. Please check the backend!",
      );
    }
  };

  const handleDeleteService = async (logId: number) => {
    if (!window.confirm("Are you sure you want to delete this service log?"))
      return;
    try {
      await api.delete(`/ServiceLog/delete/${logId}`);
      fetchLogs();
    } catch (err) {
      setError(
        "Error occurred while deleting service log. Please check the backend!",
      );
    }
  };

  const startEditingService = (log: ServiceLog) => {
    setEditingServiceId(log.id);
    setServiceDate(log.date.split("T")[0]);
    setServiceKm(log.carKmCount);
    setServiceDesc(log.serviceDescription);
    setServiceCost(log.serviceCost);
  };

  const cancelEditingService = () => {
    setEditingServiceId(null);
    setServiceDate(new Date().toISOString().split("T")[0]);
    setServiceKm("");
    setServiceDesc("");
    setServiceCost("");
  };

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100 pb-16">
      <nav className="border-b border-slate-800 bg-slate-900/50 backdrop-blur-lg sticky top-0 z-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
          <button
            onClick={() => navigate("/dashboard")}
            className="flex items-center gap-2 px-3 py-1.5 rounded-xl bg-slate-800/80 hover:bg-slate-800 text-slate-300 hover:text-white text-xs font-semibold border border-slate-700/50 transition-all duration-200"
          >
            <span>⬅</span>
            <span>Back to Garage</span>
          </button>
          <span className="font-bold text-lg tracking-tight bg-gradient-to-r from-white to-slate-400 bg-clip-text text-transparent">
            Vehicle Telemetry & Logs
          </span>
          <div className="hidden sm:block w-20 shrink-0"></div>
        </div>
      </nav>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        {error && (
          <div className="mb-6 bg-red-950/50 border border-red-500/50 text-red-300 px-4 py-3 rounded-xl text-sm">
            ⚠️ {error}
          </div>
        )}

        <div className="mb-8 bg-gradient-to-r from-slate-900 via-slate-900 to-emerald-950/30 border border-emerald-500/30 rounded-3xl p-6 shadow-2xl relative overflow-hidden">
          <div className="absolute -right-10 -bottom-10 w-40 h-40 bg-emerald-500/10 rounded-full blur-3xl pointer-events-none"></div>

          <div className="flex flex-col md:flex-row md:items-center justify-between gap-6 relative z-10">
            <div>
              <span className="px-3 py-1 rounded-full bg-emerald-500/10 text-emerald-400 border border-emerald-500/20 text-xs font-mono uppercase tracking-widest">
                Analytics Engine
              </span>
              <h3 className="text-2xl font-extrabold text-white mt-2">
                Average Fuel Consumption
              </h3>
              <p className="text-sm text-slate-400">
                Calculated based on odometer differences between full refills
              </p>
            </div>

            {avgConsumption ? (
              <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 bg-slate-950/60 border border-slate-800/80 px-4 sm:px-6 py-4 rounded-2xl backdrop-blur-md">
                <div className="text-center border-b sm:border-b-0 sm:border-r border-slate-800/80 pb-2 sm:pb-0 sm:pr-4">
                  <span className="block text-xs text-slate-400 uppercase font-medium">
                    Distance
                  </span>
                  <span className="text-lg sm:text-xl font-bold text-slate-200">
                    {avgConsumption.totalDistanceKm}{" "}
                    <small className="text-xs text-slate-500">km</small>
                  </span>
                </div>
                <div className="text-center border-b sm:border-b-0 sm:border-r border-slate-800/80 py-2 sm:py-0 sm:px-4">
                  <span className="block text-xs text-slate-400 uppercase font-medium">
                    Fuel Used
                  </span>
                  <span className="text-lg sm:text-xl font-bold text-slate-200">
                    {avgConsumption.totalFuelUsedLiters}{" "}
                    <small className="text-xs text-slate-500">L</small>
                  </span>
                </div>
                <div className="text-center pt-2 sm:pt-0 sm:pl-4">
                  <span className="block text-xs text-emerald-400 uppercase font-bold">
                    Average
                  </span>
                  <span className="text-xl sm:text-2xl font-extrabold text-emerald-400">
                    {avgConsumption.averageConsumption}{" "}
                    <small className="text-xs font-normal text-emerald-500">
                      L/100km
                    </small>
                  </span>
                </div>
              </div>
            ) : (
              <div className="bg-slate-950/40 border border-slate-800/80 px-6 py-4 rounded-2xl flex items-center gap-3 text-sm text-slate-400">
                <span className="text-xl">ℹ️</span>
                <span>
                  {avgError ||
                    "At least 2 fuel logs are required for calculation."}
                </span>
              </div>
            )}
          </div>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
          <div className="space-y-6">
            <div className="flex items-center gap-2 border-b border-slate-800 pb-3">
              <span className="text-2xl">⛽</span>
              <h4 className="text-lg font-bold text-emerald-400">
                Fuel Refill Logs
              </h4>
            </div>

            <div className="bg-slate-900/60 border border-slate-800 rounded-2xl p-5 shadow-lg">
              <h5 className="text-xs font-bold text-slate-300 uppercase tracking-wider mb-3">
                Add Refill Record
              </h5>
              <form onSubmit={handleAddFuel} className="grid grid-cols-2 gap-3">
                <div className="col-span-2 sm:col-span-1">
                  <input
                    type="date"
                    required
                    value={fuelDate}
                    onChange={(e) => setFuelDate(e.target.value)}
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-emerald-500"
                  />
                </div>
                <div className="col-span-2 sm:col-span-1">
                  <input
                    type="number"
                    required
                    placeholder="Odometer (km)"
                    value={fuelKm}
                    onChange={(e) => setFuelKm(Number(e.target.value))}
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-emerald-500"
                  />
                </div>
                <div className="col-span-1">
                  <input
                    type="number"
                    step="0.1"
                    required
                    placeholder="Liters (L)"
                    value={fuelAmount}
                    onChange={(e) => setFuelAmount(Number(e.target.value))}
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-emerald-500"
                  />
                </div>
                <div className="col-span-1">
                  <input
                    type="number"
                    required
                    placeholder="Cost (HUF)"
                    value={fuelCost}
                    onChange={(e) => setFuelCost(Number(e.target.value))}
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-emerald-500"
                  />
                </div>
                <div className="col-span-2 flex gap-2">
                  <button
                    type="submit"
                    className={`flex-1 py-2.5 rounded-xl text-xs font-bold text-white shadow-md transition-all ${
                      editingFuelId !== null
                        ? "bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-500"
                        : "bg-gradient-to-r from-emerald-600 to-teal-600 hover:from-emerald-500"
                    }`}
                  >
                    {editingFuelId !== null
                      ? "✏️ Update Fuel Record"
                      : "+ Add Fuel Record"}
                  </button>
                  {editingFuelId !== null && (
                    <button
                      type="button"
                      onClick={cancelEditingFuel}
                      className="px-3 py-2.5 rounded-xl text-xs font-bold bg-slate-800 text-slate-400 hover:bg-slate-700 transition-colors"
                    >
                      Cancel
                    </button>
                  )}
                </div>
              </form>
            </div>

            <div className="space-y-3">
              {fuelLogs.length === 0 ? (
                <p className="text-center text-sm text-slate-500 py-6">
                  No fuel records logged yet.
                </p>
              ) : (
                fuelLogs?.map((log) => (
                  <div
                    key={log.id}
                    className="bg-slate-900/40 border border-slate-800/80 rounded-xl p-4 flex items-center justify-between hover:border-slate-700/80 transition-colors"
                  >
                    <div>
                      <div className="flex items-center gap-2 mb-1">
                        <span className="text-xs font-bold text-slate-200">
                          {new Date(log.date).toLocaleDateString()}
                        </span>
                        <span className="text-[10px] font-mono px-2 py-0.5 bg-slate-800 text-slate-400 rounded-md border border-slate-700/50">
                          {log.carKmCount} km
                        </span>
                      </div>
                      <span className="text-sm font-semibold text-emerald-400">
                        {log.fuelAmount} Liters
                      </span>
                    </div>
                    <div className="text-right shrink-0 flex items-center gap-3">
                      <div>
                        <span className="text-sm font-bold text-slate-200">
                          {log.fuelCost.toLocaleString()}
                        </span>
                        <span className="text-xs text-slate-500 block">
                          HUF
                        </span>
                      </div>
                      <div className="flex flex-col sm:flex-row gap-1 border-l border-slate-800 pl-2">
                        <button
                          onClick={() => startEditingFuel(log)}
                          title="Edit"
                          className="p-1.5 rounded-lg bg-blue-500/10 text-blue-400 hover:bg-blue-500 hover:text-white transition-colors text-xs"
                        >
                          ✏️
                        </button>
                        <button
                          onClick={() => handleDeleteFuel(log.id)}
                          title="Delete"
                          className="p-1.5 rounded-lg bg-red-500/10 text-red-400 hover:bg-red-500 hover:text-white transition-colors text-xs"
                        >
                          🗑️
                        </button>
                      </div>
                    </div>
                  </div>
                ))
              )}
            </div>
          </div>

          <div className="space-y-6">
            <div className="flex items-center gap-2 border-b border-slate-800 pb-3">
              <span className="text-2xl">🔧</span>
              <h4 className="text-lg font-bold text-amber-400">
                Maintenance & Service Logs
              </h4>
            </div>

            <div className="bg-slate-900/60 border border-slate-800 rounded-2xl p-5 shadow-lg">
              <h5 className="text-xs font-bold text-slate-300 uppercase tracking-wider mb-3">
                Add Service Record
              </h5>
              <form
                onSubmit={handleAddService}
                className="grid grid-cols-2 gap-3"
              >
                <div className="col-span-2 sm:col-span-1">
                  <input
                    type="date"
                    required
                    value={serviceDate}
                    onChange={(e) => setServiceDate(e.target.value)}
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-amber-500"
                  />
                </div>
                <div className="col-span-2 sm:col-span-1">
                  <input
                    type="number"
                    required
                    placeholder="Odometer (km)"
                    value={serviceKm}
                    onChange={(e) => setServiceKm(Number(e.target.value))}
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-amber-500"
                  />
                </div>
                <div className="col-span-2">
                  <input
                    type="text"
                    required
                    placeholder="Description (e.g., Oil Change & Brake Pads)"
                    value={serviceDesc}
                    onChange={(e) => setServiceDesc(e.target.value)}
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-amber-500"
                  />
                </div>
                <div className="col-span-2">
                  <input
                    type="number"
                    required
                    placeholder="Total Cost (HUF)"
                    value={serviceCost}
                    onChange={(e) => setServiceCost(Number(e.target.value))}
                    className="w-full px-3 py-2 bg-slate-950 border border-slate-800 rounded-xl text-xs text-slate-200 focus:outline-none focus:ring-1 focus:ring-amber-500"
                  />
                </div>
                <div className="col-span-2 flex gap-2">
                  <button
                    type="submit"
                    className={`flex-1 py-2.5 rounded-xl text-xs font-bold text-white shadow-md transition-all ${
                      editingServiceId !== null
                        ? "bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-500"
                        : "bg-gradient-to-r from-amber-600 to-orange-600 hover:from-amber-500"
                    }`}
                  >
                    {editingServiceId !== null
                      ? "✏️ Update Maintenance Record"
                      : "+ Add Maintenance Record"}
                  </button>
                  {editingServiceId !== null && (
                    <button
                      type="button"
                      onClick={cancelEditingService}
                      className="px-3 py-2.5 rounded-xl text-xs font-bold bg-slate-800 text-slate-400 hover:bg-slate-700 transition-colors"
                    >
                      Cancel
                    </button>
                  )}
                </div>
              </form>
            </div>

            <div className="space-y-3">
              {serviceLogs.length === 0 ? (
                <p className="text-center text-sm text-slate-500 py-6">
                  No service records logged yet.
                </p>
              ) : (
                serviceLogs?.map((log) => (
                  <div
                    key={log.id}
                    className="bg-slate-900/40 border border-slate-800/80 rounded-xl p-4 flex items-center justify-between gap-3 hover:border-slate-700/80 transition-colors"
                  >
                    <div className="min-w-0 flex-1">
                      <div className="flex items-center gap-2 mb-1">
                        <span className="text-xs font-bold text-slate-200">
                          {new Date(log.date).toLocaleDateString()}
                        </span>
                        <span className="text-[10px] font-mono px-2 py-0.5 bg-slate-800 text-slate-400 rounded-md border border-slate-700/50">
                          {log.carKmCount} km
                        </span>
                      </div>
                      <span className="text-sm font-medium text-slate-300 block break-words">
                        {log.serviceDescription}
                      </span>
                    </div>
                    <div className="text-right shrink-0 ml-2 flex items-center gap-3">
                      <div>
                        <span className="text-sm font-bold text-amber-400">
                          {log.serviceCost.toLocaleString()}
                        </span>
                        <span className="text-xs text-slate-500 block">
                          HUF
                        </span>
                      </div>
                      <div className="flex flex-col sm:flex-row gap-1 border-l border-slate-800 pl-2">
                        <button
                          onClick={() => startEditingService(log)}
                          title="Edit"
                          className="p-1.5 rounded-lg bg-blue-500/10 text-blue-400 hover:bg-blue-500 hover:text-white transition-colors text-xs"
                        >
                          ✏️
                        </button>
                        <button
                          onClick={() => handleDeleteService(log.id)}
                          title="Delete"
                          className="p-1.5 rounded-lg bg-red-500/10 text-red-400 hover:bg-red-500 hover:text-white transition-colors text-xs"
                        >
                          🗑️
                        </button>
                      </div>
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
