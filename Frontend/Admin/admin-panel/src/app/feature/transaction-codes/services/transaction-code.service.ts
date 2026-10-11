import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { TransactionCodeResponse } from '../models/responses/transaction-code.response';
import { TransactionCodeListResponse } from '../models/responses/transaction-code-list.response';
import { TransactionCodeSimpleListResponse } from '../models/responses/transaction-code-simple-list.response';
import { CreateTransactionCodeRequest } from '../models/requests/create-transaction-code.request';
import { UpdateTransactionCodeRequest } from '../models/requests/update-transaction-code.request';

@Injectable({
  providedIn: 'root',
})
export class TransactionCodeService extends BaseHttpService {
  getTransactionCodes(): Observable<TransactionCodeListResponse[]> {
    return this.get<TransactionCodeListResponse[]>('TransactionCodes');
  }

  getTransactionCodesSimpleList(): Observable<TransactionCodeSimpleListResponse[]> {
    return this.get<TransactionCodeSimpleListResponse[]>('TransactionCodes/simple-list');
  }

  getTransactionCode(id: string): Observable<TransactionCodeResponse> {
    return this.get<TransactionCodeResponse>(`TransactionCodes/${id}`);
  }

  createTransactionCode(
    request: CreateTransactionCodeRequest
  ): Observable<string> {
    return this.post<string>('TransactionCodes', request);
  }

  updateTransactionCode(
    request: UpdateTransactionCodeRequest
  ): Observable<void> {
    return this.put<void>('TransactionCodes', request);
  }
}
