import { Component, inject, signal } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FiscalAccountService } from '../../services/fiscal-account.service';

@Component({
  imports: [MatDialogModule, MatButtonModule, MatProgressBarModule],
  template: `
    <h2 mat-dialog-title>Create Folio</h2>
    <mat-dialog-content>
      <p>Are you sure you want to create a new folio for this account?</p>
      @if (submitting()) {
        <mat-progress-bar mode="indeterminate"></mat-progress-bar>
      }
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close [disabled]="submitting()">Cancel</button>
      <button
        mat-raised-button
        color="primary"
        (click)="createFolio()"
        [disabled]="submitting()"
      >
        Create
      </button>
    </mat-dialog-actions>
  `,
})
export class CreateFolioDialogComponent {
  private readonly accountService = inject(FiscalAccountService);
  private readonly dialogRef = inject(MatDialogRef<CreateFolioDialogComponent>);
  private readonly snackBar = inject(MatSnackBar);
  readonly data: { fiscalAccountId: string } = inject(MAT_DIALOG_DATA);

  readonly submitting = signal(false);

  createFolio(): void {
    this.submitting.set(true);
    this.accountService
      .createFolio({ fiscalAccountId: this.data.fiscalAccountId })
      .subscribe({
        next: () => {
          this.submitting.set(false);
          this.dialogRef.close(true);
        },
        error: () => {
          this.submitting.set(false);
          this.snackBar.open('Failed to create folio', 'Close', { duration: 5000 });
        },
      });
  }
}