import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { TransactionGroupResponse } from '../models/responses/transaction-group.response';
import { TransactionGroupListResponse } from '../models/responses/transaction-group-list.response';
import { CreateTransactionGroupRequest } from '../models/requests/create-transaction-group.request';
import { UpdateTransactionGroupRequest } from '../models/requests/update-transaction-group.request';

@Injectable({
  providedIn: 'root',
})
export class TransactionGroupService extends BaseHttpService {
  getTransactionGroups(): Observable<TransactionGroupListResponse[]> {
    return this.get<TransactionGroupListResponse[]>('TransactionGroups');
  }

  getTransactionGroup(id: string): Observable<TransactionGroupResponse> {
    return this.get<TransactionGroupResponse>(`TransactionGroups/${id}`);
  }

  createTransactionGroup(
    request: CreateTransactionGroupRequest
  ): Observable<string> {
    return this.post<string>('TransactionGroups', request);
  }

  updateTransactionGroup(
    request: UpdateTransactionGroupRequest
  ): Observable<void> {
    return this.put<void>('TransactionGroups', request);
  }
}
