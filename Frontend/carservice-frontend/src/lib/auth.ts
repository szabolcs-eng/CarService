import { jwtDecode } from 'jwt-decode';

interface CarServiceTokenClaims {
  // ASP.NET Core's default JWT claim URIs for id/name/role.
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'?: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
  exp?: number;
}

export interface CurrentUser {
  id: number;
  username: string;
  role: string;
}

export function getToken(): string | null {
  return localStorage.getItem('token');
}

export function setSession(token: string): void {
  localStorage.setItem('token', token);
}

export function clearSession(): void {
  localStorage.removeItem('token');
}

/** Decodes the current JWT and returns its claims, or null if there is no
 * token, it's malformed, or it has expired. Uses a real JWT library instead
 * of hand-rolled base64 parsing. */
export function getCurrentUser(): CurrentUser | null {
  const token = getToken();
  if (!token) return null;

  try {
    const claims = jwtDecode<CarServiceTokenClaims>(token);

    if (claims.exp && claims.exp * 1000 < Date.now()) {
      clearSession();
      return null;
    }

    const id = claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    const username = claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
    const role = claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (!id) return null;

    return { id: parseInt(id, 10), username: username ?? '', role: role ?? 'User' };
  } catch {
    clearSession();
    return null;
  }
}

export function isAuthenticated(): boolean {
  return getCurrentUser() !== null;
}
