import { Injectable, inject } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { AuthHttpService } from '../../../core/http/auth-http.service';
import { AuthService } from '../../../core/auth/auth.service';
import { TokenResponse } from '../models/responses/token.response';

@Injectable({
  providedIn: 'root',
})
export class AuthorizationService extends AuthHttpService {
  private readonly authService = inject(AuthService);

  login(email: string, password: string): Observable<string> {
    return this.post<TokenResponse>('auth/login', { email, password }).pipe(
      tap((response) => {
        this.authService.setToken(response.token);
      }),
      map((response) => response.token),
    );
  }
}