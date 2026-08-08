import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { RatePlanResponse } from '../models/responses/rate-plan.response';
import { RatePlanListResponse } from '../models/responses/rate-plan-list.response';
import { CreateRatePlanRequest } from '../models/requests/create-rate-plan.request';

@Injectable({
  providedIn: 'root',
})
export class RatePlanService extends BaseHttpService {
  getRatePlans(): Observable<RatePlanListResponse[]> {
    return this.get<RatePlanListResponse[]>('RatePlans');
  }

  getRatePlan(id: string): Observable<RatePlanResponse> {
    return this.get<RatePlanResponse>(`RatePlans/${id}`);
  }

  createRatePlan(request: CreateRatePlanRequest): Observable<string> {
    return this.post<string>('RatePlans', request);
  }
}