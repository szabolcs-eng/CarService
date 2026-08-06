import type { ReactElement } from 'react';
import { Navigate } from 'react-router-dom';
import { isAuthenticated } from '../lib/auth';

interface ProtectedRouteProps {
  children: ReactElement;
}

/** Route guard shared by every authenticated page, instead of each page
 * re-implementing its own "redirect if no token" check in a useEffect. */
export default function ProtectedRoute({ children }: ProtectedRouteProps) {
  if (!isAuthenticated()) {
    return <Navigate to="/login" replace />;
  }
  return children;
}
