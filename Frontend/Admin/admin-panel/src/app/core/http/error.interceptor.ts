import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {

      console.log(error);

      if (error.status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('token-valid-to');
        router.navigate(['/login']);
        snackBar.open('Session expired. Please log in again.', 'Close', {
          duration: 5000,
          panelClass: 'snackbar-error',
        });
        return throwError(() => error);
      }

      const detail = extractErrorMessage(error);
      snackBar.open(detail, 'Close', {
        duration: 5000,
        panelClass: 'snackbar-error',
      });

      return throwError(() => error);
    }),
  );
};

function extractErrorMessage(error: HttpErrorResponse): string {
  const body: unknown = error.error;

  if (body && typeof body === 'object') {
    // ValidationProblemDetails: FluentValidation returns property -> messages map.
    const errors = (body as { errors?: Record<string, string[]> }).errors;
    if (errors && typeof errors === 'object') {
      const messages = Object.values(errors)
        .flat()
        .filter((message): message is string => typeof message === 'string');
      if (messages.length > 0) {
        return messages.join(' ');
      }
    }

    // ProblemDetails: domain/application exceptions return a detail message.
    const detail = (body as { detail?: unknown }).detail;
    if (typeof detail === 'string' && detail.trim().length > 0) {
      return detail;
    }

    // Fall back to the response title when no detail is provided.
    const title = (body as { title?: unknown }).title;
    if (typeof title === 'string' && title.trim().length > 0) {
      return title;
    }
  }

  if (typeof body === 'string' && body.trim().length > 0) {
    return body;
  }

  return 'An unexpected error occurred.';
}
