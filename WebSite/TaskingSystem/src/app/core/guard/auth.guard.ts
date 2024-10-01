import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { LocalStorageService } from '../services/local-storage/local-storage.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    return router.navigate(['/login']);
  }

  // Obtener roles permitidos desde la ruta
  const expectedRoles = route.data?.['roles'] as string[];

  // Si hay roles especificados en la ruta, verificar si el usuario tiene los permisos
  if (expectedRoles && !authService.hasRoles(expectedRoles)) {
    // Si el usuario no tiene los roles necesarios, redirigir a "Acceso denegado" o una página adecuada
    return router.navigate(['/access-denied']);
  }

  // Si está autenticado y tiene roles permitidos, permitir el acceso
  return true;
};
