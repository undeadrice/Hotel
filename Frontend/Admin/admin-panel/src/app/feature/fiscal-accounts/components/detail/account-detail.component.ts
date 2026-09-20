import { Component, ChangeDetectionStrategy, inject, signal, computed, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { finalize } from 'rxjs';
import { FiscalAccountService } from '../../services/fiscal-account.service';
import { FiscalAccountDetailResponse } from '../../models/fiscal-account-detail.response';
import { FolioResponse } from '../../models/folio.response';
import { CreateFolioDialogComponent } from '../dialogs/create-folio-dialog.component';
import { AddFolioItemDialogComponent } from '../dialogs/add-folio-item-dialog.component';
import { FolioItemType } from '../../enums/folio-item-type.enum';
import { FolioStatus } from '../../enums/folio-status.enum';
import { FiscalAccountStatus } from '../../enums/fiscal-account-status.enum';

@Component({
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatIconModule,
    MatDialogModule,
    MatProgressBarModule,
    MatTableModule,
  ],
  templateUrl: './account-detail.component.html',
  styleUrls: ['./account-detail.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccountDetailComponent implements OnInit {
  private readonly accountService = inject(FiscalAccountService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly account = signal<FiscalAccountDetailResponse | null>(null);
  readonly loading = signal(true);
  readonly FolioStatus = FolioStatus;
  readonly FiscalAccountStatus = FiscalAccountStatus;
  readonly folioDisplayedColumns: string[] = ['description', 'type', 'amount', 'totalAmount', 'businessDate', 'createdAt'];

  readonly canCheckOut = computed(() => {
    const acc = this.account();
    if (!acc) {
      return false;
    }

    if (acc.status !== FiscalAccountStatus.Open) {
      return false;
    }

    return acc.folios.length > 0 && acc.folios.every((folio) => folio.status === FolioStatus.Settled);
  });

  fiscalAccountStatusLabel(status: FiscalAccountStatus): string {
    switch (status) {
      case FiscalAccountStatus.CheckedOut:
        return 'Checked out';
      case FiscalAccountStatus.Open:
        return 'Open';
      default:
        return 'Invalid';
    }
  }

  folioStatusLabel(status: FolioStatus): string {
    switch (status) {
      case FolioStatus.Settled:
        return 'Settled';
      case FolioStatus.Open:
      default:
        return 'Open';
    }
  }

  folioItemTypeLabel(type: FolioItemType): string {
    switch (type) {
      case FolioItemType.Charge:
        return 'Charge';
      case FolioItemType.Payment:
        return 'Payment';
      default:
        return 'Unknown';
    }
  }

  canSettleFolio(folio: FolioResponse): boolean {
    if (folio.status === FolioStatus.Settled) {
      return false;
    }

    if (folio.items.length === 0) {
      return true;
    }

    const payments = folio.items
      .filter((item) => item.transactionGroupType === FolioItemType.Payment)
      .reduce((sum, item) => sum + item.totalAmount, 0);

    const charges = folio.items
      .filter((item) => item.transactionGroupType === FolioItemType.Charge)
      .reduce((sum, item) => sum + item.totalAmount, 0);

    return payments === charges && charges !== 0;
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.router.navigate(['/fiscal-accounts']);
      return;
    }

    this.accountService
      .getAccount(id)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe((data) => {
        this.account.set(data);
      });
  }

  checkOutAccount(): void {
    const currentAccount = this.account();
    if (!currentAccount) return;

    this.accountService.checkOut(currentAccount.id).subscribe(() => {
      this.snackBar.open('Account checked out', 'Close', { duration: 3000 });
      this.refreshAccount();
    });
  }

  postDailyCharges(): void {
    const currentAccount = this.account();
    if (!currentAccount) return;

    this.accountService.postRoomCharge(currentAccount.originatorId).subscribe(() => {
      this.snackBar.open('Room charge posted', 'Close', { duration: 3000 });
      this.refreshAccount();
    });
  }

  openCreateFolioDialog(): void {
    const currentAccount = this.account();
    if (!currentAccount) return;

    const dialogRef = this.dialog.open(CreateFolioDialogComponent, {
      width: '400px',
      data: { fiscalAccountId: currentAccount.id },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.refreshAccount();
      }
    });
  }

  goToReservation(): void {
    // Reservation details page is not implemented yet.
    void 0;
  }

  settleFolio(folio: FolioResponse): void {
    const currentAccount = this.account();
    if (!currentAccount) return;

    this.accountService
      .settleFolio({ accountId: currentAccount.id, folioId: folio.id })
      .subscribe(() => {
        this.snackBar.open('Folio settled', 'Close', { duration: 3000 });
        this.refreshAccount();
      });
  }

  openAddFolioItemDialog(folio: FolioResponse): void {
    const dialogRef = this.dialog.open(AddFolioItemDialogComponent, {
      width: '400px',
      data: { folioId: folio.id },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.refreshAccount();
      }
    });
  }

  private refreshAccount(): void {
    const currentAccount = this.account();
    if (!currentAccount) return;

    this.loading.set(true);
    this.accountService
      .getAccount(currentAccount.id)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe((data) => {
        this.account.set(data);
      });
  }
}