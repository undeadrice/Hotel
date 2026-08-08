import { Routes } from '@angular/router';
import { AuthorizedLayoutComponent } from './shared/authorized/authorized-layout.component';
import { UnauthorizedLayoutComponent } from './shared/unauthorized/unauthorized-layout.component';
import { RoleListcomponent } from './feature/roles/components/list/role-list.component';
import { RoleAddComponent } from './feature/roles/components/add/role-add.component';
import { RoleEditComponent } from './feature/roles/components/edit/role-edit.component';
import { UserListComponent } from './feature/users/components/user-list/user-list.component';
import { UserAddComponent } from './feature/users/components/add/user-add.component';
import { UserEditComponent } from './feature/users/components/edit/user-edit.component';
import { LoginComponent } from './feature/authorization/components/login.component';
import { authGuard } from './core/auth/auth.guard';
import { RoomListComponent } from './feature/rooming/components/room-list/room-list.component';
import { RoomAddComponent } from './feature/rooming/components/room-add/room-add.component';
import { RoomEditComponent } from './feature/rooming/components/room-edit/room-edit.component';
import { RoomTypeListComponent } from './feature/rooming/components/room-type-list/room-type-list.component';
import { RoomTypeAddComponent } from './feature/rooming/components/room-type-add/room-type-add.component';
import { RoomTypeEditComponent } from './feature/rooming/components/room-type-edit/room-type-edit.component';
import { GuestListComponent } from './feature/guests/components/list/guest-list.component';
import { GuestAddComponent } from './feature/guests/components/add/guest-add.component';
import { GuestEditComponent } from './feature/guests/components/edit/guest-edit.component';
import { DashboardComponent } from './feature/dashboard/components/dashboard.component';

export const routes: Routes = [
  {
    path: '',
    component: AuthorizedLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'roles',
        component: RoleListcomponent,
      },
      {
        path: 'roles/add',
        component: RoleAddComponent,
      },
      {
        path: 'roles/edit/:id',
        component: RoleEditComponent,
      },
      {
        path: 'users',
        component: UserListComponent,
      },
      {
        path: 'users/add',
        component: UserAddComponent,
      },
      {
        path: 'users/edit/:id',
        component: UserEditComponent,
      },
      {
        path: 'rooms',
        component: RoomListComponent,
      },
      {
        path: 'rooms/add',
        component: RoomAddComponent,
      },
      {
        path: 'rooms/edit/:id',
        component: RoomEditComponent,
      },
      {
        path: 'room-types',
        component: RoomTypeListComponent,
      },
      {
        path: 'room-types/add',
        component: RoomTypeAddComponent,
      },
      {
        path: 'room-types/edit/:id',
        component: RoomTypeEditComponent,
      },
      {
        path: 'guests',
        component: GuestListComponent,
      },
      {
        path: 'guests/add',
        component: GuestAddComponent,
      },
      {
        path: 'guests/edit/:id',
        component: GuestEditComponent,
      },
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
    ],
  },
  {
    path: '',
    component: UnauthorizedLayoutComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent,
      },
    ],
  },
];
