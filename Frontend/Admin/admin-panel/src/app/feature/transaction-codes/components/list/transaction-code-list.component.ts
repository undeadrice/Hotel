import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TransactionCodeService } from '../../services/transaction-code.service';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { TransactionCodeListResponse } from '../../models/responses/transaction-code-list.response';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './transaction-code-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransactionCodeListComponent {
  private readonly transactionCodeService = inject(TransactionCodeService);
  private readonly router = inject(Router);

  transactionCodes = new MatTableDataSource<TransactionCodeListResponse>([]);
  displayedColumns: string[] = [
    'Code',
    'Name',
    'TransactionGroupName',
    'IsActive',
    'actions',
  ];

  constructor() {
    this.transactionCodeService.getTransactionCodes().subscribe((data) => {
      this.transactionCodes.data = data;
    });
  }

  editTransactionCode(id: string): void {
    this.router.navigate(['transaction-codes/edit', id]);
  }
}
