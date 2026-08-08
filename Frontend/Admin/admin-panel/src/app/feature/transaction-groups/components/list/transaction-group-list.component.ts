import { Component, ChangeDetectionStrategy, inject, computed } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TransactionGroupService } from '../../services/transaction-group.service';
import { TransactionType } from '../../enums/transaction-type.enum';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { TransactionGroupListResponse } from '../../models/responses/transaction-group-list.response';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './transaction-group-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransactionGroupListComponent {
  private readonly transactionGroupService = inject(TransactionGroupService);
  private readonly router = inject(Router);

  transactionGroups = new MatTableDataSource<TransactionGroupListResponse>([]);
  displayedColumns: string[] = [
    'Code',
    'Name',
    'Type',
    'TransactionCodesCount',
    'IsActive',
    'actions',
  ];

  readonly transactionTypeEnum = TransactionType;
  readonly transactionTypeValues = computed(() =>
    Object.entries(TransactionType)
      .filter(([key]) => isNaN(Number(key)))
      .reduce(
        (acc, [key, value]) => {
          acc[value as number] = key;
          return acc;
        },
        {} as Record<number, string>
      )
  );

  constructor() {
    this.transactionGroupService.getTransactionGroups().subscribe((data) => {
      this.transactionGroups.data = data;
    });
  }

  getTransactionTypeName(typeValue: number): string {
    return this.transactionTypeValues()[typeValue] || String(typeValue);
  }

  editTransactionGroup(id: string): void {
    this.router.navigate(['transaction-groups/edit', id]);
  }
}
