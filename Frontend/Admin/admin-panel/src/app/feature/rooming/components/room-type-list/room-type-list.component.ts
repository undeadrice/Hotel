import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RoomTypeService } from '../../services/room-type.service';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { RoomTypeListResponse } from '../../models/responses/room-type-list.response';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './room-type-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoomTypeListComponent {
  private readonly roomTypeService = inject(RoomTypeService);
  private readonly router = inject(Router);

  roomTypes = new MatTableDataSource<RoomTypeListResponse>([]);
  displayedColumns: string[] = ['name', 'actions'];

  constructor() {
    this.roomTypeService.getRoomTypes().subscribe((data) => {
      this.roomTypes.data = data;
    });
  }

  edit(id: string): void {
    this.router.navigate(['room-types/edit', id]);
  }
}