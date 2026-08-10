import { Component, ChangeDetectionStrategy, OnInit, signal, inject, computed } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { DashboardResponse } from '../models/responses/dashboard.response';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  imports: [CommonModule, MatCardModule, MatProgressBarModule, MatIconModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit {
  private readonly dashboardService = inject(DashboardService);

  readonly dashboard = signal<DashboardResponse | null>(null);
  readonly loading = signal(true);

  readonly occupancy = computed(() => this.dashboard()?.occupancyPercentage ?? 0);
  readonly gaugeDashArray = computed(() => {
    const pct = this.occupancy();
    const circumference = 2 * Math.PI * 54;
    const filled = (pct / 100) * circumference;
    return `${filled} ${circumference}`;
  });

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