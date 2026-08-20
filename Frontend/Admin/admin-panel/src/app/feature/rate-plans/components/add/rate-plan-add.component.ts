import {
  Component,
  ChangeDetectionStrategy,
  signal,
  inject,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormArray,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatNativeDateModule } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { finalize } from 'rxjs';
import { RatePlanService } from '../../services/rate-plan.service';
import { TransactionCodeService } from '../../../transaction-codes/services/transaction-code.service';
import { RoomService } from '../../../rooming/services/room.service';
import { TransactionCodeListResponse } from '../../../transaction-codes/models/responses/transaction-code-list.response';
import { RoomTypeListResponse } from '../../../rooming/models/responses/room-type-list.response';
import { CreateRatePlanRequest } from '../../models/requests/create-rate-plan.request';

@Component({
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule,
  ],
  templateUrl: './rate-plan-add.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RatePlanAddComponent {
  private readonly fb = inject(FormBuilder);
  private readonly ratePlanService = inject(RatePlanService);
  private readonly transactionCodeService = inject(TransactionCodeService);
  private readonly roomService = inject(RoomService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly router = inject(Router);

  readonly transactionCodes = signal<TransactionCodeListResponse[]>([]);
  readonly roomTypes = signal<RoomTypeListResponse[]>([]);

  readonly form: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    transactionCodeId: ['', [Validators.required]],
    startDate: ['', [Validators.required]],
    endDate: ['', [Validators.required]],
    rooms: this.fb.array([]),
  });

  readonly submitting = signal(false);

  constructor() {
    this.transactionCodeService.getTransactionCodes().subscribe((codes) => {
      this.transactionCodes.set(codes);
    });

    this.roomService.getRoomTypes().subscribe((types) => {
      this.roomTypes.set(types);
    });

    this.addRoom();
  }

  get rooms(): FormArray {
    return this.form.get('rooms') as FormArray;
  }

  get selectedRoomTypeIds(): string[] {
    return this.rooms.controls
      .map((ctrl) => ctrl.get('roomTypeId')?.value)
      .filter((id): id is string => id !== null && id !== '');
  }

  availableRoomTypes(currentIndex: number): RoomTypeListResponse[] {
    const selectedIds = this.rooms.controls
      .map((ctrl, i) => (i !== currentIndex ? ctrl.get('roomTypeId')?.value : null))
      .filter((id): id is string => id !== null && id !== '');
    return this.roomTypes().filter((rt) => !selectedIds.includes(rt.id));
  }

  addRoom(): void {
    const roomGroup = this.fb.group({
      roomTypeId: ['', [Validators.required]],
      price: ['', [Validators.required, Validators.min(0.01)]],
    });
    this.rooms.push(roomGroup);
  }

  removeRoom(index: number): void {
    if (this.rooms.length > 1) {
      this.rooms.removeAt(index);
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);

    const formValue = this.form.value;
    const request: CreateRatePlanRequest = {
      name: formValue.name,
      transactionCodeId: formValue.transactionCodeId,
      startDate: formValue.startDate,
      endDate: formValue.endDate,
      rooms: formValue.rooms.map(
        (room: { roomTypeId: string; price: number }) => ({
          roomTypeId: room.roomTypeId,
          price: room.price,
        })
      ),
    };

    this.ratePlanService
      .createRatePlan(request)
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe(() => {
        this.snackBar.open('Rate plan created successfully', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/rate-plans']);
      });
  }
}