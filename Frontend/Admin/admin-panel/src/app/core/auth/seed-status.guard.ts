import { inject } from '@angular/core';
import { Router, type CanActivateFn } from '@angular/router';
import { map, catchError, of } from 'rxjs';
import { AuthService } from './auth.service';
import { ConfigurationService } from '../../feature/configurations/services/configuration.service';

export const seedStatusGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const configurationService = inject(ConfigurationService);
  const router = inject(Router);

  if (!authService.getToken()) {
    return router.parseUrl('/login');
  }

  return configurationService.getSeedStatus().pipe(
    map((isSeeded) => (isSeeded ? true : router.parseUrl('/setup'))),
    catchError(() => of(true)),
  );
};