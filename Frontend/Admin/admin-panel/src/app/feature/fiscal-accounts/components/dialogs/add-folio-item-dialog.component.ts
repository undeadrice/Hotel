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
import { MatSelectModule } from '@angular/material/select';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { finalize } from 'rxjs';
import { FiscalAccountService } from '../../services/fiscal-account.service';
import { TransactionCodeService } from '../../../transaction-codes/services/transaction-code.service';
import { TransactionCodeSimpleListResponse } from '../../../transaction-codes/models/responses/transaction-code-simple-list.response';

@Component({
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatProgressBarModule,
  ],
  template: `
    <h2 mat-dialog-title>Add Folio Item</h2>
    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-form-field appearance="outline" style="width: 100%">
          <mat-label>Transaction Code</mat-label>
          <mat-select formControlName="transactionCodeId">
            @for (code of transactionCodes(); track code.id) {
              <mat-option [value]="code.id">{{ code.name }}</mat-option>
            }
          </mat-select>
          @if (form.get('transactionCodeId')?.hasError('required')) {
            <mat-error>Transaction Code is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" style="width: 100%">
          <mat-label>Quantity</mat-label>
          <input matInput type="number" formControlName="quantity" placeholder="e.g. 1" step="1" min="1" />
          @if (form.get('quantity')?.hasError('required')) {
            <mat-error>Quantity is required</mat-error>
          } @else if (form.get('quantity')?.hasError('min')) {
            <mat-error>Quantity must be at least 1</mat-error>
          }
        </mat-form-field>

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
  private readonly transactionCodeService = inject(TransactionCodeService);
  private readonly dialogRef = inject(MatDialogRef<AddFolioItemDialogComponent>);
  readonly data: { folioId: string } = inject(MAT_DIALOG_DATA);

  readonly submitting = signal(false);
  readonly transactionCodes = signal<TransactionCodeSimpleListResponse[]>([]);

  readonly form: FormGroup = this.fb.group({
    transactionCodeId: ['', [Validators.required]],
    quantity: ['', [Validators.required, Validators.min(1)]],
    description: ['', [Validators.required]],
    amount: ['', [Validators.required, Validators.min(0.01)]],
  });

  constructor() {
    this.transactionCodeService
      .getTransactionCodesSimpleList()
      .subscribe((codes) => this.transactionCodes.set(codes));
  }

  addItem(): void {
    if (this.form.invalid) return;

    this.submitting.set(true);
    this.accountService
      .createFolioItem({
        folioId: this.data.folioId,
        transactionCodeId: this.form.value.transactionCodeId,
        quantity: this.form.value.quantity,
        description: this.form.value.description,
        amount: this.form.value.amount,
      })
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe(() => {
        this.dialogRef.close(true);
      });
  }
}