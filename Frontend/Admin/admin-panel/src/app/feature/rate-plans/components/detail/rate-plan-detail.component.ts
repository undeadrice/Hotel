import { Component, ChangeDetectionStrategy, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RatePlanService } from '../../services/rate-plan.service';
import { RatePlanResponse } from '../../models/responses/rate-plan.response';
import { SHARED_IMPORTS } from '../../../../../shared-module';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './rate-plan-detail.component.html',
  styleUrls: ['./rate-plan-detail.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RatePlanDetailComponent implements OnInit {
  private readonly ratePlanService = inject(RatePlanService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly ratePlan = signal<RatePlanResponse | null>(null);
  readonly loading = signal(true);
  readonly roomDisplayedColumns: string[] = ['roomType', 'price'];

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.router.navigate(['/rate-plans']);
      return;
    }

    this.ratePlanService.getRatePlan(id).subscribe({
      next: (data) => {
        this.ratePlan.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Failed to load rate plan details', 'Close', {
          duration: 5000,
        });
        this.loading.set(false);
      },
    });
  }
}