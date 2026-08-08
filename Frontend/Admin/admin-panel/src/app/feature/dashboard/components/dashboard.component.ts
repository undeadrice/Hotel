import { Component, ChangeDetectionStrategy, OnInit, signal, inject } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { DashboardResponse } from '../models/responses/dashboard.response';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  imports: [CommonModule, MatCardModule, MatProgressBarModule, MatIconModule],
  templateUrl: './dashboard.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit {
  private readonly dashboardService = inject(DashboardService);

  readonly dashboard = signal<DashboardResponse | null>(null);
  readonly loading = signal(true);

  ngOnInit(): void {
    this.dashboardService.getDashboard().subscribe({
      next: (data) => {
        this.dashboard.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }
}