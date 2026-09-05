import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FiscalAccountService } from '../../services/fiscal-account.service';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { FiscalAccountListResponse } from '../../models/fiscal-account-list.response';
import { FiscalAccountStatus } from '../../enums/fiscal-account-status.enum';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './account-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccountListComponent {
  private readonly accountService = inject(FiscalAccountService);
  private readonly router = inject(Router);

  accounts = new MatTableDataSource<FiscalAccountListResponse>([]);
  displayedColumns: string[] = [
    'cycleIdentifier',
    'ownerFullName',
    'createdAt',
    'status',
    'actions',
  ];

  statusLabel(status: FiscalAccountStatus): string {
    switch (status) {
      case FiscalAccountStatus.CheckedOut:
        return 'Checked out';
      case FiscalAccountStatus.Open:
        return 'Open';
      default:
        return 'Invalid';
    }
  }

  constructor() {
    this.accountService.getAccounts().subscribe((data) => {
      this.accounts.data = data;
    });
  }

  viewAccount(id: string): void {
    this.router.navigate(['fiscal-accounts', id]);
  }
}