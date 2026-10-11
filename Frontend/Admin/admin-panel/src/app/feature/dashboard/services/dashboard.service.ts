import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { DashboardResponse } from '../models/responses/dashboard.response';

@Injectable({
  providedIn: 'root',
})
export class DashboardService extends BaseHttpService {
  getDashboard(): Observable<DashboardResponse> {
    return this.get<DashboardResponse>('dashboard');
  }
}