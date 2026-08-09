import { Component, inject, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FiscalAccountService } from '../../services/fiscal-account.service';

@Component({
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatProgressBarModule,
  ],
  template: `
    <h2 mat-dialog-title>Add Folio Item</h2>
    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-form-field appearance="outline" style="width: 100%">
          <mat-label>Description</mat-label>
          <input matInput formControlName="description" placeholder="e.g. Room charge" />
          @if (form.get('description')?.hasError('required')) {
            <mat-error>Description is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" style="width: 100%">
          <mat-label>Amount</mat-label>
          <input matInput type="number" formControlName="amount" placeholder="e.g. 199.99" step="0.01" />
          @if (form.get('amount')?.hasError('required')) {
            <mat-error>Amount is required</mat-error>
          } @else if (form.get('amount')?.hasError('min')) {
            <mat-error>Amount must be greater than 0</mat-error>
          }
        </mat-form-field>
      </form>
      @if (submitting()) {
        <mat-progress-bar mode="indeterminate"></mat-progress-bar>
      }
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close [disabled]="submitting()">Cancel</button>
      <button
        mat-raised-button
        color="primary"
        (click)="addItem()"
        [disabled]="form.invalid || submitting()"
      >
        Add item
      </button>
    </mat-dialog-actions>
  `,
})
export class AddFolioItemDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly accountService = inject(FiscalAccountService);
  private readonly dialogRef = inject(MatDialogRef<AddFolioItemDialogComponent>);
  private readonly snackBar = inject(MatSnackBar);
  readonly data: { folioId: string } = inject(MAT_DIALOG_DATA);

  readonly submitting = signal(false);

  readonly form: FormGroup = this.fb.group({
    description: ['', [Validators.required]],
    amount: ['', [Validators.required, Validators.min(0.01)]],
  });

  addItem(): void {
    if (this.form.invalid) return;

    this.submitting.set(true);
    this.accountService
      .createFolioItem({
        folioId: this.data.folioId,
        description: this.form.value.description,
        amount: this.form.value.amount,
      })
      .subscribe({
        next: () => {
          this.submitting.set(false);
          this.dialogRef.close(true);
        },
        error: () => {
          this.submitting.set(false);
          this.snackBar.open('Failed to add folio item', 'Close', { duration: 5000 });
        },
      });
  }
}