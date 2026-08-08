import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { GuestService } from '../../services/guest.service';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { GuestListResponse } from '../../models/responses/guest-list.response';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './guest-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GuestListComponent {
  private readonly guestService = inject(GuestService);
  private readonly router = inject(Router);

  guests = new MatTableDataSource<GuestListResponse>([]);
  displayedColumns: string[] = [
    'FullName',
    'Phone',
    'Email',
    'DocumentNumber',
    'actions',
  ];

  constructor() {
    this.guestService.getGuests().subscribe((data) => {
      this.guests.data = data;
    });
  }

  editGuest(id: string): void {
    this.router.navigate(['guests/edit', id]);
  }
}