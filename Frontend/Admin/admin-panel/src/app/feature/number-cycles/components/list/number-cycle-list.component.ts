import {
  Component,
  ChangeDetectionStrategy,
  inject,
  computed,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NumberCycleService } from '../../services/number-cycle.service';
import { NumberCycleTopic } from '../../enums/number-cycle-topic.enum';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { NumberCycleResponse } from '../../models/responses/number-cycle.response';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './number-cycle-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NumberCycleListComponent {
  private readonly numberCycleService = inject(NumberCycleService);
  private readonly snackBar = inject(MatSnackBar);

  numberCycles = new MatTableDataSource<NumberCycleResponse>([]);
  displayedColumns: string[] = [
    'Topic',
    'Prefix',
    'StartIndex',
    'CurrentIndex',
    'CreatedAt',
    'actions',
  ];

  readonly topicValues = computed(() =>
    Object.entries(NumberCycleTopic)
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
    this.numberCycleService.getNumberCycles().subscribe((data) => {
      this.numberCycles.data = data;
    });
  }

  getTopicName(topicValue: number): string {
    return this.topicValues()[topicValue] || String(topicValue);
  }

  deleteNumberCycle(id: string): void {
    this.numberCycleService.deleteNumberCycle(id).subscribe({
      next: () => {
        this.snackBar.open('Number cycle deleted successfully', 'Close', {
          duration: 3000,
        });
        this.numberCycleService.getNumberCycles().subscribe((data) => {
          this.numberCycles.data = data;
        });
      },
      error: () => {
        this.snackBar.open('Failed to delete number cycle', 'Close', {
          duration: 5000,
        });
      },
    });
  }
}
