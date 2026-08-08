import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RatePlanService } from '../../services/rate-plan.service';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { RatePlanListResponse } from '../../models/responses/rate-plan-list.response';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './rate-plan-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RatePlanListComponent {
  private readonly ratePlanService = inject(RatePlanService);
  private readonly router = inject(Router);

  ratePlans = new MatTableDataSource<RatePlanListResponse>([]);
  displayedColumns: string[] = [
    'Name',
    'StartDate',
    'EndDate',
    'IsActive',
    'actions',
  ];

  constructor() {
    this.ratePlanService.getRatePlans().subscribe((data) => {
      this.ratePlans.data = data;
    });
  }
}