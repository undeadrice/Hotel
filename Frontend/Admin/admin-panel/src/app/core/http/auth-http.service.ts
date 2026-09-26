import { Injectable } from '@angular/core';
import { BaseHttpService } from './base-http.service';
import { AUTH_API_BASE_URL } from './api-config';

@Injectable({
  providedIn: 'root',
})
export class AuthHttpService extends BaseHttpService {
  protected override readonly baseUrl = AUTH_API_BASE_URL;
}
