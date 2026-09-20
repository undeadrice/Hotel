import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { AuthService } from '../../core/auth/auth.service';
import { ConfigurationService } from '../../feature/configurations/services/configuration.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-authorized-layout',
  imports: [
    RouterModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatButtonModule,
    MatMenuModule,
    MatIconModule,
    MatDividerModule,
    MatSnackBarModule,
  ],
  templateUrl: './authorized-layout.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthorizedLayoutComponent {
  authService = inject(AuthService);
  private readonly configurationService = inject(ConfigurationService);
  private readonly snackBar = inject(MatSnackBar);

  performEndOfDay(): void {
    this.configurationService.performEndOfDay().subscribe({
      next: (date) => {
        this.snackBar.open(`End of day performed. New business date: ${date}`, 'Close', {
          duration: 3000,
        });
      },
    });
  }
}
