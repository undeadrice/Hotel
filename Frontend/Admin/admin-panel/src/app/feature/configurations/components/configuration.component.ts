import { Component, ChangeDetectionStrategy, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfigurationService } from '../services/configuration.service';
import { SHARED_IMPORTS } from '../../../../shared-module';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

interface TimeZoneOption {
  value: string;
  label: string;
}

const TIME_ZONES: TimeZoneOption[] = [
  { value: 'Poland Standard Time', label: 'Poland (Warsaw)' },
  { value: 'UTC', label: 'UTC' },
  { value: 'GMT Standard Time', label: 'London (GMT)' },
  { value: 'Central Europe Standard Time', label: 'Central Europe (Budapest, Prague)' },
  { value: 'Romance Standard Time', label: 'Paris, Madrid, Rome' },
  { value: 'Eastern Standard Time', label: 'New York (EST)' },
  { value: 'Central Standard Time', label: 'Chicago (CST)' },
  { value: 'Pacific Standard Time', label: 'Los Angeles (PST)' },
  { value: 'China Standard Time', label: 'Beijing (CST)' },
  { value: 'India Standard Time', label: 'Mumbai (IST)' },
  { value: 'Tokyo Standard Time', label: 'Tokyo (JST)' },
];

@Component({
  imports: [
    ...SHARED_IMPORTS,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  templateUrl: './configuration.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfigurationComponent implements OnInit {
  private readonly configurationService = inject(ConfigurationService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly fb = inject(FormBuilder);

  readonly loading = signal(true);
  readonly saving = signal(false);

  readonly timeZones = TIME_ZONES;

  readonly form: FormGroup = this.fb.group({
    timeZoneId: ['Poland Standard Time', [Validators.required]],
    currentBusinessDate: [this.today(), [Validators.required]],
  });

  ngOnInit(): void {
    this.configurationService.getConfiguration().subscribe({
      next: (configuration) => {
        if (configuration) {
          this.form.patchValue({
            timeZoneId: configuration.timeZoneId,
            currentBusinessDate: configuration.currentBusinessDate,
          });
        }
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Failed to load configuration', 'Close', { duration: 5000 });
        this.loading.set(false);
      },
    });
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    this.saving.set(true);

    const value = this.form.value;
    const currentBusinessDate = this.toDateOnly(value.currentBusinessDate);

    this.configurationService
      .upsertConfiguration({
        timeZoneId: value.timeZoneId,
        currentBusinessDate,
      })
      .subscribe({
        next: () => {
          this.saving.set(false);
          this.snackBar.open('Configuration saved successfully', 'Close', { duration: 3000 });
        },
        error: () => {
          this.saving.set(false);
          this.snackBar.open('Failed to save configuration', 'Close', { duration: 5000 });
        },
      });
  }

  private today(): Date {
    return new Date();
  }

  private toDateOnly(value: unknown): string {
    if (value instanceof Date) {
      const year = value.getFullYear();
      const month = String(value.getMonth() + 1).padStart(2, '0');
      const day = String(value.getDate()).padStart(2, '0');
      return `${year}-${month}-${day}`;
    }

    if (typeof value === 'string') {
      return value;
    }

    return '';
  }
}