import { Component, ChangeDetectionStrategy, inject, signal, OnInit } from '@angular/core';
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
import { FiscalAccountService } from '../../services/fiscal-account.service';
import { FiscalAccountDetailResponse } from '../../models/fiscal-account-detail.response';
import { FolioResponse } from '../../models/folio.response';
import { CreateFolioDialogComponent } from '../dialogs/create-folio-dialog.component';
import { AddFolioItemDialogComponent } from '../dialogs/add-folio-item-dialog.component';

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
  readonly folioDisplayedColumns: string[] = ['description', 'amount', 'createdAt'];

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.router.navigate(['/fiscal-accounts']);
      return;
    }

    this.accountService.getAccount(id).subscribe({
      next: (data) => {
        this.account.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Failed to load account details', 'Close', { duration: 5000 });
        this.loading.set(false);
      },
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
    this.accountService.getAccount(currentAccount.id).subscribe({
      next: (data) => {
        this.account.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Failed to refresh account', 'Close', { duration: 5000 });
        this.loading.set(false);
      },
    });
  }
}