import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { NumberCycleResponse } from '../models/responses/number-cycle.response';
import { CreateNumberCycleRequest } from '../models/requests/create-number-cycle.request';

@Injectable({
  providedIn: 'root',
})
export class NumberCycleService extends BaseHttpService {
  getNumberCycles(): Observable<NumberCycleResponse[]> {
    return this.get<NumberCycleResponse[]>('NumberCycles');
  }

  createNumberCycle(request: CreateNumberCycleRequest): Observable<string> {
    return this.post<string>('NumberCycles', request);
  }

  deleteNumberCycle(id: string): Observable<void> {
    return this.delete<void>(`NumberCycles/${id}`);
  }
}