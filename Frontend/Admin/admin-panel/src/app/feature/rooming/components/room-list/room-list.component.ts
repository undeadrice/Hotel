import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RoomService } from '../../services/room.service';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { RoomListResponse } from '../../models/responses/room-list.response';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './room-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoomListComponent {
  private readonly roomService = inject(RoomService);
  private readonly router = inject(Router);

  rooms = new MatTableDataSource<RoomListResponse>([]);
  displayedColumns: string[] = ['roomNumber', 'roomType', 'actions'];

  constructor() {
    this.roomService.getRooms().subscribe((data) => {
      this.rooms.data = data;
    });
  }

  edit(id: string): void {
    this.router.navigate(['rooms/edit', id]);
  }

  deactivate(id: string): void {
    this.roomService.deactivateRoom(id).subscribe(() => {
      this.rooms.data = this.rooms.data.filter((r) => r.id !== id);
    });
  }
}