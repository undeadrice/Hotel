import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { FiscalAccountListResponse } from '../models/fiscal-account-list.response';
import { FiscalAccountDetailResponse } from '../models/fiscal-account-detail.response';
import { CreateFolioRequest } from '../models/requests/create-folio.request';
import { CreateFolioItemRequest } from '../models/requests/create-folio-item.request';
import { SettleFolioRequest } from '../models/requests/settle-folio.request';

@Injectable({
  providedIn: 'root',
})
export class FiscalAccountService extends BaseHttpService {
  getAccounts(): Observable<FiscalAccountListResponse[]> {
    return this.get<FiscalAccountListResponse[]>('FiscalAccounts');
  }

  getAccount(id: string): Observable<FiscalAccountDetailResponse> {
    return this.get<FiscalAccountDetailResponse>(`FiscalAccounts/${id}`);
  }

  createFolio(request: CreateFolioRequest): Observable<string> {
    return this.post<string>('Folios', request);
  }

  createFolioItem(request: CreateFolioItemRequest): Observable<string> {
    return this.post<string>('FolioItems', request);
  }

  settleFolio(request: SettleFolioRequest): Observable<void> {
    return this.post<void>('Folios/settle', request);
  }

  checkOut(id: string): Observable<void> {
    return this.post<void>(`FiscalAccounts/${id}/check-out`, {});
  }

  postRoomCharge(reservationId: string): Observable<string> {
    return this.post<string>(`FiscalAccounts/${reservationId}/post-room-charge`, {});
  }
}
